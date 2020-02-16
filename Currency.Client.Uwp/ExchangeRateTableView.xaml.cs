using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Currency.Client.Model;
using Currency.Client.Model.Service;
using Currency.Client.ViewModel;

namespace Currency.Client.Uwp
{
    public sealed partial class ExchangeRateTableView : UserControl
    {
        public ExchangeRateTableView()
        {
            this.InitializeComponent();
            var viewModel = InitializeViewModel();
            DataContext = viewModel;
            Application.Current.Suspending += viewModel.Persist;
        }

        private static ExchangeRatesTableViewModel InitializeViewModel()
        {
            return new ExchangeRatesTableViewModel(
                new CurrencyTableService(new HttpNbpClient(new HttpClient()), new HttpRestCountriesClient(new HttpClient())),
                new JsonDao<Props>(
                    json => Task.Run(() => ApplicationData.Current.LocalSettings.Values["ExchangeRatesTableViewModel"] = json), 
                    () => Task.Run(() => ApplicationData.Current.LocalSettings.Values["ExchangeRatesTableViewModel"] as string)));
        }
    }
}
