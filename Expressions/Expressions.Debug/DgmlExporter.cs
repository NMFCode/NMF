using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NMF.Expressions.Debug
{
    public class DgmlExporter
    {
        public static string Export(INotifiable root)
        {
            var dict = new Dictionary<INotifiable, string>();
            var builder = new StringBuilder();
            var edgeBuilder = new StringBuilder();
            builder.AppendLine("<DirectedGraph xmlns=\"http://schemas.microsoft.com/vs/2009/dgml\">");
            builder.AppendLine("  <Nodes>");
            AddRecursive(root, dict, builder, edgeBuilder);
            builder.AppendLine("  </Nodes>");
            builder.AppendLine("  <Links>");
            builder.Append(edgeBuilder.ToString());
            builder.AppendLine("  </Links>");
            builder.AppendLine("</DirectedGraph>");
            return builder.ToString();
        }

        private static string AddRecursive(INotifiable node, Dictionary<INotifiable, string> dict, StringBuilder nodeBuilder, StringBuilder edgeBuilder)
        {
            string nodeName;
            if (!dict.TryGetValue(node, out nodeName))
            {
                nodeName = $"node{dict.Count + 1}";
                dict.Add(node, nodeName);
                var label = node.ToString();
                if (node is INotifyExpression expression)
                {
                    label += $" {{{Print(expression.ValueObject)}}}";
                }
                nodeBuilder.Append($"    <Node Id=\"{nodeName}\" Label=\"{label}\"");
                var propDict = new Dictionary<string, object>();
                var dependencyLabels = new Dictionary<INotifiable, string>();
                foreach (var prop in node.GetType().GetProperties())
                {
                    try
                    {
                        if (!propDict.ContainsKey(prop.Name))
                        {
                            var value = prop.GetValue(node);
                            propDict.Add(prop.Name, value);
                            if (value is INotifiable notifiable)
                            {
                                if (dependencyLabels.TryGetValue(notifiable, out var depLabel))
                                {
                                    dependencyLabels[notifiable] = depLabel + ", " + prop.Name;
                                }
                                else
                                {
                                    dependencyLabels.Add(notifiable, prop.Name);
                                }
                            }
                        }
                    }
                    catch (TargetInvocationException) { }
                }
                foreach (var prop in propDict)
                {
                    nodeBuilder.Append($" {prop.Key}=\"{Print(prop.Value)}\"");
                }
                nodeBuilder.AppendLine(" />");
                foreach (var dependency in node.Dependencies)
                {
                    string depLabel;
                    if (!dependencyLabels.TryGetValue(dependency, out depLabel)) depLabel = "(unknown)"; 
                    edgeBuilder.AppendLine($"    <Link Source=\"{nodeName}\" Target=\"{AddRecursive(dependency, dict, nodeBuilder, edgeBuilder)}\" Label=\"{depLabel}\" />");
                }
            }
            return nodeName;
        }

        private static string Print(object obj)
        {
            if (obj == null) return "(null)";
            var serial = obj.ToString();
            var index = serial.IndexOfAny(new[] { '<', '>', '"', '´', '`' });
            if (serial.Length > 100) index = Math.Min(100, index == -1 ? 100 : index);
            if (index != -1)
            {
                return serial.Substring(0, index) + "...";
            }
            else
            {
                return serial;
            }
        }
    }
}
