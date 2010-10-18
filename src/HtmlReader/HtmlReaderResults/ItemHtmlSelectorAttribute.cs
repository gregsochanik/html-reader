using System;

namespace HtmlReader.HtmlReaderResults
{
    /// <summary>
    /// An attribute that is supposed to decouple the search re
    /// </summary>
    public class ItemHtmlSelectorAttribute : Attribute
    {
        public HtmlSelectorType SelectorType { get; set; }
        public string SelectorKey { get; set; }

        public ItemHtmlSelectorAttribute() : base()
        {}
        public ItemHtmlSelectorAttribute(HtmlSelectorType selectorType, string selectorKey)
        {
            SelectorType = selectorType;
            SelectorKey = selectorKey;
        }
    }

    public enum HtmlSelectorType
    {
        CssSelector = 0,
        XPath = 1,
        RegularExpression = 2
    }
}
