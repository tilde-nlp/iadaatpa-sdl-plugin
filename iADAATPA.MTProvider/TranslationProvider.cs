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
        #region ITranslationProvider Members

        public ITranslationProviderLanguageDirection GetLanguageDirection(LanguagePair languageDirection)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void LoadState(string translationProviderState)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public void RefreshStatusInfo()
        {
            throw new NotImplementedException();
        }

        public string SerializeState()
        {
            throw new NotImplementedException();
        }

        public ProviderStatusInfo StatusInfo
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsConcordanceSearch => false;
        public bool SupportsDocumentSearches => false;
        public bool SupportsFilters => false;
        public bool SupportsFuzzySearch => false;

        public bool SupportsLanguageDirection(LanguagePair languageDirection)
        {
            throw new NotImplementedException();
        }

        public bool SupportsMultipleResults => false;
        public bool SupportsPenalties => false;

        public bool SupportsPlaceables
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsScoring => false;

        public bool SupportsSearchForTranslationUnits
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsSourceConcordanceSearch => false;
        public bool SupportsStructureContext => false;

        public bool SupportsTaggedInput
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsTargetConcordanceSearch => false;

        public bool SupportsTranslation
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsUpdate
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsWordCounts => false;

        public TranslationMethod TranslationMethod
        {
            get { throw new NotImplementedException(); }
        }

        public Uri Uri
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}

