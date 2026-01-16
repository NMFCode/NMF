using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Type = System.Type;

namespace NMF.Models.Services.Forms
{
    /// <summary>
    /// Denotes a class to write schema information for a model element
    /// </summary>
    public class SchemaWriter
    {
        /// <summary>
        /// Denotes the default instance
        /// </summary>
        public static readonly SchemaWriter Instance = new SchemaWriter();

        /// <summary>
        /// Writes the schema for the given element
        /// </summary>
        /// <param name="element">the model element</param>
        /// <param name="writer">the Utf8JsonWriter to write the schema to</param>
        public void WriteSchema(IModelElement element, Utf8JsonWriter writer)
        {
            if (element == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString("type", "object");
            writer.WritePropertyName("properties");
            writer.WriteStartObject();

            var cl = element.GetClass();

            foreach (var property in AllAttributes(cl))
            {
                if (property.DeclaringType == ModelElement.ClassInstance)
                {
                    continue;
                }
                writer.WritePropertyName(property.Name);
                WriteAttributeProperty(property, writer);    
            }

            foreach (var reference in AllReferences(cl).Where(r => !r.IsContainment && !(r.Opposite != null && r.Opposite.IsContainment)))
            {
                if (reference.DeclaringType == ModelElement.ClassInstance)
                {
                    continue;
                }
                writer.WritePropertyName(reference.Name);
                WriteReferenceProperty(reference, element, writer);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        private IEnumerable<IAttribute> AllAttributes(IClass @class)
        {
            var shadows = new HashSet<IAttribute>();
            foreach (var cl in @class.Closure(c => c.BaseTypes))
            {
                foreach (var constraint in cl.AttributeConstraints.Where(ac => ac.Constrains != null))
                {
                    shadows.Add(constraint.Constrains);
                }
                foreach (var att in cl.Attributes)
                {
                    if (att.Refines != null)
                    {
                        shadows.Add(att.Refines);
                    }
                    if (!shadows.Contains(att))
                    {
                        yield return att;
                    }
                }
            }
        }

        private IEnumerable<IReference> AllReferences(IClass @class)
        {
            var shadows = new HashSet<IReference>();
            foreach (var cl in @class.Closure(c => c.BaseTypes))
            {
                foreach (var constraint in cl.ReferenceConstraints.Where(rc => rc.Constrains != null))
                {
                    shadows.Add(constraint.Constrains);
                }
                foreach (var att in cl.References)
                {
                    if (att.Refines != null)
                    {
                        shadows.Add(att.Refines);
                    }
                    if (!shadows.Contains(att))
                    {
                        yield return att;
                    }
                }
            }
        }

        private void WriteReferenceProperty(IReference reference, IModelElement element, Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            var isCollection = reference.UpperBound != 1;

            if (isCollection)
            {
                WriteCollectionHeader(writer);
            }

            writer.WriteString("type", "string");

            var referenceType = reference.ReferenceType.GetExtension<MappedType>()?.SystemType;
            var possibleItems = GetPossibleItemsFor(element, reference, referenceType)
                .Select(m => (m, m.AbsoluteUri))
                .Where(t => t.AbsoluteUri != null)
                .ToList();

            if (possibleItems.Any())
            {
                writer.WritePropertyName("oneOf");
                writer.WriteStartArray();
                foreach (var item in possibleItems)
                {
                    writer.WriteStartObject();
                    writer.WriteString("const", item.AbsoluteUri.AbsoluteUri);
                    writer.WriteString("title", item.m.ToString());
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteBoolean("readonly", true);
            }


            if (isCollection)
            {
                writer.WriteEndObject();
            }


            writer.WriteEndObject();
        }
        internal IEnumerable<IModelElement> GetPossibleItemsFor(IModelElement element, IReference reference, Type type)
        {
            if (reference.Anchor != null)
            {
                var anchor = reference.Anchor;
                var scope = element;
                while (scope.Parent != null && !(anchor.IsAssignableFrom(scope.GetClass())))
                {
                    scope = scope.Parent;
                }
                return scope.Descendants().Where(type.IsInstanceOfType);
            }
            var model = element.Model;
            if (model == null) return Enumerable.Empty<IModelElement>();
            var repository = model.Repository;
            if (repository == null)
            {
                return model.Descendants().Where(type.IsInstanceOfType);
            }
            else
            {
                IEnumerable<Model> models;
                if (repository is not ModelRepository modelRepo)
                {
                    models = repository.Models.Values.Distinct();
                }
                else
                {
                    models = modelRepo.Models.Values.Distinct().Concat(modelRepo.Parent.Models.Values.Distinct());
                }
                return models.SelectMany(m => m.Descendants()).Where(type.IsInstanceOfType);
            }
        }

        private void WriteAttributeProperty(IAttribute property, Utf8JsonWriter writer)
        {
            var systemType = property.Type.GetExtension<MappedType>()?.SystemType ?? typeof(string);
            var isCollection = property.UpperBound != 1;

            writer.WriteStartObject();

            if (isCollection)
            {
                WriteCollectionHeader(writer);
            }

            if (systemType == typeof(string))
            {
                writer.WriteString("type", "string");
                if (property.DefaultValue != null)
                {
                    writer.WriteString("default", property.DefaultValue.ToString());
                }
            }
            else if (systemType == typeof(int) || (systemType == typeof(long)) || (systemType == typeof(short)))
            {
                writer.WriteString("type", "number");
                if (property.DefaultValue != null)
                {
                    writer.WriteNumber("default", (long)Convert.ChangeType(property.DefaultValue, typeof(long)));
                }
            }
            else if (systemType == typeof(bool))
            {
                writer.WriteString("type", "boolean");
            }
            else if (systemType == typeof(double) || (systemType == typeof(float)))
            {
                writer.WriteString("type", "number");
                if (property.DefaultValue != null)
                {
                    writer.WriteNumber("default", (double)Convert.ChangeType(property.DefaultValue, typeof(double)));
                }
            }
            else if (systemType.IsEnum)
            {
                writer.WriteString("type", "string");
                writer.WritePropertyName("enum");
                writer.WriteStartArray();
                foreach (var entry in Enum.GetNames(systemType))
                {
                    writer.WriteStringValue(entry);
                }
                writer.WriteEndArray();
            }

            if (isCollection)
            {
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        private static void WriteCollectionHeader(Utf8JsonWriter writer)
        {
            writer.WriteString("type", "array");
            writer.WritePropertyName("items");
            writer.WriteStartObject();
        }
    }
}
