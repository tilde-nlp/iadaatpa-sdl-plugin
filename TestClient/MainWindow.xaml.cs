using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iADAATPA.MTProvider;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using TestClient.ViewModels;

namespace TestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var store = new TranslationProviderCredentialStore();
            Uri uri = new TranslationProviderUriBuilder("iadaatpa").Uri;
            store.AddCredential(uri, new TranslationProviderCredential("validToken", true));
            var factory = new TranslationProviderFactory();
            var provider = factory.CreateTranslationProvider(uri, null, store);
            var vm = new MainViewModel(provider);
            DataContext = vm;
        }
    }
}
