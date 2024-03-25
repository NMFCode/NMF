using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Expressions.Debug
{
    /// <summary>
    /// Denotes a helper class to export DDG nodes as DGML
    /// </summary>
    public class DgmlExporter
    {
        /// <summary>
        /// Generates a DGML document for the DDG rooted at the given element
        /// </summary>
        /// <param name="root">The rrot of the DDG</param>
        /// <returns>The DGML document</returns>
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
                var label = Print(node);
                if (node is INotifyExpression expression)
                {
                    label += $" {{{Print(expression.ValueObject)}}}";
                }
                nodeBuilder.Append($"    <Node Id=\"{nodeName}\" Label=\"{label}\"");
                var propDict = new Dictionary<string, object>();
                var dependencyLabels = new Dictionary<INotifiable, string>();
                CalculateProperties(node, propDict, dependencyLabels);
                AppendProperties(nodeBuilder, propDict);
                nodeBuilder.AppendLine(" />");
                AppendDependencies(node, dict, nodeBuilder, edgeBuilder, nodeName, dependencyLabels);
            }
            return nodeName;
        }

        private static void AppendProperties(StringBuilder nodeBuilder, Dictionary<string, object> propDict)
        {
            foreach (var prop in propDict)
            {
                nodeBuilder.Append($" {prop.Key}=\"{Print(prop.Value)}\"");
            }
        }

        private static void AppendDependencies(INotifiable node, Dictionary<INotifiable, string> dict, StringBuilder nodeBuilder, StringBuilder edgeBuilder, string nodeName, Dictionary<INotifiable, string> dependencyLabels)
        {
            foreach (var dependency in node.Dependencies)
            {
                string depLabel;
                if (!dependencyLabels.TryGetValue(dependency, out depLabel)) depLabel = "(unknown)";
                edgeBuilder.AppendLine($"    <Link Source=\"{nodeName}\" Target=\"{AddRecursive(dependency, dict, nodeBuilder, edgeBuilder)}\" Label=\"{depLabel}\" />");
            }
        }

        private static void CalculateProperties(INotifiable node, Dictionary<string, object> propDict, Dictionary<INotifiable, string> dependencyLabels)
        {
            foreach (var prop in node.GetType().GetProperties())
            {
                try
                {
                    if (!propDict.ContainsKey(prop.Name) && HasNoParameters(prop))
                    {
                        var value = prop.GetValue(node);
                        propDict.Add(prop.Name, value);
                        if (value is INotifiable notifiable)
                        {
                            AppendDependencyLabel(dependencyLabels, prop, notifiable);
                        }
                        else if (value is IEnumerable<INotifiable> nodeCollection)
                        {
#pragma warning disable S2259 // Null pointers should not be dereferenced
                            foreach (var item in nodeCollection)
#pragma warning restore S2259 // Null pointers should not be dereferenced
                            {
                                AppendDependencyLabel(dependencyLabels, prop, item);
                            }
                        }
                    }
                }
                catch (TargetInvocationException) { }
            }
        }

        private static void AppendDependencyLabel( Dictionary<INotifiable, string> dependencyLabels, PropertyInfo prop, INotifiable notifiable )
        {
            if(dependencyLabels.TryGetValue( notifiable, out var depLabel ))
            {
                dependencyLabels[notifiable] = depLabel + ", " + prop.Name;
            }
            else
            {
                dependencyLabels.Add( notifiable, prop.Name );
            }
        }

        private static bool HasNoParameters( PropertyInfo prop )
        {
            var indexParameters = prop.GetIndexParameters();
            return indexParameters == null || indexParameters.Length == 0;
        }

        private static string Print(object obj)
        {
            if (obj == null) return "(null)";
            if(obj is IEnumerable enumerable && !(obj is string) && !(obj is INotifiable))
            {
                return "[" + string.Join( ",", enumerable.Cast<object>().Select( o => Print( o ) ) ) + "]";
            }
            else
            {
                var serial = obj.ToString();
                var index = serial.IndexOfAny( new[] { '<', '>', '"', '´', '`' } );
                if(serial.Length > 100) index = Math.Min( 100, index == -1 ? 100 : index );
                if(index != -1)
                {
                    return serial.Substring( 0, index ) + "...";
                }
                else
                {
                    return serial;
                }
            }
        }
    }
}
