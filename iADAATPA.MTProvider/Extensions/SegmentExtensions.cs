using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace iADAATPA.MTProvider.Extensions
{
    public static class SegmentExtensions
    {
        public static string XmlEscape(this string unescaped)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateElement("root");
            node.InnerText = unescaped;
            return node.InnerXml;
        }

        public static string ToHtml(this Sdl.LanguagePlatform.Core.Segment segment)
        {
            var stringBuilder = new StringBuilder();
            foreach (var element in segment.Elements)
            {
                var text = element as Sdl.LanguagePlatform.Core.Text;
                if (text != null)
                {
                    stringBuilder.Append(text.Value.XmlEscape());
                }
                else
                {
                    var tag = element as Sdl.LanguagePlatform.Core.Tag;
                    if (tag != null)
                    {
                        switch (tag.Type)
                        {
                            case Sdl.LanguagePlatform.Core.TagType.Start:
                                stringBuilder.AppendFormat("<span class='{0}' id='{1}'>",
                                        tag.Type, tag.Anchor);
                                break;
                            case Sdl.LanguagePlatform.Core.TagType.End:
                                stringBuilder.AppendFormat("</span>");
                                break;
                            case Sdl.LanguagePlatform.Core.TagType.Standalone:
                            case Sdl.LanguagePlatform.Core.TagType.TextPlaceholder:
                            case Sdl.LanguagePlatform.Core.TagType.LockedContent:
                                //stringBuilder.AppendFormat("<span class='{0}' id='{1}'></span>",=tag.Type, tag.Anchor);
                                stringBuilder.AppendFormat("<span class='{0}' id='{1}'></span> ",
                                         tag.Type, tag.Anchor);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return stringBuilder.ToString();
        }

        public static Sdl.LanguagePlatform.Core.Segment ToSegment(this string translatedText, Sdl.LanguagePlatform.Core.Segment sourceSegment)
        {
            var htmlTagName = "span"; // the only we feed for translation is span, so we expect the translation only has span tags too.
            var xmlFragment = "<segment>" + translatedText + "</segment>";
            var xmlReader = new System.Xml.XmlTextReader(xmlFragment, System.Xml.XmlNodeType.Element, null);
            var tagStack = new Stack<Sdl.LanguagePlatform.Core.Tag>();
            var translatedSegment = new Sdl.LanguagePlatform.Core.Segment();
            try
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            if (xmlReader.Name == htmlTagName)
                            {
                                var tagClass = xmlReader.GetAttribute("class");
                                var tagType = (Sdl.LanguagePlatform.Core.TagType)
                                     Enum.Parse(typeof(Sdl.LanguagePlatform.Core.TagType), tagClass);
                                int id = Convert.ToInt32(xmlReader.GetAttribute("id"));
                                Sdl.LanguagePlatform.Core.Tag sourceTag = sourceSegment.FindTag(tagType, id);
                                if (tagType != Sdl.LanguagePlatform.Core.TagType.Standalone && !xmlReader.IsEmptyElement)
                                {
                                    tagStack.Push(sourceTag);
                                }
                                translatedSegment.Add(sourceTag.Duplicate());
                                if (tagType != Sdl.LanguagePlatform.Core.TagType.Standalone && xmlReader.IsEmptyElement)
                                // the API translated <span></span> to <span/> (it does that if the tag is empty).
                                // must fetch the end tag as there is no EndElement to triger the next case block.
                                {
                                    var endTag = sourceSegment.FindTag(Sdl.LanguagePlatform.Core.TagType.End, id);
                                    translatedSegment.Add(endTag.Duplicate());
                                }
                            }
                            break;
                        case System.Xml.XmlNodeType.EndElement:
                            {
                                if (xmlReader.Name == htmlTagName)
                                {
                                    var startTag = tagStack.Pop();
                                    if (startTag.Type != Sdl.LanguagePlatform.Core.TagType.Standalone)
                                    {
                                        var endTag = sourceSegment.FindTag(
                                           Sdl.LanguagePlatform.Core.TagType.End, startTag.Anchor);
                                        if (endTag != null)
                                            translatedSegment.Add(endTag.Duplicate());
                                    }
                                }
                            }
                            break;
                        case System.Xml.XmlNodeType.Text:
                            translatedSegment.Add(xmlReader.Value);
                            break;
                        case System.Xml.XmlNodeType.Whitespace:
                            translatedSegment.Add(xmlReader.Value);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                var paintextSegment = new Sdl.LanguagePlatform.Core.Segment();
                string plaitext = Regex.Replace(translatedText, "<[^>]+>", "");
                paintextSegment.Add(plaitext);
                return paintextSegment;
            }

            return translatedSegment;
        }
    }
}
