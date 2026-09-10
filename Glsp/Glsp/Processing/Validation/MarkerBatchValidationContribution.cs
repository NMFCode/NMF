using System;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

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