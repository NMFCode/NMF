using System;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing;

internal abstract class BatchValidationContribution : ActionElement
{
    public abstract Marker Validate(object element, string elementId);
}

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

internal class MarkerBatchValidationContribution<T> : BatchValidationContribution
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