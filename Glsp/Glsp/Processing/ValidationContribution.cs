using System;
using NMF.Glsp.Protocol.Validation;

namespace NMF.Glsp.Processing.Layouting;

internal class ValidationContribution<T> : ActionElement
{
    public Func<T, bool> Validator { get; init; }

    public string Label { get; init; }

    public string Description { get; init; } = "";
    
    public string Severity { get; init; }
    
    // public void Validate(T input, GElement element)
    // {
    //     var result = Validator(input);
    //
    //     if (!result)
    //     {
    //         System.Diagnostics.Debugger.Break();
    //     }
    // }
}