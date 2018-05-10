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

        private static Uri formatUri(int uriNum)
            => new Uri($"{PluginResources.Plugin_UriSchema}:///proj-{uriNum.ToString()}");

        private static Tuple<Uri, int> generateProviderUri(ITranslationProviderCredentialStore store)
        {
            int i = 0;
            // find the first unused uri
            while (store.GetCredential(formatUri(i)) != null)
            {
                i++;
            }
            return Tuple.Create(formatUri(i), i);
        }

        #region ITranslationProviderWinFormsUI Members

        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            // We use a new uri for each new project to which the plugin is added. This is ok by itself
            // but, when the plugin is removed, I don't see a way to remove the respective credential.
            // So credentials just keep on piling up over time and the only way they get removed is via
            // the logout button. An alternative is to revert to a single API key for all projects
            // (the implementation can be found in some previous commit).
            var unused = generateProviderUri(credentialStore);
            Uri newUri = unused.Item1;
            int uriNum = unused.Item2;

            string prevAuthToken = credentialStore.GetCredential(formatUri(uriNum - 1))?.Credential;
            if (!GetCredentialsFromUser(owner, newUri, null, credentialStore, prevAuthToken))
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
            => GetCredentialsFromUser(owner, translationProviderUri, translationProviderState, credentialStore, null);

        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore, string initialAuthToken)
        {
            var authViewModel = new AuthViewModel(_client);
            string existingToken = credentialStore.GetCredential(translationProviderUri)?.Credential ?? initialAuthToken;
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
