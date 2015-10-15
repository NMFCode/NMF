using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Linq.Tests
{
	public static class AssertExtensions
	{
		public static void AssertSequence<T>(this IEnumerable<T> sequence, params T[] assertedSequence)
		{
			int i = 0;
			Assert.IsNotNull(sequence);
			foreach (var item in sequence)
			{
				Assert.IsTrue(assertedSequence.Length > i);
				Assert.AreEqual(assertedSequence[i], item);
				i++;
			}
			Assert.AreEqual(i, assertedSequence.Length);
		}
	}
}
