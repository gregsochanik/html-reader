using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlReader.HtmlReaders;

namespace HtmlReaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BritishHorseRacingWebReader reader = new BritishHorseRacingWebReader();

            reader.ReadResults();

        }
    }
}
