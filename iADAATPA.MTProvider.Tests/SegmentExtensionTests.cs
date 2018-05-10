using Xunit;
using iADAATPA.MTProvider.Extensions;
using Sdl.LanguagePlatform.Core;
using iADAATPA.MTProvider.Tests.Extensions;

namespace iADAATPA.MTProvider.Tests
{
    public class SegmentExtensionTests
    {
        [Fact]
        public void Convert_Plaintext_to_Html()
        {
            string simple = "Here is some simple text without tags.";
            Segment wrapper = simple.ToSegment();
            Assert.Equal(simple, wrapper.ToHtml());
        }
    }
}
