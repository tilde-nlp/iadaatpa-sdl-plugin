using iADAATPA.MTProvider.API;
using iADAATPA.MTProvider.Tests.Extensions;
using iADAATPA.MTProvider.Tests.Mocks;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace iADAATPA.MTProvider.Tests
{
    public class LanguageDirectionTests
    {
        [Theory]
        [InlineData(new []{ "source1", "source2" }, new []{ true, true}, new []{ "translation1", "translation2"}, new []{ "translation1", "translation2"})]
        [InlineData(new []{ "source1", "source2" }, new []{ false, true}, new []{"translation2"}, new []{ null, "translation2"})]
        [InlineData(new []{ "source1", "source2", "source3" }, new []{ true, true, false}, new []{"translation1", "translation2"}, new []{ "translation1", "translation2", null})]
        [InlineData(new []{ "source1", "source2", "source3" }, new []{ false, false, false}, new string[]{}, new string[]{ null, null, null})]
        public void Translate_Masked(string[] sources, bool[] masks, string[] translations, string[] expectedTargets)
        {
            var toTranslate = sources
                .Zip(masks, (source, mask) => new { source, mask })
                .Where(x => x.mask)
                .Select(x => x.source).ToList();
            var translationList = translations.ToList();
            string srcLang = "en";
            string trgLang = "lv";
            var mock = new Mocks.MockClient().MockTranslate(toTranslate, translationList, srcLang, trgLang);
            IClient client = mock.Object;

            var langPair = new LanguagePair(srcLang, trgLang);
            var langDir = new TranslationProviderLanguageDirection(langPair, client, null);
            Segment[] sourceSegments = sources.Select(x => x.ToSegment()).ToArray();
            SearchResults[] results = langDir.SearchSegmentsMasked(null, sourceSegments, masks);
            string[] resultTargets = results
                .SelectMany(x => x?.Select(elem => elem.TranslationProposal.TargetSegment.ToPlain()) ?? new string[] { null })
                .ToArray();
            Assert.Equal(expectedTargets, resultTargets);
        }
    }
}
