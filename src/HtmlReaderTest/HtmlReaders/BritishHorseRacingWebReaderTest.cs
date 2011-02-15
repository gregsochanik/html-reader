using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using HtmlReader.Fizzler;
using NUnit.Framework;

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

					_currentCourse = _selector.SelectNodes("b")
										.ElementAt(1)
										.InnerHtml;

					// from results build ResultList))
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
			string jockey = allResults.ElementAt(2).QuerySelector("b").InnerText;
			string startingPrice = allResults.ElementAt(3).QuerySelector("b").InnerText.Trim();

			var price = new Price{DecimalPrice = 0,Denominator=0,Numerator = 0};
			try {
				price = new Price(startingPrice);
			} catch(ArgumentException) {}

			return new Result
			       	{
						CourseName = courseName,
						EventName = eventName,
						Position = position,
						HorseName = name,
						JockeyName = jockey,
						StartingPriceDecimal = price.DecimalPrice,
						StartingPriceDenominator = price.Denominator,
						StartingPriceNumerator = price.Numerator
					};
		}

	}

	public class Price
	{
		public int Numerator { get; set; }
		public int Denominator { get; set; }
		public decimal DecimalPrice { get; set; }

		public Price() {}
		public Price(string formattedPrice) {
			var strings = formattedPrice.Split(new[] {" - "}, StringSplitOptions.None);
			if(strings.Length < 2)
				throw new ArgumentException("odds must be in the \"n / d\" format (e.g. 100/1)");

			Numerator = int.Parse(strings[0]);
			Denominator = int.Parse(strings[1]);

			if(Numerator > 0 && Denominator > 0)
				DecimalPrice = 1 + ((decimal)Numerator/Denominator);
		}

		public override string ToString() {
			return string.Format("{0} - {1}", Numerator, Denominator);
		}
	}

	public class Result
	{
		public DateTime EventStartTime { get; set; }
        public string CourseName { get; set; }
        public string EventName { get; set; }
        public string Going { get; set; }
        public string Weather { get; set; }
        public string Position { get; set; }
        public string HorseName { get; set; }
        public string TrainerName { get; set; }
        public string JockeyName { get; set; }
        public string Weight { get; set; }
        public int StartingPriceNumerator { get; set; } // eg - the 1 in 1/2
        public int StartingPriceDenominator { get; set; } // eg - the 2 in 1/2
        public decimal StartingPriceDecimal { get; set; } // eg 2-1 = 3.0, 1-3 on = 1.33 etc
        public string BeatenDistance { get; set; }
	}
	
}
