using System;
using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

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
