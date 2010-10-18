using System.Collections.Generic;
using NUnit.Framework;
using HtmlAgilityPack;

namespace HtmlReaderTest.Fizzler
{
    [TestFixture]
    public class FizzlerTest : SelectorBaseTest
    {
        [Test]
        public void SuccessQuerySelectAll()
        {
            // Fizzler for HtmlAgilityPack is implemented as the 
            // QuerySelectorAll extension method on HtmlNode
            // yields: [<p class="content">Fizzler</p>]
            IList<HtmlNode> nodes = SelectList(".content");

            Assert.IsNotNull(nodes);
            Assert.IsNotNull(nodes[0]);
            Assert.AreEqual("Fizzler", nodes[0].InnerText);
            //Assert.IsNotEmpty(nodes);

            // yields: [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
            SelectList("p");

            // yields empty sequence
            SelectList("body>p");

            // yields [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
            SelectList("body p");

            // yields [<p class="content">Fizzler</p>]
            SelectList("p:first-child");
        }
    }
}


