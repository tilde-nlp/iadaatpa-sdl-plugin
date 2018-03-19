using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace iADAATPA.MTProvider
{
    [TranslationProviderFactory(
        Id = "Translation_Provider_Plug_inFactory",
        Name = "Translation_Provider_Plug_inFactory",
        Description = "Translation_Provider_Plug_inFactory")]
    public class TranslationProviderFactory : ITranslationProviderFactory
    {
        #region ITranslationProviderFactory Members

        public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            throw new NotImplementedException();
        }

        public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
            => new TranslationProviderInfo { Name = PluginResources.Plugin_Name, TranslationMethod = TranslationMethod.MachineTranslation };

        public bool SupportsTranslationProviderUri(Uri translationProviderUri) => translationProviderUri.ToString().StartsWith("iadaatpa");

        #endregion
    }
}
