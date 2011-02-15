using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace HtmlReaderTest
{
    public abstract class SelectorBaseTest
    {
        private const string HTML = "@<html><head></head><body><div><p class='content'>Fizzler</p><p>CSS Selector Engine</p></div></body></html>";

        protected SelectorBaseTest()
        {
            var document = new HtmlDocument();
			document.LoadHtml(HTML);
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
