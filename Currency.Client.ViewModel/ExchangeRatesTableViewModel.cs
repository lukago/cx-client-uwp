using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Currency.Client.Model.Entity;
using Currency.Client.Model.Service;
using Currency.Client.Model.Storage;

namespace Currency.Client.ViewModel
{
    public sealed class ExchangeRatesTableViewModel : INotifyPropertyChanged, IPersistable
    {
        private readonly ICurrencyTableService _currencyTableService;
        private readonly IDao<Props> _dao;

        private DateTimeOffset _dateTime;
        private DateTimeOffset _dateTimeDetailStart;
        private DateTimeOffset _dateTimeDetailEnd;
        private Rate _rate;
        private bool _empty;
        private bool _loadingDetails;
        private double _loadingDetailsProgress;

        public ExchangeRatesTableViewModel()
        {
        }

        public ExchangeRatesTableViewModel(ICurrencyTableService currencyTableService, IDao<Props> dao)
        {
            _currencyTableService = currencyTableService;
            _dao = dao;
            IsEmpty = true;
            _dateTime = DateTimeOffset.Now;
            _dateTimeDetailStart = DateTimeOffset.Now;
            _dateTimeDetailEnd = DateTimeOffset.Now;
            Rates = new ObservableCollection<Rate>();
            RatesDetails = new ObservableCollection<Rate>();
            LoadStateCommand = new DelegateCommand(async () => await LoadStateAsync());
            LoadRatesCommand = new DelegateCommand(async () => await LoadRatesAsync(SelectedDate.DateTime));
            LoadDetailsCommand = new DelegateCommand(async () =>
                await LoadDetailsAsync(SelectedRate, SelectedDateDetailStart, SelectedDateDetailEnd));
            LoadStateCommand.Execute(null);
        }

        public ObservableCollection<Rate> Rates { get; set; }
        public ObservableCollection<Rate> RatesDetails { get; set; }
        public ICommand LoadRatesCommand { get; set; }
        public ICommand LoadDetailsCommand { get; set; }
        public ICommand LoadStateCommand { get; set; }

        public int MaxYear => DateTime.Now.Year;
        public int MinYear => 2002;

        public bool IsEmpty
        {
            get => _empty;
            set
            {
                _empty = value;
                OnPropertyChanged();
            }
        }

        public DateTimeOffset SelectedDate
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                SelectedDateDetailEnd = value;
                SelectedDateDetailStart = SelectedDateDetailEnd.AddYears(-1);
                OnPropertyChanged();
                LoadRatesCommand.Execute(null);
            }
        }

        public DateTimeOffset SelectedDateDetailStart
        {
            get => _dateTimeDetailStart;
            set
            {
                _dateTimeDetailStart = value;
                OnPropertyChanged();
                LoadDetailsCommand.Execute(null);
            }
        }

        public DateTimeOffset SelectedDateDetailEnd
        {
            get => _dateTimeDetailEnd;
            set
            {
                _dateTimeDetailEnd = value;
                OnPropertyChanged();
                LoadDetailsCommand.Execute(null);
            }
        }

        public Rate SelectedRate
        {
            get => _rate;
            set
            {
                _rate = value;
                OnPropertyChanged();
                LoadDetailsCommand.Execute(null);
            }
        }

        public bool IsLoading { get; set; }
 
        public bool IsLoadingDetails
        {
            get => _loadingDetails;
            set
            {
                _loadingDetails = value;
                OnPropertyChanged();
            }
        }

        public double LoadingDetailsProgress
        {
            get => _loadingDetailsProgress;
            set
            {
                _loadingDetailsProgress = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Persist(object sender, object o)
        {
            _dao.WriteAsync(new Props(SelectedDate, SelectedRate));
        }

        public async Task LoadRatesAsync(DateTime dateTime)
        {
            IsLoading = true;
            List<Rate> rates = await _currencyTableService.FetchRatesForDateAsync(dateTime);
            IsEmpty = rates.Count == 0;
            Rates.Clear();
            rates.ForEach(Rates.Add);
            DownloadList<Rate> ratesWithFlag = _currencyTableService.FetchFlagsForRates(rates);
            while (!ratesWithFlag.IsFinished()) UpdateRatesWithNewValue(await ratesWithFlag.GetNextFinishedAsync());
            IsLoading = false;
        }

        public async Task LoadStateAsync()
        {
            IsLoading = true;
            var props = await _dao.ReadAsync();
            SelectedDate = props?.TableDateTime ?? SelectedDate;
            SelectedRate = props?.SelectedRate;
            IsLoading = false;
        }

        private async Task LoadDetailsAsync(Rate selectedRate,
            DateTimeOffset selectedDateDetailStart,
            DateTimeOffset selectedDateDetailEnd)
        {
            if (selectedRate == null) return;
            IsLoadingDetails = true;
            List<Rate> rates = await _currencyTableService.FetchRatesForCodeAsync(
                selectedRate, selectedDateDetailStart.DateTime, selectedDateDetailEnd.DateTime, (d) => LoadingDetailsProgress = d);
            
            RatesDetails = new ObservableCollection<Rate>(rates);
            OnPropertyChanged("RatesDetails");
            IsLoadingDetails = false;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateRatesWithNewValue(Rate rate)
        {
            var foundRate = Rates.SingleOrDefault(r => r.Code.Equals(rate.Code));
            if (foundRate != null) Rates[Rates.IndexOf(foundRate)] = rate;
        }
    }
}