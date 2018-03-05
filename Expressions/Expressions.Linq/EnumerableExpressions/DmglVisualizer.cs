using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace NMF.Expressions
{
    public static class DmglVisualizer
    {
        public static XDocument Doc { get; set; }

        private static XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/vs/2009/dgml");

        private static int lastAddedNodeId;

        public static void Initialize()
        {
            Doc = CreateDgmlDoc();
            var nodes = Doc.Root.Element(ns + "Nodes");
            var links = Doc.Root.Element(ns + "Links");

            lastAddedNodeId = 0;
        }

        public static void AddNode(IEnumerableExpression node)
        {
            if(Doc == null) return;
            var nodes = Doc.Root.Element(ns + "Nodes");
            var nodeToAdd = ToDgmlNode(node);
            nodes.Add(nodeToAdd);
        }

        private static XElement ToDgmlLink(IEnumerableExpression node)
        {
            var e = new XElement(ns + "Link");
            e.SetAttributeValue("Source", node.GetHashCode());
            e.SetAttributeValue("Target", lastAddedNodeId);
            return e;
        }

        private static XElement ToDgmlNode(IEnumerableExpression node)
        {
            var links = Doc.Root.Element(ns + "Links");

            var e = new XElement(ns + "Node");

            if (lastAddedNodeId != 0)
                links.Add(ToDgmlLink(node));

            lastAddedNodeId = node.GetHashCode();
            e.SetAttributeValue("Id", lastAddedNodeId);
            e.SetAttributeValue("Label", NodeToLabel(node));

            return e;

        }

        public static void OpenDgml()
        {
            OpenDgml(Doc);
        }


        private static XDocument CreateDgmlDoc()
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            doc.Add(new XElement(ns + "DirectedGraph"));
            doc.Root.SetAttributeValue("Layout", "Sugiyama");
            doc.Root.SetAttributeValue("GraphDirection", "BottomToTop");
            var nodes = new XElement(ns + "Nodes");
            doc.Root.Add(nodes);
            var links = new XElement(ns + "Links");
            doc.Root.Add(links);
            var categories = new XElement(ns + "Categories");
            doc.Root.Add(categories);
            var potentialChangeSource = new XElement(ns + "Category");
            potentialChangeSource.SetAttributeValue("Id", "PotentialChangeSource");
            potentialChangeSource.SetAttributeValue("Background", "LightBlue");
            categories.Add(potentialChangeSource);
            var actualChangeSource = new XElement(ns + "Category");
            actualChangeSource.SetAttributeValue("Id", "ActualChangeSource");
            actualChangeSource.SetAttributeValue("Background", "Green");
            actualChangeSource.SetAttributeValue("Stroke", "Green");
            actualChangeSource.SetAttributeValue("StrokeThickness", "3");
            categories.Add(actualChangeSource);
            var syncNode = new XElement(ns + "Category");
            syncNode.SetAttributeValue("Id", "SyncNode");
            syncNode.SetAttributeValue("Background", "Red");
            categories.Add(syncNode);
            return doc;
        }

        private static void OpenDgml(XDocument doc)
        {
            var fileName = DateTime.Now.ToString("hh-mm-ss-ff") + ".dgml";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            doc.Save(filePath);
            System.Diagnostics.Process.Start(filePath);
        }



        private static string NodeToLabel(IEnumerableExpression node)
        {
            var typeName = node.GetType().Name;
            var result = typeName.Replace("Observable", "");
            int genericIndex = result.IndexOf('`');
            if (genericIndex > 0)
                result = result.Remove(genericIndex);

            var valueProp = node.GetType().GetProperties().FirstOrDefault(p => p.Name == "Value");
            if (valueProp != null)
                result += "\nValue: " + valueProp.GetValue(node);

            var countProp = node.GetType().GetProperty("Count");
            if (countProp != null)
                result += "\nCount: " + countProp.GetValue(node);

            return result;
        }




    }


}