using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Currency.Client.Model.Api;
using Currency.Client.Model.Entity;
using Currency.Client.Model.Service;
using Currency.Client.Model.Storage;
using Currency.Client.ViewModel;

namespace Currency.Client.Uwp
{
    public sealed partial class ExchangeRateTableView : UserControl
    {
        public ExchangeRateTableView()
        {
            InitializeComponent();
            var viewModel = InitializeViewModel();
            DataContext = viewModel;
            viewModel.LoadStateCommand.Execute(null);
            Application.Current.Suspending += viewModel.Persist;
        }

        private static ExchangeRatesTableViewModel InitializeViewModel()
        {
            return new ExchangeRatesTableViewModel(
                new CurrencyTableService(
                    new HttpNbpClient(new HttpClient()),
                    new HttpRestCountriesClient(new HttpClient())),
                new JsonDao<Props>(
                    json => Task.Run(() =>
                        ApplicationData.Current.LocalSettings.Values["ExchangeRatesTableViewModel"] = json),
                    () => Task.Run(() =>
                        ApplicationData.Current.LocalSettings.Values["ExchangeRatesTableViewModel"] as string)));
        }
    }
}