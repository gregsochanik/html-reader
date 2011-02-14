using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace HtmlReader.Fizzler
{
	public class FluentHtmlSelector : IHtmlSelector
	{
		private HtmlDocument _document;
		
		public FluentHtmlSelector(IHtmlLoader htmlLoader) {
			_document = new HtmlDocument();
			LoadHtml(htmlLoader.Load());
		}

		public string BaseUrl { get; set; }

		public IHtmlSelector MoveToNode(string selectorChain) {
			var htmlList = _document.DocumentNode.QuerySelector(selectorChain);
			if(htmlList == null)
				throw new ArgumentException(string.Format("{0}: This doesn't seem to be a valid selector, it has resulted in zero nodes found", selectorChain));

			LoadHtml(htmlList.OuterHtml);
			return this;
		}

		public IHtmlSelector ClickLink(string selectorChain) {
			HtmlAttribute htmlAttribute = _document.DocumentNode.QuerySelector(selectorChain).Attributes["href"];
			if(htmlAttribute == null)
				throw new ArgumentException(string.Format("{0}This doesn't seem to be a valid link, has no href attribute", selectorChain));

			var link = new Uri(BaseUrl + htmlAttribute.Value);
			var webclient = new WebClient();
			string html = webclient.DownloadString(link);
			LoadHtml(html);
			return this;
		}

		public IEnumerable<HtmlNode> Build() {
			return _document.DocumentNode.DescendantsAndSelf();
		}

		public IEnumerable<HtmlNode> SelectNodes(string selectorChain) {
			IEnumerable<HtmlNode> htmlList = _document.DocumentNode.QuerySelectorAll(selectorChain);

			if (htmlList == null)
				throw new ArgumentException(string.Format("{0}: This doesn't seem to be a valid selector, it has resulted in zero nodes found", selectorChain));

			return htmlList;
		}

		public HtmlNode CurrentNode() {
			return _document.DocumentNode;
		}

		private void LoadHtml(string html) {
			_history.Push(_document);
			_document.LoadHtml(html);
		}

		private Stack<HtmlDocument> _history = new Stack<HtmlDocument>();

		public IHtmlSelector GoBack() {
			_document = _history.Pop();
			return this;
		}
	}
}