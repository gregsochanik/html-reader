using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;
using HtmlReader.Fizzler;
using NUnit.Framework;
using Rhino.Mocks;

namespace HtmlReader.Unit.Tests {
	
	[TestFixture]
	public class FluentSelectorTests
	{
		private IHtmlLoader _htmlLoader;
		private const string HTML = @"<html>
										<head></head>
										<body>
											<div id=""div1""><p class='content'>Fizzler</p><p>CSS Selector Engine</p></div>
											<div id=""div2"">2nd div</div>
										</body>
										</html>";

		private const string HTML_WITH_LINK = "<html><head></head><body><a href=\"http://www.google.co.uk\" id=\"googleLink\">google</a></body></html>";

		private const string HTML_WITH_TABLE = "<table width=\"474\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\"><tbody><tr><td colspan=\"3\"><img src=\"/images/general/clear.gif\" width=\"4\" height=\"4\" alt=\"\" border=\"0\"></td></tr><tr><td class=\"gradImageBG\"><img src=\"/images/go_racing/racing/goraceImg/1.1.8_top.gif\" alt=\"\" width=\"233\" height=\"160\" border=\"0\"></td><td><img src=\"/images/general/clear.gif\" width=\"4\" height=\"4\" alt=\"\" border=\"0\"></td><td style=\"padding-right:4px;\" valign=\"top\"><img src=\"/images/go_racing/racing/goraceImg/1.1.8_top.jpg\" alt=\"\" width=\"233\" height=\"160\" border=\"0\"></td></tr><tr><td colspan=\"3\"><img src=\"/images/general/hr_green.gif\" width=\"470\" height=\"1\" alt=\"\" border=\"0\"></td></tr><tr><td colspan=\"3\" class=\"cnt_txt\" style=\"padding-left:5px; padding-right:5px; padding-top:5px\"><!-- Place content below-->Archive<br><br>12 February 2011 - <a href=\"meeting.asp?i=939\">Ayr</a>, <a href=\"meeting.asp?i=940\">Lingfield</a>, <a href=\"meeting.asp?i=941\">Newbury</a>, <a href=\"meeting.asp?i=942\">Warwick</a>, <a href=\"meeting.asp?i=943\">Wolverhampton</a>, <br><br>11 February 2011 - <a href=\"meeting.asp?i=934\">Bangor</a>, <a href=\"meeting.asp?i=935\">Kempton</a>, <a href=\"meeting.asp?i=936\">Musselburgh</a>, <a href=\"meeting.asp?i=937\">Southwell</a>, <a href=\"meeting.asp?i=938\">Wolverhampton</a>, <br><br>10 February 2011 - <a href=\"meeting.asp?i=930\">Huntingdon</a>, <a href=\"meeting.asp?i=931\">Kempton</a>, <a href=\"meeting.asp?i=932\">Southwell</a>, <a href=\"meeting.asp?i=933\">Taunton</a>, <br><br>09 February 2011 - <a href=\"meeting.asp?i=926\">Carlisle</a>, <a href=\"meeting.asp?i=927\">Kempton</a>, <a href=\"meeting.asp?i=928\">Lingfield</a>, <a href=\"meeting.asp?i=929\">Ludlow</a>, <br><br>08 February 2011 - <a href=\"meeting.asp?i=923\">Market Rasen</a>, <a href=\"meeting.asp?i=924\">Sedgefield</a>, <a href=\"meeting.asp?i=925\">Southwell</a>, <br><br>07 February 2011 - <a href=\"meeting.asp?i=920\">Ayr</a>, <a href=\"meeting.asp?i=921\">Lingfield</a>, <a href=\"meeting.asp?i=922\">Wolverhampton</a>, <br><br>06 February 2011 - <a href=\"meeting.asp?i=918\">Fontwell</a>, <a href=\"meeting.asp?i=919\">Musselburgh</a>, <br><br>05 February 2011 - <a href=\"meeting.asp?i=914\">Ffos Las</a>, <a href=\"meeting.asp?i=915\">Lingfield</a>, <a href=\"meeting.asp?i=916\">Sandown</a>, <a href=\"meeting.asp?i=917\">Wetherby</a>, <br><br>04 February 2011 - <a href=\"meeting.asp?i=909\">Bangor</a>, <a href=\"meeting.asp?i=910\">Chepstow</a>, <a href=\"meeting.asp?i=911\">Catterick</a>, <a href=\"meeting.asp?i=912\">Lingfield</a>, <a href=\"meeting.asp?i=913\">Wolverhampton</a>, <br><br>03 February 2011 - <a href=\"meeting.asp?i=905\">Southwell</a>, <a href=\"meeting.asp?i=906\">Towcester</a>, <a href=\"meeting.asp?i=907\">Wincanton</a>, <a href=\"meeting.asp?i=908\">Wolverhampton</a>, <br><br>02 February 2011 - <a href=\"meeting.asp?i=901\">Kempton</a>, <a href=\"meeting.asp?i=902\">Leicester</a>, <a href=\"meeting.asp?i=903\">Lingfield</a>, <a href=\"meeting.asp?i=904\">Newcastle</a>, <br><br>01 February 2011 - <a href=\"meeting.asp?i=898\">Folkestone</a>, <a href=\"meeting.asp?i=899\">Southwell</a>, <a href=\"meeting.asp?i=900\">Taunton</a>, <br><br>31 January 2011 - <a href=\"meeting.asp?i=895\">Ayr</a>, <a href=\"meeting.asp?i=896\">Ludlow</a>, <a href=\"meeting.asp?i=897\">Wolverhampton</a>, <br><br>30 January 2011 - <a href=\"meeting.asp?i=892\">Fakenham</a>, <a href=\"meeting.asp?i=893\">Hereford</a>, <a href=\"meeting.asp?i=894\">Kempton</a>, <br><br>29 January 2011 - <a href=\"meeting.asp?i=888\">Cheltenham</a>, <a href=\"meeting.asp?i=889\">Doncaster</a>, <a href=\"meeting.asp?i=890\">Lingfield</a>, <a href=\"meeting.asp?i=891\">Uttoxeter</a>, <br><br>28 January 2011 - <a href=\"meeting.asp?i=884\">Doncaster</a>, <a href=\"meeting.asp?i=885\">Fontwell</a>, <a href=\"meeting.asp?i=886\">Lingfield</a>, <a href=\"meeting.asp?i=887\">Wolverhampton</a>, <br><br>27 January 2011 - <a href=\"meeting.asp?i=880\">Ffos Las</a>, <a href=\"meeting.asp?i=881\">Kempton</a>, <a href=\"meeting.asp?i=882\">Southwell</a>, <a href=\"meeting.asp?i=883\">Warwick</a>, <br><br>26 January 2011 - <a href=\"meeting.asp?i=876\">Huntingdon</a>, <a href=\"meeting.asp?i=877\">Kempton</a>, <a href=\"meeting.asp?i=878\">Lingfield</a>, <a href=\"meeting.asp?i=879\">Musselburgh</a>, <br><br>25 January 2011 - <a href=\"meeting.asp?i=873\">Leicester</a>, <a href=\"meeting.asp?i=874\">Sedgefield</a>, <a href=\"meeting.asp?i=875\">Southwell</a>, <br><br>24 January 2011 - <a href=\"meeting.asp?i=870\">Kempton</a>, <a href=\"meeting.asp?i=871\">Wolverhampton</a>, <a href=\"meeting.asp?i=872\">Wetherby</a>, <br><br>23 January 2011 - <a href=\"meeting.asp?i=867\">Kempton</a>, <a href=\"meeting.asp?i=868\">Market Rasen</a>, <a href=\"meeting.asp?i=869\">Towcester</a>, <br><br>22 January 2011 - <a href=\"meeting.asp?i=863\">Ascot</a>, <a href=\"meeting.asp?i=864\">Haydock</a>, <a href=\"meeting.asp?i=865\">Lingfield</a>, <a href=\"meeting.asp?i=866\">Wincanton</a>, <br><br>21 January 2011 - <a href=\"meeting.asp?i=858\">Chepstow</a>, <a href=\"meeting.asp?i=859\">Catterick</a>, <a href=\"meeting.asp?i=860\">Kelso</a>, <a href=\"meeting.asp?i=861\">Lingfield</a>, <a href=\"meeting.asp?i=862\">Wolverhampton</a>, <br><br>20 January 2011 - <a href=\"meeting.asp?i=854\">Lingfield</a>, <a href=\"meeting.asp?i=855\">Ludlow</a>, <a href=\"meeting.asp?i=856\">Taunton</a>, <a href=\"meeting.asp?i=857\">Wolverhampton</a>, <br><br>19 January 2011 - <a href=\"meeting.asp?i=850\">Kempton</a>, <a href=\"meeting.asp?i=851\">Lingfield</a>, <a href=\"meeting.asp?i=852\">Newbury</a>, <a href=\"meeting.asp?i=853\">Newcastle</a>, <br><br>18 January 2011 - <a href=\"meeting.asp?i=847\">Folkestone</a>, <a href=\"meeting.asp?i=848\">Southwell</a>, <a href=\"meeting.asp?i=849\">Wolverhampton</a>, <br><br>17 January 2011 - <a href=\"meeting.asp?i=844\">Fakenham</a>, <a href=\"meeting.asp?i=845\">Plumpton</a>, <a href=\"meeting.asp?i=846\">Wolverhampton</a>, <br><br>16 January 2011 - <a href=\"meeting.asp?i=842\">Ffos Las</a>, <a href=\"meeting.asp?i=843\">Southwell</a>, <br><br>15 January 2011 - <a href=\"meeting.asp?i=838\">Kempton</a>, <a href=\"meeting.asp?i=839\">Lingfield</a>, <a href=\"meeting.asp?i=840\">Warwick</a>, <a href=\"meeting.asp?i=841\">Wetherby</a>, <br><br>14 January 2011 - <a href=\"meeting.asp?i=834\">Huntingdon</a>, <a href=\"meeting.asp?i=835\">Lingfield</a>, <a href=\"meeting.asp?i=836\">Musselburgh</a>, <a href=\"meeting.asp?i=837\">Wolverhampton</a>, <br><br>13 January 2011 - <a href=\"meeting.asp?i=830\">Catterick</a>, <a href=\"meeting.asp?i=831\">Hereford</a>, <a href=\"meeting.asp?i=832\">Kempton</a>, <a href=\"meeting.asp?i=833\">Southwell</a>, <!-- content --></td></tr></tbody></table>";
		
		[TestFixtureSetUp]
		public void SetUp() {
			_htmlLoader = MockRepository.GenerateStub<IHtmlLoader>();
			_htmlLoader.Stub(x => x.Load()).Return(HTML);
		}

		[Test]
		public void Can_build_all_html_elements() {

			IEnumerable<HtmlNode> htmlNodes = new FluentHtmlSelector(_htmlLoader)
													.Build();

			Assert.That(htmlNodes, Is.Not.Null);
		}

		[Test]
		public void Can_fluently_select_inner_html() {
			HtmlNode currentNode = new FluentHtmlSelector(_htmlLoader)
											.MoveToNode("body")
											.MoveToNode("div#div2")
											.CurrentNode();

			Assert.That(currentNode.InnerText, Is.EqualTo("2nd div"));
		}

		[Test]
		public void Can_fluently_select_inner_html_table_class() {
			var stringHtmlLoader = new StringHtmlLoader(HTML_WITH_TABLE);

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(HTML_WITH_TABLE);

			var currentNodes = new FluentHtmlSelector(stringHtmlLoader)
											.MoveToNode("td.cnt_txt")
											.MoveToNode("a")
											.Build();

			Assert.That(currentNodes.Count(), Is.GreaterThan(0));
		}

		[Test]
		public void Can_fluently_select_links() {
			var stringHtmlLoader = new StringHtmlLoader(HTML_WITH_LINK);

			IHtmlSelector htmlSelector = new FluentHtmlSelector(stringHtmlLoader)
				.ClickLink("a[id='googleLink']");

			Assert.That(htmlSelector, Is.Not.Null);
			string title = htmlSelector.MoveToNode("title").CurrentNode().InnerText;
			Assert.That(title, Is.EqualTo("Google"));
		}

		[Test]
		public void Can_add_history()
		{
			var stringHtmlLoader = new StringHtmlLoader(HTML_WITH_LINK);

			IHtmlSelector htmlSelector = new FluentHtmlSelector(stringHtmlLoader);
			Assert.That(htmlSelector.CurrentNode().OuterHtml, Is.EqualTo(HTML_WITH_LINK));

			htmlSelector.ClickLink("a[id='googleLink']");
			Assert.That(htmlSelector.CurrentNode().OuterHtml, Is.Not.EqualTo(HTML_WITH_LINK));

			htmlSelector.GoBack();
			Assert.That(htmlSelector.CurrentNode().OuterHtml, Is.EqualTo(HTML_WITH_LINK));

			htmlSelector.ClickLink("a[id='googleLink']");
			Assert.That(htmlSelector.CurrentNode().OuterHtml, Is.Not.EqualTo(HTML_WITH_LINK));

			htmlSelector.GoBack();
			Assert.That(htmlSelector.CurrentNode().OuterHtml, Is.EqualTo(HTML_WITH_LINK));
		}
	}
}
