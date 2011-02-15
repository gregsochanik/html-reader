using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using NUnit.Framework;
using HtmlAgilityPack;

namespace HtmlReaderTest.Fizzler
{
    [TestFixture]
    public class FizzlerTest
    {
    	private HtmlDocument _document;
    	private const string HTML = "@<html><head></head><body><div><p class='content'>Fizzler</p><p>CSS Selector Engine</p></div><div>2nd</div><div>3rd</div></body></html>";

		[TestFixtureSetUp]
		public void SetUp() {
			_document = new HtmlDocument();
			_document.LoadHtml(HTML);
		}

        [Test]
        public void SuccessQuerySelectAll()
        {
            // Fizzler for HtmlAgilityPack is implemented as the 
            // QuerySelectorAll extension method on HtmlNode
            // yields: [<p class="content">Fizzler</p>]
            var nodes = Select(".content");

            Assert.IsNotNull(nodes);
            Assert.IsNotNull(nodes.First());
			Assert.AreEqual("Fizzler", nodes.First().InnerText);

            // yields: [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
        	string outerHtml = Select("p").FirstOrDefault().OuterHtml;
			Assert.That(outerHtml, Is.EqualTo("<p class=\"content\">Fizzler</p>"));
			Assert.That(Select("p").Count(), Is.EqualTo(2));

        	// yields empty sequence
			Assert.That(Select("body>p").FirstOrDefault(), Is.Null);

            // yields [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
			outerHtml = Select("body p").FirstOrDefault().OuterHtml;
			Assert.That(outerHtml, Is.EqualTo("<p class=\"content\">Fizzler</p>"));
            
            // yields [<p class="content">Fizzler</p>]
			outerHtml = Select("p:first-child").FirstOrDefault().OuterHtml;
			Assert.That(outerHtml, Is.EqualTo("<p class=\"content\">Fizzler</p>"));
			Assert.That(Select("p:first-child").Count(), Is.EqualTo(1));
			
			outerHtml = Select("div:nth-child(2)").FirstOrDefault().OuterHtml;
			Assert.That(outerHtml, Is.EqualTo("<div>2nd</div>"));
			Assert.That(Select("div:nth-child(2)").Count(), Is.EqualTo(1));
        }
		
		[Test]
		public void FailUnsupportedPseudoSelectors() {
			Assert.Throws<FormatException>(() => Select("div:nth-last-of-type(1)"));
			Assert.Throws<FormatException>(() => Select("div:first-letter"));
		}

        protected IEnumerable<HtmlNode> Select(string selectorChain)
        {
            return _document.DocumentNode.QuerySelectorAll(selectorChain);
        }
    }
}


