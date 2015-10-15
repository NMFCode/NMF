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
        [ExpectedException(typeof(ArgumentNullException))]
        public void All_SourceNull_ArgumentException()
        {
            ObservableExtensions.All<string>(null, s => s.Length > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void All_PredicateNull_ArgumentNullException()
        {
            ObservableExtensions.All(Source, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Any_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Any<string>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaAny_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Any<string>(null, s => s.Length > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaAny_PredicateNull_ArgumentNullException()
        {
            ObservableExtensions.Any(Source, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IntAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<int>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LongAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<long>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FloatAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<float>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DoubleAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<double>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecimalAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<decimal>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableIntAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<int?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableLongAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<long?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableFloatAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<float?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableDoubleAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<double?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableDecimalAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<decimal?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaIntAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<int>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaIntAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaLongAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<long>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaLongAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, long>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaFloatAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<float>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaFloatAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, float>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDoubleAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<double>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDoubleAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, double>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDecimalAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<decimal>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDecimalAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, decimal>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableIntAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<int?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableIntAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, int?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableLongAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<long?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableLongAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, long?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableFloatAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<float?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableFloatAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, float?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDoubleAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<double?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDoubleAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, double?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDecimalAverage_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Average(null as INotifyEnumerable<Dummy<decimal?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDecimalAverage_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Average(Source, null as Expression<Func<string, decimal?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cast_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Cast<string>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Contains_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Contains(null, "42");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Count_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Count(null as INotifyEnumerable<string>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountPredicate_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Count(null as INotifyEnumerable<string>, s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountPredicate_PredicateNull_ArgumentNullException()
        {
            ObservableExtensions.Count(Source, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Concat_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Concat(null, Source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Distinct_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Distinct(null as INotifyEnumerable<string>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Distinct(null, StringComparer.Ordinal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Except_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Except(null, Source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Except(null, Source, StringComparer.Ordinal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FirstOrDefault_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.FirstOrDefault(null as INotifyEnumerable<string>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FirstOrDefaultPredicate_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.FirstOrDefault(null as INotifyEnumerable<string>, s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FirstOrDefaultPredicate_PredicateNull_ArgumentNullException()
        {
            ObservableExtensions.FirstOrDefault(Source, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupBy_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.GroupBy(null as INotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupBy_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.GroupBy<string, int>(Source, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupByComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.GroupBy(null as INotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupByComparer_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.GroupBy<string, int>(Source, null, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Join_OuterSourceNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(null, new List<string>(), s => s.Length, s => s.Length, (s1, s2) => s1 + s2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Join_InnerSourceNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), null, s => s.Length, s => s.Length, (s1, s2) => s1 + s2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Join_OuterKeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), null, s => s.Length, (s1, s2) => s1 + s2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Join_InnerKeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, null, (s1, s2) => s1 + s2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Join_ResultSelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, s => s.Length, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JoinComparer_OuterSourceNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(null, new List<string>(), s => s.Length, s => s.Length, (s1, s2) => s1 + s2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JoinComparer_InnerSourceNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), null, s => s.Length, s => s.Length, (s1, s2) => s1 + s2, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JoinComparer_OuterKeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), null, s => s.Length, (s1, s2) => s1 + s2, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JoinComparer_InnerKeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, null, (s1, s2) => s1 + s2, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JoinComparer_ResultSelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Join<string, string, int, string>(new NotifyCollection<string>(), new List<string>(), s => s.Length, s => s.Length, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Intersect_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Intersect(null, Source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IntersectComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Intersect(null, Source, StringComparer.Ordinal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Max_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as INotifyEnumerable<int>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as INotifyEnumerable<int>, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMax_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as INotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMax_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Max(Source, null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMaxComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as  INotifyEnumerable<string>, s => s.Length, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMaxComparer_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Max(Source, null as Expression<Func<string, int>>, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableMax_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as INotifyEnumerable<int?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableMaxComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as INotifyEnumerable<int?>, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMax_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as INotifyEnumerable<string>, s => new int?(s.Length));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMax_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Max(Source, null as Expression<Func<string, int?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMaxComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Max(null as INotifyEnumerable<string>, s => new int?(s.Length), new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMaxComparer_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Max(Source, null as Expression<Func<string, int?>>, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Min_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<int>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<int>, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMin_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMin_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Min(Source, null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMinComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<string>, s => s.Length, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaMinComparer_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Min(Source, null as Expression<Func<string, int>>, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableMin_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<int?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableMinComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<int?>, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMin_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<string>, s => new int?(s.Length));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMin_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Min(Source, null as Expression<Func<string, int?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMinComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Min(null as INotifyEnumerable<string>, s => new int?(s.Length), new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableMinComparer_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Min(Source, null as Expression<Func<string, int?>>, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OfType_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.OfType<string>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderBy_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.OrderBy(null as INotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderBy_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.OrderBy(Source, null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderByComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.OrderBy(null as INotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderByComparer_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.OrderBy(Source, null, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderByDescending_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.OrderByDescending(null as INotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderByDescending_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.OrderByDescending(Source, null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderByDescendingComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.OrderByDescending(null as INotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderByDescendingComparer_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.OrderByDescending(Source, null, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Select_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Select(null as INotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Select_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Select(Source, null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SelectMany_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.SelectMany(null as INotifyEnumerable<string>, s => s.ToCharArray(), (s, c) => char.IsLetterOrDigit(c));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SelectMany_FuncNull_ArgumentNullException()
        {
            ObservableExtensions.SelectMany(Source, null as Expression<Func<string, IEnumerable<char>>>, (s, c) => char.IsLetterOrDigit(c));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SelectMany_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.SelectMany(Source, s => s.ToCharArray(), null as Expression<Func<string, char, bool>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IntSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<int>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LongSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<long>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FloatSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<float>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DoubleSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<double>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecimalSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<decimal>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableIntSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<int?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableLongSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<long?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableFloatSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<float?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableDoubleSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<double?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullableDecimalSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<decimal?>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaIntSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<int>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaIntSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaLongSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<long>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaLongSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, long>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaFloatSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<float>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaFloatSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, float>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDoubleSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<double>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDoubleSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, double>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDecimalSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<decimal>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaDecimalSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, decimal>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableIntSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<int?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableIntSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, int?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableLongSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<long?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableLongSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, long?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableFloatSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<float?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableFloatSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, float?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDoubleSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<double?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDoubleSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, double?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDecimalSum_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(null as INotifyEnumerable<Dummy<decimal?>>, d => d.Item);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaNullableDecimalSum_SelectorNull_ArgumentNullException()
        {
            ObservableExtensions.Sum(Source, null as Expression<Func<string, decimal?>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenBy_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.ThenBy(null as IOrderableNotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenBy_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.ThenBy(new OrderableList<string>(), null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenByComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.ThenBy(null as IOrderableNotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenByComparer_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.ThenBy(new OrderableList<string>(), null as Expression<Func<string, int>>, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenByDescending_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.ThenByDescending(null as IOrderableNotifyEnumerable<string>, s => s.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenByDescending_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.ThenByDescending(new OrderableList<string>(), null as Expression<Func<string, int>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenByDescendingComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.ThenByDescending(null as IOrderableNotifyEnumerable<string>, s => s.Length, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenByDescendingComparer_KeySelectorNull_ArgumentNullException()
        {
            ObservableExtensions.ThenByDescending(new OrderableList<string>(), null as Expression<Func<string, int>>, new AbsoluteValueComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Union_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Union(null, Source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnionComparer_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Union(null, Source, StringComparer.Ordinal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Where_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.Where(null as INotifyEnumerable<string>, s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Where_FilterNull_ArgumentNullException()
        {
            ObservableExtensions.Where(Source, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithUpdates_SourceNull_ArgumentNullException()
        {
            ObservableExtensions.WithUpdates<string>(null);
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
