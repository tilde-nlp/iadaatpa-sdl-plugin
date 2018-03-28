using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
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
        private static Uri providerUri => new TranslationProviderUriBuilder(PluginResources.Plugin_UriSchema).Uri;

        #region ITranslationProviderWinFormsUI Members

        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            var factory = new TranslationProviderFactory();
            return new ITranslationProvider[] { factory.CreateTranslationProvider(providerUri, null, credentialStore) };
        }

        public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
            => GetCredentialsFromUser(owner, providerUri, null, credentialStore);

        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            var authViewModel = new AuthViewModel();
            string existingToken = credentialStore.GetCredential(translationProviderUri)?.Credential;
            authViewModel.AuthToken = existingToken;
            var authView = new AuthWindow(authViewModel);
            bool success = authView.ShowDialog() == true;
            if (!success)
            {
                return false;
            }
            string authToken = authViewModel.AuthToken;
            credentialStore.AddCredential(providerUri, new TranslationProviderCredential(authToken, true));
            return true;
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
