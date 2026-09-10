using System;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

internal class BooleanBatchValidationContribution<T> : BatchValidationContribution
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
