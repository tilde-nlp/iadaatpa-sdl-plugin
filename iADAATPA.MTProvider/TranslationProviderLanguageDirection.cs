using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using iADAATPA.MTProvider.Extensions;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace iADAATPA.MTProvider
{
    public class TranslationProviderLanguageDirection : ITranslationProviderLanguageDirection
    {
        private LanguagePair _languageDirection;
        private API.IClient _client;
        private string src => _languageDirection.SourceCultureName;
        private string trg => _languageDirection.TargetCultureName;

        public TranslationProviderLanguageDirection(LanguagePair languageDirection, API.IClient client, ITranslationProvider parent)
        {
            _languageDirection = languageDirection;
            _client = client;
            TranslationProvider = parent; // It isn't (and probably shouldn't be) used internally. We hold on to the provider just because the API asks us to.
        }

        private ImportResult discardedResult => new ImportResult(Sdl.LanguagePlatform.TranslationMemory.Action.Discard);
        private ImportResult[] discardedResults(TranslationUnit[] translationUnits)
            => translationUnits.Select(x => discardedResult).ToArray();

        #region ITranslationProviderLanguageDirection Members

        public ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)
            => discardedResults(translationUnits); // The method is not supported;

        public ImportResult[] AddOrUpdateTranslationUnitsMasked(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings, bool[] mask)
            => discardedResults(translationUnits); // The method is not supported;

        public ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)
            => discardedResult; // The method is not supported;

        public ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)
            => discardedResults(translationUnits); // The method is not supported;

        public ImportResult[] AddTranslationUnitsMasked(TranslationUnit[] translationUnits, ImportSettings settings, bool[] mask)
            => discardedResults(translationUnits); // The method is not supported;

        public bool CanReverseLanguageDirection => false;

        public SearchResults SearchSegment(SearchSettings settings, Segment segment)
            => SearchSegments(settings, new Segment[] { segment }).First();

        public SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)
            => SearchSegmentsMasked(settings, segments, null);

        public SearchResults[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask)
        {
            IEnumerable<Segment> toTranslate = mask != null ? segments.Where((x, i) => mask[i]) : segments;
            List<string> sources = toTranslate.Select(x => x.ToHtml()).ToList();
            List<string> translations;
            try
            {
                translations = _client.Translate(sources, src, trg).Result;
            }
            catch (AggregateException e)
            {
                var httpException = e.InnerException as SimpleHttpResponseException ?? throw e.InnerException;
                if (httpException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new TranslationProviderAuthenticationException("Invalid Consumer API Token", httpException);
                }
                throw httpException;
            }

            IEnumerable<SearchResults> nonMaskedResults = toTranslate.Zip(translations,
                // Converting a translation string to SearchResults is quite involved
                (Segment sourceSegment, string translation) => {
                    var targetSegment = translation.ToSegment(sourceSegment);

                    var tu = new TranslationUnit(sourceSegment.Duplicate(), targetSegment);
                    tu.Origin = TranslationUnitOrigin.MachineTranslation;
                    tu.ResourceId = new PersistentObjectToken(tu.GetHashCode(), Guid.Empty); // TODO: is this needed?

                    var res = new SearchResult(tu) {
                        TranslationProposal = tu.Duplicate(),
                        ScoringResult = new ScoringResult() { BaseScore = 85 } // Although we do not support scoring, returning some kind of a score is mandatory, else Trados freezes
                    };
                    var results = new SearchResults { SourceSegment = sourceSegment.Duplicate() };
                    results.Add(res);
                    return results;
                });

            IEnumerable<SearchResults> allResults()
            {
                var enumerator = nonMaskedResults.GetEnumerator();
                foreach (var item in segments.Select((value, i) => new { i, value }))
                {
                    if (mask == null || mask[item.i])
                    {
                        enumerator.MoveNext();
                        yield return enumerator.Current;
                        continue;
                    }
                    yield return null;
                }
            }
            var resArray = allResults().ToArray();
            return resArray;
        }

        public SearchResults SearchText(SearchSettings settings, string segment)
        {
            Segment toTranslate = new Segment();
            toTranslate.Add(segment);
            return SearchSegment(settings, toTranslate);
        }

        public SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)
            => SearchTranslationUnits(settings, new TranslationUnit[] { translationUnit }).First();

        public SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] translationUnits)
            => SearchTranslationUnitsMasked(settings, translationUnits, null);

        public SearchResults[] SearchTranslationUnitsMasked(SearchSettings settings, TranslationUnit[] translationUnits, bool[] mask)
            => SearchSegmentsMasked(settings, translationUnits.Select(x => x.SourceSegment).ToArray(), mask);
            // TODO: maybe it's required that we preserve the target segments or something

        public System.Globalization.CultureInfo SourceLanguage => _languageDirection.SourceCulture;

        public System.Globalization.CultureInfo TargetLanguage => _languageDirection.TargetCulture;

        /// <summary>
        /// The property is here simply because <see cref="ITranslationProviderLanguageDirection"/> asks for it.
        /// NOTE: It's probably best to not use it to not break encapsulation and end up with an entangled mess of a code.
        /// </summary>
        public ITranslationProvider TranslationProvider { get; private set; }

        public ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)
            => discardedResult; // The method is not supported;

        public ImportResult[] UpdateTranslationUnits(TranslationUnit[] translationUnits)
            => discardedResults(translationUnits); // The method is not supported;

        #endregion
    }
}
