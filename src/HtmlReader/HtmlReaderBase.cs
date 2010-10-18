using System.Collections.Generic;

namespace HtmlReader
{
    public abstract class HtmlReaderBase
    {
        public List<IHtmlReaderResult> Results { get; set; }
        string PageToRead { get; set; }

        public abstract void ReadResults();

        protected HtmlReaderBase()
        {
            Results = new List<IHtmlReaderResult>();
        }
    }
}
