using System;
using System.Net;

namespace HtmlReader.Fizzler
{
	public class HtmlLoader : IHtmlLoader {

		private readonly Uri _webPage;
		public HtmlLoader(Uri webPage) {
			_webPage = webPage;
		}

		public string Load() {
			var webclient = new WebClient();
			return webclient.DownloadString(_webPage);
		}
	}

	public class StringHtmlLoader : IHtmlLoader
	{
		private readonly string _html;
		public StringHtmlLoader(string html) {
			_html = html;
		}

		public string Load() {
			return _html;
		}
	}
}