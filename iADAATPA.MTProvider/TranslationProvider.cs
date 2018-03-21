using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace iADAATPA.MTProvider
{
    public class TranslationProvider : ITranslationProvider
    {
        API.Client _client;
        public TranslationProvider(Uri providerUri, API.Client client)
        {
            _client = client;
            Uri = providerUri;
        }

        #region ITranslationProvider Members
        public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
        {
            return new TranslationProviderLanguageDirection(languageDirection, _client, this);
        }

        public bool IsReadOnly => false; // TODO: don't know about this
        public void LoadState(string translationProviderState) { }  // don't have any state yet except for the auth token which is stored in credentials
        public string Name => PluginResources.Plugin_Name;
        public void RefreshStatusInfo() { }
        public string SerializeState() => ""; // don't have any state yet except for the auth token which is stored in credentials
        public ProviderStatusInfo StatusInfo => new ProviderStatusInfo(true, PluginResources.Plugin_Name);
        public bool SupportsConcordanceSearch => false;
        public bool SupportsDocumentSearches => false;
        public bool SupportsFilters => false;
        public bool SupportsFuzzySearch => false;
        public bool SupportsLanguageDirection(LanguagePair languageDirection) => true; // TODO: don't know yet of any limitations
        public bool SupportsMultipleResults => false;
        public bool SupportsPenalties => false;
        public bool SupportsPlaceables => false; // TODO: don't know what these are so let's leave it at false for now
        public bool SupportsScoring => false;
        public bool SupportsSearchForTranslationUnits => false; // TODO: think this should be true, but lets leavi it for now to see what happens
        public bool SupportsSourceConcordanceSearch => false;
        public bool SupportsStructureContext => false;
        public bool SupportsTaggedInput => false; // TODO: fix this
        public bool SupportsTargetConcordanceSearch => false;
        public bool SupportsTranslation => true;
        public bool SupportsUpdate => false;
        public bool SupportsWordCounts => false;
        public TranslationMethod TranslationMethod => TranslationMethod.MachineTranslation;

        public Uri Uri
        {
            get; private set;
        }

        #endregion
    }
}

