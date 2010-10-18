﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlReader.Exceptions;
using HtmlReader.Fizzler;
using HtmlReader.HtmlReaderResults;

namespace HtmlReader.HtmlReaders
{
    public class BritishHorseRacingWebReader : HtmlReaderBase
    {
        private const string ResultsPageFolder = "http://www.britishhorseracing.com/goracing/racing/results/";
        private const string HubPageOuterHtmlSelector = "td.cnt_txt"; // jquery style selector at the mo
        private const string ResultPageLink = "meeting.asp?file={0}.xml"; // the link to the meeting results
               
        private readonly Uri _hubPageUrl = new Uri(string.Format("{0}archive.asp", ResultsPageFolder));

        private DateTime _currentEventDate;
        private string _currentCourse;

        /// <summary>
        /// TODO - Racing for a specific day!! - Also need to find a way of searching for results prior to those on the archive page!!
        /// Sh*t - need to get these into a database asap as they delete the older results!
        /// </summary>
        public override void ReadResults()
        {
            HtmlSelector selector = GetHtmlSelector();

            // read through each date and immediate subsequent course names (until next date? or until next <br />- this bit could be tricky!!!)
            IList<HtmlNode> aList = selector.SelectList("a");
            if (aList.Count < 1)
                return;

            // for each one, formulate a string like so - "c[yyyymmdd][coursecode]"
            foreach (var htmlNode in aList)
            {
                // read back the date for the result
                string hrefValue = htmlNode.Attributes["href"].Value;
                // read back the textnode as the course
                string courseName = htmlNode.InnerText;

                Regex patternToMatch = new Regex(@"c\d{8}[a-z]{3}"); // matches c20100108ban
                string resultKey = patternToMatch.Match(hrefValue).Value;

                // browse to that results page
                ParseResultsHubPage(resultKey);
            }
        }

        private HtmlSelector GetHtmlSelector()
        {
            // browse to hub page url
            HtmlSelector selector = new HtmlSelector(_hubPageUrl);
            IList<HtmlNode> selectList = selector.SelectList(HubPageOuterHtmlSelector);
            selector.ResetDocument(selectList[0].InnerHtml);
            return selector;
        }

        private void ParseResultsHubPage(string resultKey)
        {
            string link = string.Format(ResultPageLink, resultKey);
            Uri resultsPage = new Uri(string.Format("{0}{1}", ResultsPageFolder, link));

            _currentEventDate = GetDateFromResultKey(resultKey);

            // browse to results page
            HtmlSelector resultSelector = new HtmlSelector(resultsPage);
            IList<HtmlNode> selectList = resultSelector.SelectList(HubPageOuterHtmlSelector);
            if(selectList.Count <1)
                return;
            resultSelector.ResetDocument(selectList[0].InnerHtml);
            _currentCourse = resultSelector.SelectList("b")[0].InnerHtml;

            // get list of result anchors
            IList<HtmlNode> aList = resultSelector.SelectList("a");
            // read through results using the attribs from the RacingReaderResult ??
            foreach (var htmlNode in aList)
            {
                string href = htmlNode.Attributes["href"].Value;
                if(href.Contains("results.asp?"))
                    ParseResults(href);
            }
        }

        private void ParseResults(string href)
        {
            Uri resultsPage = new Uri(string.Format("{0}{1}", ResultsPageFolder, href));
            // browse to page
            HtmlSelector resultSelector = new HtmlSelector(resultsPage);
            // select correct table
            IList<HtmlNode> selectList = resultSelector.SelectList("table.cnt_txt>tr>td");
            if(selectList.Count < 2)
                throw new HtmlReaderException(string.Format("Could not grab the race results for {0} on {1}", _currentCourse, _currentEventDate),
                    null, true);

            HtmlNode resultsTable = selectList[2]; // This is a bit flakey! Need some more testing to make sure this is the correct one....

            // Add them to the Results list
            BritishHorseRacingHtmlReaderResult resultApi = new BritishHorseRacingHtmlReaderResult();
            List<BritishHorseRacingHtmlReaderResult> results = resultApi.FillResults(resultsTable.InnerHtml);
            Results.AddRange(results.ToArray());
        }

        //[ExceptionDialog]
        private static DateTime GetDateFromResultKey(string resultKey)
        {
            Regex patternToMatch = new Regex(@"\d{8}");
            string rawDate = patternToMatch.Match(resultKey).Value;
            if(rawDate.Length != 8)
                throw new HtmlReaderException("resultKey did not contain correct date pattern", null, true);

           
            int year = Convert.ToInt32(rawDate.Substring(0, 4));
            int month = Convert.ToInt32(rawDate.Substring(4, 2));
            int day = Convert.ToInt32(rawDate.Substring(6, 2));
            DateTime eventDate = new DateTime(year, month, day);
            return eventDate;
            
        }

        /// <summary>
        /// TODO - this needs to be prvate and fired by another constructor for this class that takes a date (or a renge of dates?)
        /// </summary>
        /// <param name="date"></param>
        public void GetResultsForDay(DateTime date)
        {
            HtmlSelector selector = GetHtmlSelector();
            string dateToSearch = date.ToString("yyyyMMdd");

            IList<HtmlNode> aList = selector.SelectList("a").ToList();

            IList<HtmlNode> dateList = aList.Where(x => x.Attributes["href"].Value.Contains(dateToSearch)).ToList();

            foreach (var htmlNode in dateList)
            {
                // read back the date for the result
                string hrefValue = htmlNode.Attributes["href"].Value;
                // read back the textnode as the course
                string courseName = htmlNode.InnerText;

                Regex patternToMatch = new Regex(@"c\d{8}[a-z]{3}"); // matches c20100108ban
                string resultKey = patternToMatch.Match(hrefValue).Value;

                // browse to that results page
                ParseResultsHubPage(resultKey);
            }
        }
    }
}