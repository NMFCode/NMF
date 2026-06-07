using System;
using NMF.Glsp.Graph;

namespace NMF.Glsp.Processing.Layouting;

internal class ValidationContribution<T> : ActionElement
{
    public Func<T, bool> Validator { get; init; }

    public string Message { get; init; }
    
    //public MarkerLevel Marker { get; init; }
    
    public void Validate(T input, GElement element)
    {
        var result = Validator(input);

        if (!result)
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}