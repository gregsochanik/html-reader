using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace HtmlReaderTest
{
    public abstract class SelectorBaseTest
    {
        private const string Html = "@<html><head></head><body><div><p class='content'>Fizzler</p><p>CSS Selector Engine</p></div></body></html>";

        protected SelectorBaseTest()
        {
            string html = Html;
            var document = new HtmlDocument();
            document.LoadHtml(html);
            Document = document;
        }

        protected HtmlDocument Document { get; private set; }

        protected IEnumerable<HtmlNode> Select(string selectorChain)
        {
            return Document.DocumentNode.QuerySelectorAll(selectorChain);
        }

        protected IList<HtmlNode> SelectList(string selectorChain)
        {
            return new ReadOnlyCollection<HtmlNode>(Select(selectorChain).ToArray());
        }
    }
}
