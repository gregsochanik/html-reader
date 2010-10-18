using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HtmlAgilityPack;
using NUnit.Framework;
using HtmlReader.Fizzler;
using System.Net;

namespace HtmlReaderTest.Fizzler
{
    [TestFixture]
    public class HtmlSelectorTest
    {
        [Test]
        public void SuccessCtor1()
        {
            HtmlSelector selector = new HtmlSelector();
            Assert.IsNotNull(selector);
            Assert.AreEqual(string.Empty, selector.Html);
        }

        [Test]
        public void SuccessCtor2()
        {
            StringBuilder html = new StringBuilder("<html><head></head><body><div><p class='content'>Fizzler</p><p>CSS Selector Engine</p></div></body></html>");

            HtmlSelector selector = new HtmlSelector(html);
            Assert.IsNotNull(selector);
            Assert.AreEqual(html.ToString(), selector.Html);
        }
        
        [Test]
        public void SuccessCtor3()
        {
            const string filePath = "test.html";
            HtmlSelector selector = new HtmlSelector(filePath);
            Assert.IsNotNull(selector);

            string expected = File.ReadAllText(filePath);
            Assert.AreEqual(expected, selector.Html);
        }

        [Test]
        public void SuccessCtor4()
        {
            Uri webpage = new Uri("http://www.britishhorseracing.co.uk/goracing/racing/results/default.asp");
            HtmlSelector selector = new HtmlSelector(webpage);
            Assert.IsNotNull(selector);

            WebClient webclient = new WebClient();
            string expected  = webclient.DownloadString(webpage);
            Assert.AreEqual(expected, selector.Html);

        }

        [Test]
        public void SuccessSelect1()
        {
            const string filePath = "test.html";
            var selector = new HtmlSelector(filePath);
            Assert.IsNotNull(selector);
            IEnumerable<HtmlNode> htmlNodes = selector.Select("td.cnt_txt");
            Assert.IsNotNull(htmlNodes);

        }

        [Test]
        public void SuccessSelect2()
        {
            Uri webpage = new Uri("http://www.britishhorseracing.co.uk/goracing/racing/results/default.asp");
            var selector = new HtmlSelector(webpage);
            Assert.IsNotNull(selector);
            IEnumerable<HtmlNode> htmlNodes = selector.Select("td.cnt_txt");
            Assert.IsNotNull(htmlNodes);
        }
        
        [Test]
        public void SuccessSelectList()
        {
            const string filePath = "test.html";
            var selector = new HtmlSelector(filePath);
            Assert.IsNotNull(selector);
            var selectList = (List<HtmlNode>)selector.SelectList("td.cnt_txt");
            Assert.IsNotNull(selectList);
            Assert.IsFalse(selectList.Count < 1);
        }

        [Test]
        public void SuccessSelectXPathNodes()
        {
            const string filePath = "test.html";
            var selector = new HtmlSelector(filePath);
            Assert.IsNotNull(selector);
            var selectedNodes = (List<HtmlNode>)selector.SelectXPathNodes("//a[contains(@href, 'results.asp?')]");
            Assert.IsTrue(selectedNodes.Count > 0);
        }

        [Test]
        public void SuccessSelectXPath()
        {
            const string filePath = "test.html";
            var selector = new HtmlSelector(filePath);
            Assert.IsNotNull(selector);
            var selectedNode = selector.SelectXPath("//a[text() = 'Lingfield.co.uk Handicap']");
            Assert.IsNotNull(selectedNode);
            const string expected = "Lingfield.co.uk Handicap";
            Assert.AreEqual(expected, selectedNode.InnerText);
        }

    }
}


