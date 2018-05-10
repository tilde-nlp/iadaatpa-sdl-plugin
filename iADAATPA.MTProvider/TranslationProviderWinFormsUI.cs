using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using iADAATPA.MTProvider.API;
using iADAATPA.MTProvider.ViewModels;
using iADAATPA.MTProvider.Views;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace iADAATPA.MTProvider
{
    [TranslationProviderWinFormsUi(
        Id = "iADAATPA_Translation_Provider_Plugin_WinFormsUI",
        Name = "iADAATPA Translator UI",
        Description = "User interface for the iADAATPA Translator plugin")]
    public class TranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
    {
        private IClient _client = new API.Client(PluginResources.iADAATPA_API);

        //private static Uri providerUri => new TranslationProviderUriBuilder(PluginResources.Plugin_UriSchema).Uri;

        private static Uri generateProviderUri(ITranslationProviderCredentialStore store)
        {
            Uri makeUri(int uriNum)
                => new Uri($"{PluginResources.Plugin_UriSchema}:///proj-{uriNum.ToString()}");

            int i = 0;
            // find the first unused uri
            while (store.GetCredential(makeUri(i)) != null)
            {
                i++;
            }
            return makeUri(i);
        }

        #region ITranslationProviderWinFormsUI Members

        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            var newUri = generateProviderUri(credentialStore);

            if (!GetCredentialsFromUser(owner, newUri, null, credentialStore))
            {
                return null;
            }
            var factory = new TranslationProviderFactory();
            ITranslationProvider provider = factory.CreateTranslationProvider(newUri, null, credentialStore);
            return new ITranslationProvider[] { provider };
        }

        public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
            => GetCredentialsFromUser(owner, translationProvider.Uri, null, credentialStore);

        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            var authViewModel = new AuthViewModel(_client);
            string existingToken = credentialStore.GetCredential(translationProviderUri)?.Credential;
            authViewModel.AuthToken = existingToken;
            var authView = new AuthWindow(authViewModel);
            bool success = authView.ShowDialog() == true;
            if (!success)
            {
                return false;
            }
            string authToken = authViewModel.AuthToken;
            if (authToken != null)
            {
                credentialStore.AddCredential(translationProviderUri, new TranslationProviderCredential(authToken, true));
                return true;
            }
            else
            {
                // the user has deleted the token using the logout button
                credentialStore.RemoveCredential(translationProviderUri);

                // TODO: return something that will make Trados disable the plugin. Currently I don't think it's possible, though.
                // Returning false indicates that the settings haven't changed which isn't true but if true is returned the user
                // gets endless prompts from Trados to input credentials. This seems like a bug, though, so hopefully this will change.
                return false;
            }
        }

        public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
            => new TranslationProviderDisplayInfo() {
                Name = PluginResources.Plugin_Name,
                TooltipText = PluginResources.Plugin_Description,
                TranslationProviderIcon = PluginResources.iadaatpa_logo,
                SearchResultImage = PluginResources.iadaatpa_logo.ToBitmap()
            };

        public bool SupportsEditing => true;

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
            => string.Equals(translationProviderUri.Scheme, PluginResources.Plugin_UriSchema, StringComparison.InvariantCultureIgnoreCase); // TODO: check what is actually passed into here

        public string TypeDescription => PluginResources.Plugin_Description;

        public string TypeName => PluginResources.Plugin_Name + "...";

        #endregion
    }
}
