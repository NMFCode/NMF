using System;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Validation;

internal abstract class BatchValidationContribution : ActionElement
{
    public abstract Marker Validate(object element, string elementId);
}
