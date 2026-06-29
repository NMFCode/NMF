using System;
using NMF.Expressions;

namespace NMF.Glsp.Processing;

internal abstract class ValidationContribution : ActionElement
{
    public string Label {get;init;}
    public string Description {get;init;}
    public string Severity {get;init;}
}

internal class ValidationContribution<T> : ValidationContribution
{
    public Func<T, bool> Validator { get; init; } 
}

internal class LiveValidationContribution<T> : ValidationContribution
{
    public ObservingFunc<T, bool> Validator { get; init; } 

    public INotifyValue<bool> Observe(object element)
    {
        return Validator.Observe((T)element);
    }
}