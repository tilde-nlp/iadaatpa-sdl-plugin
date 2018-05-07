using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using iADAATPA.MTProvider;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using TestClient.ViewModels;

namespace TestClient
{
    /// <summary>
    /// This is a simplistic implementation of the SDL Trados Studio's plugin handling logic, just so
    /// that we can test the UI and translation parts of our plugin without running SDL Trados Studio.
    /// Furthermore, the implementation is specific to the iADAATPA plugin, therefore we can get away
    /// by passing nulls and calling only a subset of the plugin's methods.
    /// </summary>
    public partial class MainWindow : Window
    {
        ITranslationProviderWinFormsUI _ui = new TranslationProviderWinFormsUI();
        TranslationProviderCredentialStore _store = new TranslationProviderCredentialStore();
        Uri _uri = new TranslationProviderUriBuilder("iadaatpa").Uri;

        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists("./settings.xml")){
                using (XmlReader reader = XmlReader.Create("./settings.xml"))
                {
                    _store.ReadXml(reader);
                }
            }
            else {
                _ui.Edit(null, null, null, _store);
                SaveStore();
            }
            SetupTranslation();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ui.Edit(null, null, null, _store);
            SaveStore();
            SetupTranslation();
        }

        private void SaveStore()
        {
            using (XmlWriter writer = XmlWriter.Create("./settings.xml"))
            {
                _store.WriteXml(writer);
            }
        }

        private void SetupTranslation()
        {
            var factory = new TranslationProviderFactory();
            var provider = factory.CreateTranslationProvider(_uri, null, _store);
            var vm = new MainViewModel(provider);
            DataContext = vm;
        }
    }
}
