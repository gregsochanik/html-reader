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
using HtmlReader.HtmlReaders;


namespace HtmlReaderTest.HtmlReaders
{
    [TestFixture]
    public class BritishHorseRacingWebReaderTest
    {
        [Test]
        public void SuccessReadResults()
        {
            BritishHorseRacingWebReader reader = new BritishHorseRacingWebReader();
            
            reader.ReadResults();
            Assert.IsFalse(reader.Results.Count < 1);
        }


        [Test]
        public void SuccessFillResults()
        {
            const string filePath = "testResults.html";
            var selector = new HtmlSelector(filePath);
            Assert.IsNotNull(selector);
            var result = new BritishHorseRacingHtmlReaderResult();
            List<BritishHorseRacingHtmlReaderResult> results = result.FillResults(selector.Html);
            
            Assert.IsNotEmpty(results);
        }

        [Test]
        public void SuccessGetResultsForDay()
        {
            DateTime date = new DateTime(2010, 03, 17);
            BritishHorseRacingWebReader reader = new BritishHorseRacingWebReader();
            //List<BritishHorseRacingHtmlReaderResult> results = reader.GetResultsForDay(date);
        } 
    }
}
