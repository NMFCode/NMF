using System;
using System.Collections.Generic;
using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing;

internal abstract class ValidationContribution : ActionElement
{
    public abstract Marker Validate(object element, string elementId);
}

internal class BooleanValidationContribution<T> : ValidationContribution
{
    public Func<T, bool> Validator { get; init; }
    public string Label { get; init; }
    public string Description { get; init; }
    public string Severity { get; init; }
    public override Marker Validate(object element, string elementId)
    {
        if (Validator((T)element))
            return null;
        
        return new Marker
        {
            ElementId = elementId,
            Label = Label,
            Description = Description,
            Kind = Severity
        };
    }
}

internal class StringValidationContribution<T> : ValidationContribution
{
    public Func<T, string> Validator { get; init; }

    public string Severity { get; init; }

    public override Marker Validate(object element, string elementId)
    {
        var message = Validator((T)element);

        if (message == null)
            return null;

        return new Marker
        {
            ElementId = elementId,
            Label = message,
            Description = message,
            Kind = Severity
        };
    }
}

internal class MarkerValidationContribution<T> : ValidationContribution
{
    public Func<T, Marker> Validator { get; init; }

    public override Marker Validate(object element, string elementId)
    {
        var marker =  Validator((T)element);
        
        if (marker == null)
            return null;
        
        return new Marker
        {
            ElementId = elementId,
            Label = marker.Label,
            Description = marker.Description,
            Kind = marker.Kind
        };

    }
}

internal abstract class LiveValidationContribution : ActionElement
{
    public abstract IDisposable Observe(object semanticElement, GElement element);

    protected abstract Marker GetCurrentMarker(GElement element, IDisposable observer);

    protected void CheckLiveValidationResults(GElement element, List<Marker> markers)
    {
        foreach (var observer in element.ValidationObservers)
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

internal class LiveBooleanValidationContribution<T> : LiveValidationContribution<T, bool>
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

internal class LiveStringValidationContribution<T> : LiveValidationContribution<T, string>
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

internal class LiveMarkerValidationContribution<T> : LiveValidationContribution<T, Marker>
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