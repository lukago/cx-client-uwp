using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Currency.Client.Model;
using Currency.Client.Model.Service;

namespace Currency.Client.ViewModel
{
    public sealed class ExchangeRatesTableViewModel : INotifyPropertyChanged, IPersistable
    {
        private readonly IDao<Props> _dao;
        private readonly ICurrencyTableService _currencyTableService;

        public ExchangeRatesTableViewModel()
        {
        }

        public ExchangeRatesTableViewModel(ICurrencyTableService currencyTableService, IDao<Props> dao)
        {
            _currencyTableService = currencyTableService;
            _dao = dao;
            IsLoading = false;
            SelectedDate = DateTime.Now;
            Rates = new ObservableCollection<Rate>();
            LoadStateCommand = new DelegateCommand(async () => await LoadStateAsync());
            LoadRatesCommand = new DelegateCommand(async () => await LoadRatesAsync(SelectedDate));
            LoadStateCommand.Execute(null);
        }

        public ObservableCollection<Rate> Rates { get; set; }
        public ICommand LoadRatesCommand { get; set; }
        public ICommand LoadStateCommand { get; set; }
        public DateTime SelectedDate { get; set; }
        public bool IsLoading { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Persist(object sender, object o)
        {
            _dao.WriteAsync(new Props(SelectedDate));
        }

        public async Task LoadRatesAsync(DateTime dateTime)
        {
            IsLoading = true;
            List<Rate> rates = await _currencyTableService.FetchRatesForDateWithFlagsAsync(dateTime);
            Rates.Clear();
            rates.ForEach(Rates.Add);
            IsLoading = false;
        }

        public async Task LoadStateAsync()
        {
            IsLoading = true;
            Props props = await _dao.ReadAsync();
            SelectedDate = props?.TableDateTime ?? SelectedDate;
            LoadRatesCommand.Execute(null);
            IsLoading = false;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}