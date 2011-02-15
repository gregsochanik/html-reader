using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace HtmlReader.Fizzler
{
	public interface IHtmlSelector
	{
		IHtmlSelector MoveToNode(string chain);
		IHtmlSelector ClickLink(string selector);
		IHtmlSelector GoBack();

		IEnumerable<HtmlNode> Build();
		IEnumerable<HtmlNode> SelectNodes(string selector);
		HtmlNode CurrentNode();
	}
}
