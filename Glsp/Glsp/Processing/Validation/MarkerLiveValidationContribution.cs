using System;
using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

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