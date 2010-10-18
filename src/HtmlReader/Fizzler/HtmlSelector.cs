using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Net;

namespace HtmlReader.Fizzler
{
    public class HtmlSelector
    {
        public string Html { get; set; }

        public HtmlSelector()
        {
            InitializeDocument(string.Empty);
        }

        public HtmlSelector(StringBuilder html)
        {
            InitializeDocument(html.ToString());
        }

        [FileIOPermission(SecurityAction.LinkDemand, AllLocalFiles=FileIOPermissionAccess.Read)]
        public HtmlSelector(string filePath)
        {
            string html = File.ReadAllText(filePath);
            
            InitializeDocument(html);
        }

        [WebPermission(SecurityAction.LinkDemand)]
        public HtmlSelector(Uri webpage)
        {
            var webclient = new WebClient();
            string html = webclient.DownloadString(webpage);
            InitializeDocument(html);
        }


        private void InitializeDocument(string html)
        {
            Html = html;
            var document = new HtmlDocument();
            document.LoadHtml(html);
            Document = document;
        }
               
        private HtmlDocument Document { get; set; }

        /// <summary>
        /// Selects all nodes from the current instance document based on the provided Selector as an IEnumerable 
        /// </summary>
        /// <param name="selectorChain"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Select(string selectorChain)
        {            
            return Document.DocumentNode.QuerySelectorAll(selectorChain);
        }

        /// <summary>
        /// Selects all nodes from the current instance document based on the provided Selector as a generic IList 
        /// </summary>
        /// <param name="selectorChain"></param>
        /// <returns></returns>
        public IList<HtmlNode> SelectList(string selectorChain)
        {
            return new List<HtmlNode>(Select(selectorChain).ToArray());
        }

        /// <summary>
        /// Resets the current instance html Document using the supplied html string
        /// </summary>
        /// <param name="html"></param>
        public void ResetDocument(string html)
        {
            Html = html;
            Document.LoadHtml(html);
        }

        /// <summary>
        /// Selects the X path nodes.
        /// </summary>
        /// <param name="xpathQuery">The xpath query.</param>
        /// <returns></returns>
        public IList<HtmlNode> SelectXPathNodes(string xpathQuery)
        {
            HtmlNodeCollection coll = Document.DocumentNode.SelectNodes(xpathQuery);
            IEnumerable<HtmlNode> enumerable =  coll.Cast<HtmlNode>().Select(node => node);
            return new List<HtmlNode>(enumerable.ToArray());
        }

        /// <summary>
        /// Selects the X path.
        /// </summary>
        /// <param name="xpathQuery">The xpath query.</param>
        /// <returns></returns>
        public HtmlNode SelectXPath(string xpathQuery)
        {
            HtmlNode node = Document.DocumentNode.SelectSingleNode(xpathQuery);
            return node;
        }
    }
}
