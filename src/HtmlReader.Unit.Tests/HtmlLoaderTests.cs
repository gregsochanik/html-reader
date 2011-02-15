using System;
using HtmlReader.Fizzler;
using NUnit.Framework;

namespace HtmlReader.Unit.Tests
{
	[TestFixture]
	public class HtmlLoaderTests
	{
		[Test]
		public void Can_load_html() {
			var htmlLoader = new HtmlLoader(new Uri("http://www.google.co.uk"));
			string load = htmlLoader.Load();
			Assert.That(load, Is.Not.Null);
			Assert.That(load.Length > 0);
		}
	}
}