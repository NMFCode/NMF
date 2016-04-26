﻿using System;
using System.IO;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Repository;

namespace NMF.Expressions.Linq.Orleans.TestGrains
{
    public class TestModelProcessingNodeGrain<TIn, TOut> : ModelProcessingNodeGrain<TIn, TOut>, ITestModelProcessingNodeGrain<TIn, TOut>
    {

    }

    public interface ITestModelProcessingNodeGrain<TIn, TOut> : IModelProcessingNodeGrain<TIn, TOut, NMF.Models.Model>
    {
         
    }
}