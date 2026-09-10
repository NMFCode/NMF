using System;
using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

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
