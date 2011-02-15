using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlReader.Exceptions;
using HtmlReader.Fizzler;
using HtmlReader.HtmlReaderResults;
using NUnit.Framework;
using Rhino.Mocks;

namespace HtmlReaderTest.HtmlReaders
{
    [TestFixture]
	[Category("Integration")]
    public class BritishHorseRacingWebReaderTest
    {
		private const string RESULTS_PAGE_FOLDER = "http://www.britishhorseracing.com/goracing/racing/results/archive.asp";
		private const string BASE_URL = "http://www.britishhorseracing.com/goracing/racing/results";
		
		[Test]
        public void Reader_should_return_some_results() {
    		var htmlLoader = new HtmlLoader(new Uri(RESULTS_PAGE_FOLDER));

    		var fluentHtmlSelector = new FluentHtmlSelector(htmlLoader){BaseUrl = BASE_URL};

    		var britishHorseracingWebReader = new BritishHorseracingWebReader(fluentHtmlSelector);

    		IEnumerable<Result> enumerable = britishHorseracingWebReader.GetResults();

    	}
    }

	public class BritishHorseracingWebReader
	{
		private readonly IHtmlSelector _selector;

		public BritishHorseracingWebReader(IHtmlSelector selector) {
			_selector = selector;
		}

		public IEnumerable<Result> GetResults() {
			IEnumerable<HtmlNode> htmlNodes = _selector.MoveToNode("td.cnt_txt").SelectNodes("a");
			foreach (var htmlNode in htmlNodes) {
				string item = htmlNode.Attributes["href"].Value;
				IEnumerable<HtmlNode> nodes = _selector.ClickLink("a[href='" + item + "']").MoveToNode("a").Build();
				foreach (var node in nodes) {
					_selector.GoBack();
				}
			}
			return null;
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
        public int StartingPriceFractionA { get; set; } // TODO: Work out correct name for this! You used to know it!!
        public int StartingPriceFractionB { get; set; } // eg - at the moment its A/B or A-B 3/1
        public decimal StartingPriceDecimal { get; set; } // eg 2-1 = 3.0, 1-3 on = 1.33 etc
        public string BeatenDistance { get; set; }
    
	}
	
}
