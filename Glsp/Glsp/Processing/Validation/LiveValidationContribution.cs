using System;
using System.Collections.Generic;
using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

internal abstract class LiveValidationContribution : ActionElement
{
    public abstract IDisposable Observe(object semanticElement, GElement element);

    protected abstract Marker GetCurrentMarker(GElement element, IDisposable observer);

    protected void CheckLiveValidationResults(GElement element, List<Marker> markers)
    {
        foreach (var observer in element.Collectibles)
        {
            if (observer.Key is LiveValidationContribution liveValidationContribution)
            {
                var marker = liveValidationContribution.GetCurrentMarker(element, observer.Value);
                if (marker != null)
                {
                    markers.Add(marker);
                }
            }
        }

        foreach (var child in element.Children)
        {
            CheckLiveValidationResults(child, markers);
        }
    }

    public void StartObserving<T>(T input, GElement element)
    {
        element.Collectibles.Add(this, Observe(input, element));
    }
}

internal abstract class LiveValidationContribution<T, TResult> : LiveValidationContribution
{
    public ObservingFunc<T, TResult> Validator { get; init; }
    
    public override IDisposable Observe(object semanticElement, GElement element)
    {
        var result = Validator.Observe((T)semanticElement);

        result.Successors.SetDummy();

        result.ValueChanged += (_, _) =>
        {
            var markers = new List<Marker>();

            CheckLiveValidationResults(element.Graph, markers);

            element.Graph.OnMarkersChanged(markers);
        };

        return result;
    }
}
