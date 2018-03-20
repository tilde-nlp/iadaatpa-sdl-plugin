using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace iADAATPA.MTProvider
{
    [TranslationProviderWinFormsUi(
        Id = "Translation_Provider_Plug_inWinFormsUI",
        Name = "Translation_Provider_Plug_inWinFormsUI",
        Description = "Translation_Provider_Plug_inWinFormsUI")]
    public class TranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
    {
        #region ITranslationProviderWinFormsUI Members

        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            throw new NotImplementedException();
        }

        public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            throw new NotImplementedException();
        }

        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            throw new NotImplementedException();
        }

        public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
        {
            throw new NotImplementedException();
        }

        public bool SupportsEditing => true;

        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
            => string.Equals(translationProviderUri.Scheme, PluginResources.Plugin_UriSchema, StringComparison.InvariantCultureIgnoreCase); // TODO: check what is actually passed into here

        public string TypeDescription => PluginResources.Plugin_Description;

        public string TypeName => PluginResources.Plugin_Name + "...";

        #endregion
    }
}
