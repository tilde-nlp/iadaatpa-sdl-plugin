using Sdl.LanguagePlatform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iADAATPA.MTProvider.Tests.Extensions
{
    public static class SegmentExtensions
    {
        public static Segment ToSegment(this string toConvert)
        {
            Segment wrapper = new Segment();
            wrapper.Add(toConvert);
            return wrapper;
        }
    }
}
