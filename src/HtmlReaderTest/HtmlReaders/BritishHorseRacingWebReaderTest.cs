using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using HtmlReader.Exceptions;
using HtmlReader.Fizzler;
using NUnit.Framework;
using Rhino.Mocks;

namespace HtmlReaderTest.HtmlReaders
{
    [TestFixture]
	[Category("Integration")]
    public class BritishHorseRacingWebReaderTest
    {
		private const string RESULTS_PAGE_FOLDER = "http://www.britishhorseracing.com/goracing/racing/results/archive.asp";
		private const string BASE_URL = "http://www.britishhorseracing.com/goracing/racing/results/";
		
		[Test]
        public void Reader_should_return_some_results() {
    		var htmlLoader = new HtmlLoader(new Uri(RESULTS_PAGE_FOLDER));

    		var fluentHtmlSelector = new FluentHtmlSelector(htmlLoader){BaseUrl = BASE_URL};

    		var britishHorseracingWebReader = new BritishHorseracingWebReader(fluentHtmlSelector);

    		var results = britishHorseracingWebReader.GetResults().ToList();

    	}
    }

	public class BritishHorseracingWebReader
	{
		private readonly IHtmlSelector _selector;

		private string _currentCourse;

		public BritishHorseracingWebReader(IHtmlSelector selector) {
			_selector = selector;
		}

		public IEnumerable<Result> GetResults() {
			IEnumerable<HtmlNode> htmlNodes = _selector.MoveToNode("td.cnt_txt").SelectNodes("a");
			
			foreach (var htmlNode in htmlNodes) {
				string meetingHref = htmlNode.Attributes["href"].Value;
				IEnumerable<HtmlNode> nodes = _selector.ClickLink("a[href='" + meetingHref + "']")
													.MoveToNode("td.cnt_txt")
													.SelectNodes("a");

				var resultLinks = nodes.Select(node => node.Attributes["href"].Value).ToList();

				foreach (var resultHref in resultLinks){
					// link through to that page
					_selector.ClickLink("a[href='" + resultHref + "']")
						.MoveToNode("td.cnt_txt")
						.MoveToNode("table.cnt_txt");

					IEnumerable<HtmlNode> results = _selector
						.SelectNodes("tr").Skip(2);

					_currentCourse = _selector.SelectNodes("b").ElementAt(1).InnerHtml;
					// from results build ResultList

					foreach (var result in results)
					{
						yield return ParseResult(result);
					}
				}
				_selector.GoBack();
			}
			yield break;
		}

		private Result ParseResult(HtmlNode resultSet)
		{
			string[] courseSplit = _currentCourse.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
			string courseName = courseSplit[0];
			string eventName = courseSplit[1];

			var doc = new HtmlDocument();
			doc.LoadHtml(resultSet.OuterHtml);

			IEnumerable<HtmlNode> allResults = doc.DocumentNode.QuerySelectorAll("td");
			string position = allResults.ElementAt(0).QuerySelector("b").InnerText;
			string name = allResults.ElementAt(1).QuerySelector("b").InnerText;
			return new Result
			       	{
						CourseName = courseName,
						EventName = eventName,
						Position = Convert.ToInt32(position),
						HorseName = name
					};
		}
	}

	public class Result
	{
		public DateTime EventStartTime { get; set; }
        public string CourseName { get; set; }
        public string EventName { get; set; }
        public string Going { get; set; }
        public string Weather { get; set; }
        public int Position { get; set; }
        public string HorseName { get; set; }
        public string TrainerName { get; set; }
        public string JockeyName { get; set; }
        public string Weight { get; set; }
        public int StartingPriceNumerator { get; set; } 
        public int StartingPriceDenominator { get; set; } // eg - at the moment its A/B or A-B 3/1
        public decimal StartingPriceDecimal { get; set; } // eg 2-1 = 3.0, 1-3 on = 1.33 etc
        public string BeatenDistance { get; set; }
	}
	
}
