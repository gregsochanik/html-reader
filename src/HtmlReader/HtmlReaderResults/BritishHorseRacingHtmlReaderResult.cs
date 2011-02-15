using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlReader.Exceptions;
using HtmlReader.Fizzler;

namespace HtmlReader.HtmlReaderResults
{
    /// <summary>
    /// TODO: Create a way of adding Attribute containing the Html Selector string for each result item!!!!
    /// eg 
    /// [ItemHtmlSelector("td#CourseName")] or
    /// [ItemHtmlSelector("td>p.courseName")] etc
    /// public string CourseName { get; set; }
    /// </summary>
    public class BritishHorseRacingHtmlReaderResult : IHtmlReaderResult
    {
        [ItemHtmlSelector(HtmlSelectorType.RegularExpression, @"\d{2}\s[A-z]{3,10}\s\d{4},\s\d{2}:\d{2}")]
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
    
        /// <summary>
        /// TODO: REFACTOR THIS!
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public List<BritishHorseRacingHtmlReaderResult> FillResults(string html) {
        	return null;

        	//var htmlString = new StringBuilder(html);
        	//var selector = new HtmlSelector(htmlString);

        	//var results = new List<BritishHorseRacingHtmlReaderResult>();

        	//// Event Date
        	//var datePattern = new Regex(@"\d{2}\s[A-z]{3,10}\s\d{4},\s\d{2}:\d{2}");
        	//string date = datePattern.Match(selector.Html).Value;
        	//DateTime eventStartTime = Convert.ToDateTime(date);

        	//// COurseName and Event Name
        	//IList<HtmlNode> courseandeventnameNodes = selector.SelectList("b");
        	//HtmlNode node = courseandeventnameNodes[1];
        	//string[] courseSplit = node.InnerHtml.Split(new[] { "<br>\r\n" }, StringSplitOptions.None);
        	//string courseName = courseSplit[0];
        	//string eventName = courseSplit[1];

        	//// Weather
        	//string[] linelist = selector.Html.Split(new[] { "<br>\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        	//string weather = linelist.Where(l => l.Contains("Weather:"))
        	//                         .Select(l => l.Replace("Weather:", ""))
        	//                         .DefaultIfEmpty("None")
        	//                         .First();

        	//// Going
        	//string going = linelist.Where(l => l.Contains("Going:"))
        	//                        .Select(l => l.Replace("Going:", ""))
        	//                        .DefaultIfEmpty("None")
        	//                        .First();


        	//// Results table
        	//IList<HtmlNode> selectList = selector.SelectList("table.cnt_txt tr");
        	//selectList.RemoveAt(0);
        	//int counter = 0;
        	//foreach (var htmlNode in selectList)
        	//{

        	//    var result = new BritishHorseRacingHtmlReaderResult
        	//    {
        	//        EventStartTime = eventStartTime,
        	//        EventName = eventName,
        	//        CourseName = courseName,
        	//        Weather = weather,
        	//        Going = going
        	//    };// Ctor should contain code for autofill?

        	//    var innerSelector = new HtmlSelector(new StringBuilder(htmlNode.InnerHtml));
        	//    IList<HtmlNode> cells = innerSelector.SelectList("td");

        	//    if (cells.Count != 4)
        	//        continue;

        	//    if (string.IsNullOrEmpty(cells[0].InnerHtml) || cells[0].InnerHtml == "&nbsp;")
        	//        continue;
        	//    try
        	//    {
        	//        //Position
        	//        int position = int.TryParse(GetResultColA(cells[0]), out position) ? position : -1;
        	//        result.Position = position;
        	//        //HorseName
        	//        result.HorseName = GetResultColA(cells[1]);

        	//        //Trainer
        	//        result.TrainerName = GetResultColB(cells[1]);

        	//        //Jockey
        	//        result.JockeyName = GetResultColA(cells[2]);

        	//        //Weight
        	//        result.Weight = GetResultColB(cells[2]);

        	//        //StartingPrice
        	//        string spRaw = GetResultColA(cells[3]);
        	//        result.ParseFractionsFromString(spRaw, new[] { " - " });

        	//        //BeatenDistance
        	//        result.BeatenDistance = GetResultColB(cells[3]);
        	//    }
        	//    catch (Exception ex)
        	//    {
        	//        throw new HtmlReaderException(string.Format("Could not parse result data at row number {0}", counter), ex, true);
        	//    }
        	//    results.Add(result);
        	//    counter++;
        	//}
        	//return results;
        }

        private static string GetResultColA(HtmlNode resultNode)
        {
            HtmlNode node = resultNode.SelectSingleNode("b");
            return node.InnerText;
        }

        private static string GetResultColB(HtmlNode resultNode)
        {
            string html = resultNode.InnerHtml;
            string[] sections = html.Split(new[] { "<br>" }, StringSplitOptions.None);

            return sections[1].Replace("\r\n", "");
        }

        private void ParseFractionsFromString(string fraction, string[] delimeter)
        {
            if (string.IsNullOrEmpty(fraction))
                return;

            string[] fractionsplit = fraction.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
            StartingPriceFractionA = int.Parse(fractionsplit[0]);
            StartingPriceFractionB = int.Parse(fractionsplit[1]);
            StartingPriceDecimal = StartingPriceFractionA / (decimal)StartingPriceFractionB;
        }
    }


}
