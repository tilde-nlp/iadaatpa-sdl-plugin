using iADAATPA.MTProvider.API;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iADAATPA.MTProvider.Tests.Mocks
{
    public class MockClient : Mock<IClient>
    {
        public MockClient MockTranslate(List<string> sources, List<string> retTranslations, string sourceLang, string targetLang)
        {
            Setup(x => x.Translate(sources, sourceLang, targetLang))
                .ReturnsAsync(retTranslations);
            return this;
        }
    }
}
