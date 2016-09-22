using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrainBenchmark;

namespace DgmlVisualizer
{
    class Program
    {
        private static XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/vs/2009/dgml");

        static void Main(string[] args)
        {
            var test = new SwitchSet();
            var query = test.Data.Query;
            var changeSources = Enumerable.Empty<INotifiable>();

            var doc = CreateDgml(query, changeSources);
            OpenDgml(doc);
        }

        private static XDocument CreateDgml(INotifiable query, IEnumerable<INotifiable> changeSources)
        {
            var doc = CreateDgmlDoc();
            var nodes = doc.Root.Element(ns + "Nodes");
            var links = doc.Root.Element(ns + "Links");

            var allNodes = new[] { query }.Concat(NMF.Utilities.Extensions.SelectRecursive(query, n => n.Dependencies)).ToList();
            foreach (var node in allNodes)
            {
                nodes.Add(ToDgmlNode(node));
                foreach (var link in ToDgmlLink(node))
                    links.Add(link);
            }
            
            var sourceNodesIds = changeSources.Select(n => n.GetHashCode().ToString()).ToList();
            var actualSources = nodes.Elements().Where(e => sourceNodesIds.Contains(e.Attribute("Id").Value));
            foreach (var s in actualSources)
                s.SetAttributeValue("Category", "ActualChangeSource");

            var syncNodes = FindSyncNodes(changeSources);
            var syncNodesIds = syncNodes.Select(n => n.GetHashCode().ToString()).ToList();
            var syncs = nodes.Elements().Where(e => syncNodesIds.Contains(e.Attribute("Id").Value));
            foreach (var s in syncs)
                s.SetAttributeValue("Category", "SyncNode");

            foreach (var node in changeSources)
            {
                var path = NMF.Utilities.Extensions.SelectRecursive(node, n => n.Successors).ToList();
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var src = path[i];
                    var dst = path[i + 1];
                    var link = links.Elements().First(e => e.Attribute("Source").Value == src.GetHashCode().ToString() && e.Attribute("Target").Value == dst.GetHashCode().ToString());
                    link.SetAttributeValue("Category", "ActualChangeSource");
                }
            }

            return doc;
        }

        private static void OpenDgml(XDocument doc)
        {
            var fileName = DateTime.Now.ToString("hh-mm-ss") + ".dgml";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            doc.Save(filePath);
            System.Diagnostics.Process.Start(filePath);
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

        private static XElement ToDgmlNode(INotifiable node)
        {
            var e = new XElement(ns + "Node");
            e.SetAttributeValue("Id", node.GetHashCode());
            e.SetAttributeValue("Label", NodeToLabel(node));
            return e;
        }

        private static string NodeToLabel(INotifiable node)
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

        private static IEnumerable<XElement> ToDgmlLink(INotifiable node)
        {
            return node.Dependencies.Select(d =>
            {
                var e = new XElement(ns + "Link");
                e.SetAttributeValue("Source", d.GetHashCode());
                e.SetAttributeValue("Target", node.GetHashCode());
                return e;
            });
        }

        private static List<INotifiable> FindSyncNodes(IEnumerable<INotifiable> sourceNodes)
        {
            var result = new List<INotifiable>();
            var visited = new HashSet<int>();
            var stack = new Stack<INotifiable>(sourceNodes);

            while (stack.Any())
            {
                var item = stack.Pop();
                visited.Add(item.GetHashCode());
                foreach (var suc in item.Successors)
                    stack.Push(suc);

                if (item.Dependencies.Count(d => visited.Contains(d.GetHashCode())) == 2)
                    result.Add(item);
            }

            return result;
        }
    }
}
