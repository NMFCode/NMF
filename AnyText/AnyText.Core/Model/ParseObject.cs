using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Model
{
    internal class ParseObject
    {
        private readonly object _semanticElement;
        private readonly Dictionary<string, List<object>> _modelTokens = new Dictionary<string, List<object>>();

        public ParseObject(object semanticElement)
        {
            _semanticElement = semanticElement;
        }

        public bool TryPeekModelToken<T, TMember>(string feature, Func<T, ParseContext, TMember> featureImplementation, ParseContext context, out TMember modelToken)
        {
            if (!_modelTokens.TryGetValue(feature, out var tokensForFeature))
            {
                if (_semanticElement is T typedElement)
                {
                    modelToken = featureImplementation(typedElement, context);
                    _modelTokens.Add(feature, new List<object> { modelToken });
                    return true;
                }
            }
            else if (tokensForFeature != null && tokensForFeature.Count > 0 && tokensForFeature[tokensForFeature.Count - 1] is TMember result)
            {
                modelToken = result;
                return true;
            }
            modelToken = default;
            return false;
        }

        public bool TryPeekModelToken<T, TMember>(string feature, Func<T, ParseContext, ICollection<TMember>> featureImplementation, ParseContext context, out TMember modelToken)
        {
            if (!_modelTokens.TryGetValue(feature, out var tokensForFeature))
            {
                if (_semanticElement is T typedElement)
                {
                    var items = featureImplementation(typedElement, context);
                    var list = new List<object>();
                    foreach (var item in items)
                    {
                        list.Add(item);
                    }
                    list.Reverse();
                    _modelTokens.Add(feature, list);
                    if (list.Count > 0 && list[list.Count - 1] is TMember result)
                    {
                        modelToken = result;
                        return true;
                    }
                    modelToken = default;
                    return false;
                }
            }
            else if (tokensForFeature != null && tokensForFeature.Count > 0 && tokensForFeature[tokensForFeature.Count - 1] is TMember result)
            {
                modelToken = result;
                return true;
            }
            modelToken = default;
            return false;
        }

        public bool TryConsumeModelToken<T, TMember>(string feature, Func<T, ParseContext, TMember> featureImplementation, ParseContext context, out TMember modelToken)
        {
            if (!_modelTokens.TryGetValue(feature, out var tokensForFeature))
            {
                if (_semanticElement is T typedElement)
                {
                    modelToken = featureImplementation(typedElement, context);
                    _modelTokens.Add(feature, null);
                    return true;
                }
            }
            else if (tokensForFeature != null && tokensForFeature.Count > 0 && tokensForFeature[tokensForFeature.Count - 1] is TMember result)
            {
                modelToken = result;
                tokensForFeature.RemoveAt(tokensForFeature.Count - 1);
                return true;
            }
            modelToken = default;
            return false;
        }

        public bool TryConsumeModelToken<T, TMember>(string feature, Func<T, ParseContext, ICollection<TMember>> featureImplementation, ParseContext context, out TMember modelToken)
        {
            if (!_modelTokens.TryGetValue(feature, out var tokensForFeature))
            {
                if (_semanticElement is T typedElement)
                {
                    var items = featureImplementation(typedElement, context);
                    var list = new List<object>();
                    foreach ( var item in items )
                    {
                        list.Add(item);
                    }
                    list.Reverse();
                    _modelTokens.Add(feature, list);
                    if (list.Count > 0 && list[list.Count - 1] is TMember result)
                    {
                        modelToken= result;
                        list.RemoveAt(list.Count - 1);
                        return true;
                    }
                    modelToken = default;
                    return false;
                }
            }
            else if (tokensForFeature != null && tokensForFeature.Count > 0 && tokensForFeature[tokensForFeature.Count - 1] is TMember result)
            {
                modelToken = result;
                tokensForFeature.RemoveAt(tokensForFeature.Count - 1);
                return true;
            }
            modelToken = default;
            return false;
        }
    }
}
