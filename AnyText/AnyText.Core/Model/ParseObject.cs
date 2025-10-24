using System;
using System.Collections.Generic;

namespace NMF.AnyText.Model
{
    internal class ParseObject
    {
        private readonly object _semanticElement;
        private readonly Dictionary<string, TokenStore> _modelTokens = new Dictionary<string, TokenStore>();

        public ParseObject(object semanticElement)
        {
            _semanticElement = semanticElement;
        }

        public object SemanticElement => _semanticElement;

        public void Reserve<T, TMember>(string feature, Func<T, ParseContext, TMember> featureImplementation, ParseContext context)
        {
            if (!_modelTokens.TryGetValue(feature, out var tokensForFeature))
            {
                if (_semanticElement is T typedElement)
                {
                    var modelToken = featureImplementation(typedElement, context);
                    tokensForFeature = new TokenStore(modelToken);
                    _modelTokens.Add(feature, tokensForFeature);
                }
                else
                {
                    return;
                }
            }
            tokensForFeature.Reservations++;
        }

        public void Reserve<T, TMember>(string feature, Func<T, ParseContext, ICollection<TMember>> featureImplementation, ParseContext context)
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
                    tokensForFeature = new TokenStore(list);
                    _modelTokens.Add(feature, tokensForFeature);
                }
                else
                {
                    return;
                }
            }
            tokensForFeature.Reservations++;
        }

        public void FreeReservation(string feature)
        {
            if (_modelTokens.TryGetValue(feature, out var tokenStore))
            {
                tokenStore.Reservations--;
            }
        }

        public bool TryPeekModelToken<T, TMember>(string feature, Func<T, ParseContext, TMember> featureImplementation, ParseContext context, out TMember modelToken)
        {
            if (!_modelTokens.TryGetValue(feature, out var tokensForFeature))
            {
                if (_semanticElement is T typedElement)
                {
                    modelToken = featureImplementation(typedElement, context);
                    _modelTokens.Add(feature, new TokenStore(modelToken));
                    return true;
                }
            }
            else if (tokensForFeature != null && tokensForFeature.HasToken && tokensForFeature.LastToken is TMember result)
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
                    _modelTokens.Add(feature, new TokenStore(list));
                    if (list.Count > 0 && list[list.Count - 1] is TMember result)
                    {
                        modelToken = result;
                        return true;
                    }
                    modelToken = default;
                    return false;
                }
            }
            else if (tokensForFeature != null && tokensForFeature.HasToken && tokensForFeature.LastToken is TMember result)
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
            else if (tokensForFeature != null && tokensForFeature.HasToken && tokensForFeature.LastToken is TMember result)
            {
                modelToken = result;
                tokensForFeature.Tokens.RemoveAt(tokensForFeature.Tokens.Count - 1);
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
                    _modelTokens.Add(feature, new TokenStore(list));
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
            else if (tokensForFeature != null && tokensForFeature.HasToken && tokensForFeature.LastToken is TMember result)
            {
                modelToken = result;
                tokensForFeature.Tokens.RemoveAt(tokensForFeature.Tokens.Count - 1);
                return true;
            }
            modelToken = default;
            return false;
        }

        private class TokenStore
        {
            public TokenStore(List<object> tokens)
            {
                Tokens = tokens;
            }

            public TokenStore(object token)
            {
                Tokens = new List<object> { token };
            }

            public List<object> Tokens { get; set; }

            public int Reservations { get; set; }

            public bool HasToken => Tokens.Count > Reservations;

            public object LastToken => Tokens[Tokens.Count - 1];
        }
    }
}
