using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlReader.Exceptions
{
    public class HtmlReaderException : Exception
    {
        public HtmlReaderException() : base()
        {}
        public HtmlReaderException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public HtmlReaderException(string message, Exception innerException, bool isErrorLogged) : base(message, innerException)
        {
            if(isErrorLogged)
            {
                //log error using Enterprise?
            }
        }
    }
}
