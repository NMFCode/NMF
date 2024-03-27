﻿using System;

namespace NMF.Transformations
{
    /// <summary>
    /// Marks a transformation rule to override another transformation rule
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public sealed class OverrideRuleAttribute : Attribute
    {
    }
}
