using System;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

internal class StringBatchValidationContribution<T> : BatchValidationContribution
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
