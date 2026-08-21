using System;
using System.Collections.Generic;
using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing;

internal abstract class LiveValidationContribution : ActionElement
{
    public abstract IDisposable Observe(object semanticElement, GElement element);

    protected abstract Marker GetCurrentMarker(GElement element, IDisposable observer);

    protected void CheckLiveValidationResults(GElement element, List<Marker> markers)
    {
        foreach (var observer in element.LiveValidationObservers)
        {
            var marker = observer.Key.GetCurrentMarker(element, observer.Value);

            if (marker != null)
            {
                markers.Add(marker);
            }
        }

        foreach (var child in element.Children)
        {
            CheckLiveValidationResults(child, markers);
        }
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

internal class BooleanLiveValidationContribution<T> : LiveValidationContribution<T, bool>
{
    public string Label { get; init; }
    public string Description { get; init; }
    public string Severity { get; init; }

    protected override Marker GetCurrentMarker(GElement element,  IDisposable observer) 
    {
        var boolObs = (INotifyValue<bool>)observer;
        if (boolObs.Value)
            return null;

        return new Marker
        {
            ElementId = element.Id,
            Label = Label,
            Description = Description,
            Kind = Severity
        };
    }
}

internal class StringLiveValidationContribution<T> : LiveValidationContribution<T, string>
{
    public string Severity { get; init; }

    protected override Marker GetCurrentMarker(GElement element, IDisposable observer)
    {
        var stringObs = (INotifyValue<string>)observer;
        if (stringObs.Value == null)
            return null;

        return new Marker
        {
            ElementId = element.Id,
            Label = stringObs.Value,
            Description = stringObs.Value,
            Kind = Severity
        };
    }
}

internal class MarkerLiveValidationContribution<T> : LiveValidationContribution<T, Marker>
{
    protected override Marker GetCurrentMarker(GElement element,  IDisposable observer)
    {
        var markerObs = (INotifyValue<Marker>)observer;
        if (markerObs.Value == null)
            return null;

        return new Marker
        {
            ElementId = element.Id,
            Label = markerObs.Value.Label,
            Description = markerObs.Value.Description,
            Kind = markerObs.Value.Kind
        };
    }
}