using System.Collections.Generic;
using HtmlAgilityPack;

namespace HtmlReader.Fizzler
{
	public interface ISelector
	{
		HtmlNode SelectNode(string query);
		IEnumerable<HtmlNode> SelectNodes(string query);
	}
}