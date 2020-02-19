using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Currency.Client.Model.Api;
using Currency.Client.Model.Entity;
using Currency.Client.Model.Service;
using Currency.Client.Model.Storage;
using Currency.Client.ViewModel;

namespace Currency.Client.Uwp
{
    public class UwpLocator
    {
        public static ExchangeRatesTableViewModel RatesTable { get; } = InitializeRatesTableViewModel();

        private static ExchangeRatesTableViewModel InitializeRatesTableViewModel()
        {
            var viewModel = new ExchangeRatesTableViewModel(
                new CurrencyTableService(
                    new HttpNbpClient(new HttpClient()),
                    new HttpRestCountriesClient(new HttpClient())),
                new JsonDao<Props>(
                    json => Task.Run(() =>
                        ApplicationData.Current.LocalSettings.Values["ExchangeRatesTableViewModel"] = json),
                    () => Task.Run(() =>
                        ApplicationData.Current.LocalSettings.Values["ExchangeRatesTableViewModel"] as string)));
            Application.Current.Suspending += viewModel.Persist;
            return viewModel;
        }
    }
}