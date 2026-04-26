using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NMF.Expressions.Test;
using System.Linq.Expressions;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void All_SourceNull_ArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.All<string>(null, s => s.Length > 0));
        }

        [TestMethod]
        public void All_PredicateNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.All(Source, null));
        }

        [TestMethod]
        public void Any_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Any<string>(null));
        }

        [TestMethod]
        public void LambdaAny_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Any<string>(null, s => s.Length > 0));
        }

        [TestMethod]
        public void LambdaAny_PredicateNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Any(Source, null));
        }

        [TestMethod]
        public void IntAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<int>));
        }

        [TestMethod]
        public void LongAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<long>));
        }

        [TestMethod]
        public void FloatAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<float>));
        }

        [TestMethod]
        public void DoubleAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<double>));
        }

        [TestMethod]
        public void DecimalAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<decimal>));
        }

        [TestMethod]
        public void NullableIntAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<int?>));
        }

        [TestMethod]
        public void NullableLongAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<long?>));
        }

        [TestMethod]
        public void NullableFloatAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<float?>));
        }

        [TestMethod]
        public void NullableDoubleAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<double?>));
        }

        [TestMethod]
        public void NullableDecimalAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<decimal?>));
        }

        [TestMethod]
        public void LambdaIntAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<int>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaIntAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, int>>));
        }

        [TestMethod]
        public void LambdaLongAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<long>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaLongAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, long>>));
        }

        [TestMethod]
        public void LambdaFloatAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<float>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaFloatAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, float>>));
        }

        [TestMethod]
        public void LambdaDoubleAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<double>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaDoubleAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, double>>));
        }

        [TestMethod]
        public void LambdaDecimalAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<decimal>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaDecimalAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, decimal>>));
        }

        [TestMethod]
        public void LambdaNullableIntAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<int?>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaNullableIntAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, int?>>));
        }

        [TestMethod]
        public void LambdaNullableLongAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<long?>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaNullableLongAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, long?>>));
        }

        [TestMethod]
        public void LambdaNullableFloatAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<float?>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaNullableFloatAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, float?>>));
        }

        [TestMethod]
        public void LambdaNullableDoubleAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<double?>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaNullableDoubleAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, double?>>));
        }

        [TestMethod]
        public void LambdaNullableDecimalAverage_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(null as INotifyEnumerable<Dummy<decimal?>>, d => d.Item));
        }

        [TestMethod]
        public void LambdaNullableDecimalAverage_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Average(Source, null as Expression<Func<string, decimal?>>));
        }

        [TestMethod]
        public void Cast_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Cast<string>(null));
        }

        [TestMethod]
        public void Contains_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Contains(null, "42"));
        }

        [TestMethod]
        public void Count_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Count(null as INotifyEnumerable<string>));
        }

        [TestMethod]        
        public void CountPredicate_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Count(null as INotifyEnumerable<string>, s => true));
        }

        [TestMethod]        
        public void CountPredicate_PredicateNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Count(Source, null));
        }

        [TestMethod]        
        public void Concat_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Concat(null, Source));
        }

        [TestMethod]        
        public void Distinct_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Distinct(null as INotifyEnumerable<string>));
        }

        [TestMethod]        
        public void DistinctComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Distinct(null, StringComparer.Ordinal));
        }

        [TestMethod]        
        public void Except_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Except(null, Source));
        }

        [TestMethod]        
        public void ExceptComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Except(null, Source, StringComparer.Ordinal));
        }

        [TestMethod]        
        public void FirstOrDefault_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.FirstOrDefault(null as INotifyEnumerable<string>));
        }

        [TestMethod]        
        public void FirstOrDefaultPredicate_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.FirstOrDefault(null as INotifyEnumerable<string>, s => true));
        }

        [TestMethod]        
        public void FirstOrDefaultPredicate_PredicateNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.FirstOrDefault(Source, null));
        }

        [TestMethod]        
        public void GroupBy_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.GroupBy(null as INotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void GroupBy_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.GroupBy<string, int>(Source, null));
        }

        [TestMethod]        
        public void GroupByComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.GroupBy(null as INotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void GroupByComparer_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.GroupBy<string, int>(Source, null, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void Join_OuterSourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(null, new List<string>(), s => s.Length, s => s.Length, (s1, s2) => s1 + s2));
        }

        [TestMethod]        
        public void Join_InnerSourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), null, s => s.Length, s => s.Length, (s1, s2) => s1 + s2));
        }

        [TestMethod]        
        public void Join_OuterKeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), null, s => s.Length, (s1, s2) => s1 + s2));
        }

        [TestMethod]        
        public void Join_InnerKeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, null, (s1, s2) => s1 + s2));
        }

        [TestMethod]        
        public void Join_ResultSelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, s => s.Length, null));
        }

        [TestMethod]        
        public void JoinComparer_OuterSourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(null, new List<string>(), s => s.Length, s => s.Length, (s1, s2) => s1 + s2));
        }

        [TestMethod]        
        public void JoinComparer_InnerSourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), null, s => s.Length, s => s.Length, (s1, s2) => s1 + s2, null));
        }

        [TestMethod]        
        public void JoinComparer_OuterKeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), null, s => s.Length, (s1, s2) => s1 + s2, null));
        }

        [TestMethod]        
        public void JoinComparer_InnerKeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, null, (s1, s2) => s1 + s2, null));
        }

        [TestMethod]        
        public void JoinComparer_ResultSelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, s => s.Length, null, null));
        }

        [TestMethod]        
        public void Intersect_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Intersect(null, Source));
        }

        [TestMethod]        
        public void IntersectComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Intersect(null, Source, StringComparer.Ordinal));
        }

        [TestMethod]        
        public void Max_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as INotifyEnumerable<int>));
        }

        [TestMethod]        
        public void MaxComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as INotifyEnumerable<int>, null));
        }

        [TestMethod]        
        public void LambdaMax_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as INotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void LambdaMax_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(Source, null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void LambdaMaxComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as  INotifyEnumerable<string>, s => s.Length, null));
        }

        [TestMethod]        
        public void LambdaMaxComparer_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(Source, null as Expression<Func<string, int>>, null));
        }

        [TestMethod]        
        public void NullableMax_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as INotifyEnumerable<int?>));
        }

        [TestMethod]        
        public void NullableMaxComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as INotifyEnumerable<int?>, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void LambdaNullableMax_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as INotifyEnumerable<string>, s => new int?(s.Length)));
        }

        [TestMethod]        
        public void LambdaNullableMax_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(Source, null as Expression<Func<string, int?>>));
        }
        
        [TestMethod]        
        public void LambdaNullableMaxComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(null as INotifyEnumerable<string>, s => new int?(s.Length), new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void LambdaNullableMaxComparer_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Max(Source, null as Expression<Func<string, int?>>, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void Min_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<int>));
        }

        [TestMethod]        
        public void MinComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<int>, null));
        }

        [TestMethod]        
        public void LambdaMin_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void LambdaMin_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(Source, null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void LambdaMinComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<string>, s => s.Length, null));
        }

        [TestMethod]        
        public void LambdaMinComparer_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(Source, null as Expression<Func<string, int>>, null));
        }

        [TestMethod]        
        public void NullableMin_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<int?>));
        }

        [TestMethod]        
        public void NullableMinComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<int?>, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void LambdaNullableMin_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<string>, s => new int?(s.Length)));
        }

        [TestMethod]        
        public void LambdaNullableMin_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(Source, null as Expression<Func<string, int?>>));
        }

        [TestMethod]        
        public void LambdaNullableMinComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(null as INotifyEnumerable<string>, s => new int?(s.Length), new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void LambdaNullableMinComparer_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Min(Source, null as Expression<Func<string, int?>>, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void OfType_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OfType<string>(null));
        }

        [TestMethod]        
        public void OrderBy_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderBy(null as INotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void OrderBy_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderBy(Source, null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void OrderByComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderBy(null as INotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void OrderByComparer_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderBy(Source, null, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void OrderByDescending_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderByDescending(null as INotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void OrderByDescending_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderByDescending(Source, null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void OrderByDescendingComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderByDescending(null as INotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void OrderByDescendingComparer_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.OrderByDescending(Source, null, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void Select_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Select(null as INotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void Select_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Select(Source, null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void SelectMany_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.SelectMany(null as INotifyEnumerable<string>, s => s.ToCharArray(), (s, c) => char.IsLetterOrDigit(c)));
        }

        [TestMethod]        
        public void SelectMany_FuncNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.SelectMany(Source, null as Expression<Func<string, IEnumerable<char>>>, (s, c) => char.IsLetterOrDigit(c)));
        }

        [TestMethod]        
        public void SelectMany_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.SelectMany(Source, s => s.ToCharArray(), null as Expression<Func<string, char, bool>>));
        }

        [TestMethod]        
        public void IntSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<int>));
        }

        [TestMethod]        
        public void LongSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<long>));
        }

        [TestMethod]        
        public void FloatSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<float>));
        }

        [TestMethod]        
        public void DoubleSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<double>));
        }

        [TestMethod]        
        public void DecimalSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<decimal>));
        }

        [TestMethod]        
        public void NullableIntSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<int?>));
        }

        [TestMethod]        
        public void NullableLongSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<long?>));
        }

        [TestMethod]        
        public void NullableFloatSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<float?>));
        }

        [TestMethod]        
        public void NullableDoubleSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<double?>));
        }

        [TestMethod]        
        public void NullableDecimalSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<decimal?>));
        }

        [TestMethod]        
        public void LambdaIntSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<int>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaIntSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void LambdaLongSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<long>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaLongSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, long>>));
        }

        [TestMethod]        
        public void LambdaFloatSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<float>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaFloatSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, float>>));
        }

        [TestMethod]        
        public void LambdaDoubleSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<double>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaDoubleSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, double>>));
        }

        [TestMethod]        
        public void LambdaDecimalSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<decimal>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaDecimalSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, decimal>>));
        }

        [TestMethod]        
        public void LambdaNullableIntSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<int?>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaNullableIntSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, int?>>));
        }

        [TestMethod]        
        public void LambdaNullableLongSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<long?>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaNullableLongSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, long?>>));
        }

        [TestMethod]        
        public void LambdaNullableFloatSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<float?>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaNullableFloatSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, float?>>));
        }

        [TestMethod]        
        public void LambdaNullableDoubleSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<double?>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaNullableDoubleSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, double?>>));
        }

        [TestMethod]        
        public void LambdaNullableDecimalSum_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<decimal?>>, d => d.Item));
        }

        [TestMethod]        
        public void LambdaNullableDecimalSum_SelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Sum(Source, null as Expression<Func<string, decimal?>>));
        }

        [TestMethod]        
        public void ThenBy_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenBy(null as IOrderableNotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void ThenBy_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenBy(new OrderableList<string>(), null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void ThenByComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenBy(null as IOrderableNotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void ThenByComparer_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenBy(new OrderableList<string>(), null as Expression<Func<string, int>>, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void ThenByDescending_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenByDescending(null as IOrderableNotifyEnumerable<string>, s => s.Length));
        }

        [TestMethod]        
        public void ThenByDescending_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenByDescending(new OrderableList<string>(), null as Expression<Func<string, int>>));
        }

        [TestMethod]        
        public void ThenByDescendingComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenByDescending(null as IOrderableNotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void ThenByDescendingComparer_KeySelectorNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.ThenByDescending(new OrderableList<string>(), null as Expression<Func<string, int>>, new AbsoluteValueComparer()));
        }

        [TestMethod]        
        public void Union_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Union(null, Source));
        }

        [TestMethod]        
        public void UnionComparer_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Union(null, Source, StringComparer.Ordinal));
        }

        [TestMethod]        
        public void Where_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Where(null as INotifyEnumerable<string>, s => true));
        }

        [TestMethod]        
        public void Where_FilterNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.Where(Source, null));
        }

        [TestMethod]        
        public void WithUpdates_SourceNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ObservableExtensions.WithUpdates<string>(null));
        }


        private INotifyEnumerable<string> Source
        {
            get
            {
                return CreateSource<string>();
            }
        }

        private INotifyEnumerable<T> CreateSource<T>()
        {
            return new List<T>().WithUpdates();
        }
    }
}
