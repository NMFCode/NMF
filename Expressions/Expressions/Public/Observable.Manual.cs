using NMF.Expressions.Arithmetics;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// This is a facade class that exposes the functionality of NMF Expressions compactly
    /// </summary>
    public static partial class Observable
    {
        /// <summary>
        /// Create an observable expression from the given LINQ Expression, i.e. the expression updates its value according to model changes
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The LINQ Expression tree of the expression</param>
        /// <returns>A notify value</returns>
        public static INotifyValue<T> Expression<T>(Expression<Func<T>> expression)
        {
            var result = NotifySystem.CreateExpression<T>(expression.Body, expression.Parameters);
            result.Successors.SetDummy();
            return result;
        }

        /// <summary>
        /// Creates a reversable and observable expression from the given LINQ expression, i.e. the expression updates its value according to model changes and any changes to the expression are propagated back to the model
        /// </summary>
        /// <typeparam name="T">The type of the expression</typeparam>
        /// <param name="expression">The LINQ Expression tree of the expression</param>
        /// <returns>A reversable notify value</returns>
        public static INotifyReversableValue<T> Reversable<T>(Expression<Func<T>> expression)
        {
            try
            {
                var result = NotifySystem.CreateReversableExpression<T>(expression.Body, expression.Parameters);
                result.Successors.SetDummy();
                return result;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("The expression is not reversable.", "expression");
            }
        }

        /// <summary>
        /// Acesses the given array index of the given incremental array
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="array">The incremental array</param>
        /// <param name="index">The incremental index</param>
        /// <returns>An incremental expression for the array index</returns>
        public static INotifyExpression<T> ArrayIndex<T>(INotifyExpression<T[]> array, INotifyExpression<int> index)
        {
            return new ObservableIntArrayIndex<T>(array, index);
        }

        /// <summary>
        /// Acesses the given array index of the given incremental array
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="array">The incremental array</param>
        /// <param name="index">The incremental index</param>
        /// <returns>An incremental expression for the array index</returns>
        public static INotifyExpression<T> ArrayIndex<T>(INotifyExpression<T[]> array, INotifyExpression<long> index)
        {
            return new ObservableLongArrayIndex<T>(array, index);
        }

        /// <summary>
        /// Initilaizes an array incrementally
        /// </summary>
        /// <typeparam name="T">The array element type</typeparam>
        /// <param name="elements">The elements of the array</param>
        /// <returns>An incremental array</returns>
        public static INotifyExpression<T[]> ArrayInitialization<T>(IEnumerable<INotifyExpression<T>> elements)
        {
            return new ObservableArrayInitializationExpression<T>(elements);
        }

        /// <summary>
        /// Coalesces the given value with the given alternative
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="value">The incremental value that should be coalesced</param>
        /// <param name="ifNull">The incremental coalesce value</param>
        /// <returns>An incremental coalesced value</returns>
        public static INotifyExpression<T> Coalesce<T>(INotifyExpression<T> value, INotifyExpression<T> ifNull) where T : class
        {
            return new ObservableCoalesceExpression<T>(value, ifNull);
        }

        /// <summary>
        /// Coalesces the given value with the given alternative
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="value">The incremental value that should be coalesced</param>
        /// <param name="ifNull">The incremental coalesce value</param>
        /// <returns>An incremental coalesced value</returns>
        public static INotifyReversableExpression<T> Coalesce<T>(INotifyReversableExpression<T> value, INotifyReversableExpression<T> ifNull) where T : class
        {
            return new ObservableReversableCoalesceExpression<T>(value, ifNull);
        }

        /// <summary>
        /// Boxes the given incremental value
        /// </summary>
        /// <typeparam name="T">The type of the box</typeparam>
        /// <param name="value">The inner incremental value expression</param>
        /// <returns>An incremental value of type object</returns>
        public static INotifyExpression<object> Box<T>(INotifyExpression<T> value)
        {
            if(value is INotifyReversableExpression<T> reversable)
            {
                return new ObservableReversableBoxExpression<T>( reversable );
            }
            else
            {
                return new ObservableBoxExpression<T>( value );
            }
        }

        /// <summary>
        /// Boxes the given incremental value
        /// </summary>
        /// <typeparam name="T">The type of the box</typeparam>
        /// <param name="value">The inner incremental value expression</param>
        /// <returns>An incremental value of type object</returns>
        public static INotifyReversableExpression<object> Box<T>(INotifyReversableExpression<T> value)
        {
            return new ObservableReversableBoxExpression<T>(value);
        }

        /// <summary>
        /// Creates an incremental conditional
        /// </summary>
        /// <typeparam name="T">The type of the conditional</typeparam>
        /// <param name="test">An incremental value determing the test</param>
        /// <param name="ifTrue">The conditional value if the test is true</param>
        /// <param name="ifFalse">The conditional value if the test is false</param>
        /// <returns>An incremental conditional</returns>
        /// <remarks>The incremental value not used for the conditional is automatically detached</remarks>
        public static INotifyExpression<T> Conditional<T>(INotifyExpression<bool> test, INotifyExpression<T> ifTrue, INotifyExpression<T> ifFalse)
        {
            return new ObservableConditionalExpression<T>(test, ifTrue, ifFalse);
        }

        /// <summary>
        /// Creates an incremental constant value
        /// </summary>
        /// <typeparam name="T">The type of the constant</typeparam>
        /// <param name="value">The value of the constant</param>
        /// <returns>An incremental constant</returns>
        public static INotifyExpression<T> Constant<T>(T value)
        {
            return new ObservableConstant<T>(value);
        }

        /// <summary>
        /// Creates an incremental 1-dimensional array with the given bounds
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="bounds">The bounds of the array (dimension 0)</param>
        /// <returns>An incremental array</returns>
        public static INotifyExpression<T[]> NewArray<T>(INotifyExpression<int> bounds)
        {
            return new ObservableNewArray1Expression<T>(bounds);
        }

        /// <summary>
        /// Creates an incremental 2-dimensional array with the given bounds
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="bounds1">The bounds of the array (dimension 0)</param>
        /// <param name="bounds2">The bounds of the array (dimension 1)</param>
        /// <returns>An incremental array</returns>
        public static INotifyExpression<T[,]> NewArray<T>(INotifyExpression<int> bounds1, INotifyExpression<int> bounds2)
        {
            return new ObservableNewArray2Expression<T>(bounds1, bounds2);
        }

        /// <summary>
        /// Creates an incremental 3-dimensional array with the given bounds
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="bounds1">The bounds of the array (dimension 0)</param>
        /// <param name="bounds2">The bounds of the array (dimension 1)</param>
        /// <param name="bounds3">The bounds of the array (dimension 2)</param>
        /// <returns>An incremental array</returns>
        public static INotifyExpression<T[, ,]> NewArray<T>(INotifyExpression<int> bounds1, INotifyExpression<int> bounds2, INotifyExpression<int> bounds3)
        {
            return new ObservableNewArray3Expression<T>(bounds1, bounds2, bounds3);
        }

        /// <summary>
        /// Incrementally tests whether the given incremental value is an instance of the given type
        /// </summary>
        /// <param name="inner">The value that should be tested for the given type</param>
        /// <param name="type">The type for which should be tested</param>
        /// <returns>An incremental value whether the object is of the given type</returns>
        public static INotifyExpression<bool> InstanceOf(INotifyExpression<object> inner, Type type)
        {
            return new ObservableTypeExpression(inner, type, ReflectionHelper.IsValueType(type));
        }

        /// <summary>
        /// Casts the given object to the given type or returns null if not successful
        /// </summary>
        /// <typeparam name="TInner">The type of the object that is already known</typeparam>
        /// <typeparam name="TOuter">The type to which the object should be casted</typeparam>
        /// <param name="value">The incremental value to cast</param>
        /// <returns>An incremental valuen with the new type or null</returns>
        public static INotifyExpression<TOuter> As<TInner, TOuter>(INotifyExpression<TInner> value)
            where TOuter : class
        {
            return new ObservableTypeAs<TInner, TOuter>(value);
        }

        /// <summary>
        /// Converts the given object to the given type incrementally
        /// </summary>
        /// <typeparam name="TSource">The current type of the object</typeparam>
        /// <typeparam name="TTarget">The conversion type</typeparam>
        /// <param name="source">The incrementalk value that should be converted</param>
        /// <returns>An incremental value with the conversion result</returns>
        public static INotifyExpression<TTarget> Convert<TSource, TTarget>(INotifyExpression<TSource> source)
        {
            return new ObservableConvert<TSource, TTarget>(source);
        }

        /// <summary>
        /// Performs a bitwise and operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise and of the operators</returns>
        public static INotifyExpression<int> And(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntBitwiseAnd(left, right);
        }

        /// <summary>
        /// Performs a bitwise and operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise and of the operators</returns>
        public static INotifyExpression<long> And(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongBitwiseAnd(left, right);
        }

        /// <summary>
        /// Performs a bitwise and operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise and of the operators</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> And(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntBitwiseAnd(left, right);
        }

        /// <summary>
        /// Performs a bitwise and operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise and of the operators</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> And(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongBitwiseAnd(left, right);
        }

        /// <summary>
        /// Performs a bitwise or operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise or of the operators</returns>
        public static INotifyExpression<int> Or(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntBitwiseOr(left, right);
        }

        /// <summary>
        /// Performs a bitwise or operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise or of the operators</returns>
        public static INotifyExpression<long> Or(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongBitwiseOr(left, right);
        }

        /// <summary>
        /// Performs a bitwise or operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise or of the operators</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> Or(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntBitwiseOr(left, right);
        }

        /// <summary>
        /// Performs a bitwise or operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise or of the operators</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> Or(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongBitwiseOr(left, right);
        }

        /// <summary>
        /// Performs a bitwise xor operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise xor of the operators</returns>
        public static INotifyExpression<int> Xor(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntBitwiseXor(left, right);
        }

        /// <summary>
        /// Performs a bitwise xor operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise xor of the operators</returns>
        public static INotifyExpression<long> Xor(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongBitwiseXor(left, right);
        }

        /// <summary>
        /// Performs a bitwise xor operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise xor of the operators</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> Xor(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntBitwiseXor(left, right);
        }

        /// <summary>
        /// Performs a bitwise xor operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the bitwise xor of the operators</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> Xor(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongBitwiseXor(left, right);
        }

        /// <summary>
        /// Performs a logic and operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the logic and of the operators</returns>
        public static INotifyExpression<bool> And(INotifyExpression<bool> left, INotifyExpression<bool> right)
        {
            return new ObservableLogicAnd(left, right);
        }

        /// <summary>
        /// Performs a logic or operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the logic or of the operators</returns>
        public static INotifyExpression<bool> Or(INotifyExpression<bool> left, INotifyExpression<bool> right)
        {
            return new ObservableLogicOr(left, right);
        }

        /// <summary>
        /// Performs a logic xor operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the logic xor of the operators</returns>
        public static INotifyExpression<bool> Xor(INotifyExpression<bool> left, INotifyExpression<bool> right)
        {
            return new ObservableLogicXor(left, right);
        }

        /// <summary>
        /// Performs a shorthand logic and operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the logic and of the operators</returns>
        /// <remarks>While the left operand returns false, the right operand is detached from the model</remarks>
        public static INotifyExpression<bool> AndAlso(INotifyExpression<bool> left, INotifyExpression<bool> right)
        {
            return new ObservableLogicAndAlso(left, right);
        }

        /// <summary>
        /// Performs a shorthand logic or operation on the given incremental values
        /// </summary>
        /// <param name="left">The left operator</param>
        /// <param name="right">The right operator</param>
        /// <returns>An incremental value with the logic or of the operators</returns>
        /// <remarks>While the left operand returns true, the right operand is detached from the model</remarks>
        public static INotifyExpression<bool> OrElse(INotifyExpression<bool> left, INotifyExpression<bool> right)
        {
            return new ObservableLogicOrElse(left, right);
        }

        /// <summary>
        /// Performs a logic not on the given incremental value
        /// </summary>
        /// <param name="inner">The incremental value</param>
        /// <returns>An incremental inversed value</returns>
        public static INotifyExpression<bool> Not(INotifyExpression<bool> inner)
        {
            return new ObservableLogicNot(inner);
        }

        /// <summary>
        /// Computes the ones complement of the given incremental value
        /// </summary>
        /// <param name="inner">The inner incremental value</param>
        /// <returns>An incremental value with the ones complement</returns>
        public static INotifyExpression<int> OnesComplement(INotifyExpression<int> inner)
        {
            return new ObservableIntOnesComplement(inner);
        }

        /// <summary>
        /// Computes the ones complement of the given incremental value
        /// </summary>
        /// <param name="inner">The inner incremental value</param>
        /// <returns>An incremental value with the ones complement</returns>
        public static INotifyExpression<long> OnesComplement(INotifyExpression<long> inner)
        {
            return new ObservableLongOnesComplement(inner);
        }

        /// <summary>
        /// Computes the ones complement of the given incremental value
        /// </summary>
        /// <param name="inner">The inner incremental value</param>
        /// <returns>An incremental value with the ones complement</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> OnesComplement(INotifyExpression<uint> inner)
        {
            return new ObservableUIntOnesComplement(inner);
        }

        /// <summary>
        /// Computes the ones complement of the given incremental value
        /// </summary>
        /// <param name="inner">The inner incremental value</param>
        /// <returns>An incremental value with the ones complement</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> OnesComplement(INotifyExpression<ulong> inner)
        {
            return new ObservableULongOnesComplement(inner);
        }

        /// <summary>
        /// Gets the negation of the given value
        /// </summary>
        /// <param name="inner">The incremental value which should be negated</param>
        /// <returns>An incremental negated value</returns>
        public static INotifyExpression<int> Negate(INotifyExpression<int> inner)
        {
            return new ObservableUnaryIntMinus(inner);
        }

        /// <summary>
        /// Gets the negation of the given value
        /// </summary>
        /// <param name="inner">The incremental value which should be negated</param>
        /// <returns>An incremental negated value</returns>
        public static INotifyExpression<long> Negate(INotifyExpression<long> inner)
        {
            return new ObservableUnaryLongMinus(inner);
        }

        /// <summary>
        /// Gets the negation of the given value
        /// </summary>
        /// <param name="inner">The incremental value which should be negated</param>
        /// <returns>An incremental negated value</returns>
        public static INotifyExpression<float> Negate(INotifyExpression<float> inner)
        {
            return new ObservableUnaryFloatMinus(inner);
        }

        /// <summary>
        /// Gets the negation of the given value
        /// </summary>
        /// <param name="inner">The incremental value which should be negated</param>
        /// <returns>An incremental negated value</returns>
        public static INotifyExpression<double> Negate(INotifyExpression<double> inner)
        {
            return new ObservableUnaryDoubleMinus(inner);
        }

        /// <summary>
        /// Divides the given dividor by the given divident incrementally
        /// </summary>
        /// <param name="left">The divisor</param>
        /// <param name="right">The divident</param>
        /// <returns>The incremnetal division</returns>
        public static INotifyExpression<int> Divide(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntDivide(left, right);
        }

        /// <summary>
        /// Divides the given dividor by the given divident incrementally
        /// </summary>
        /// <param name="left">The divisor</param>
        /// <param name="right">The divident</param>
        /// <returns>The incremnetal division</returns>
        public static INotifyExpression<long> Divide(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongDivide(left, right);
        }

        /// <summary>
        /// Divides the given dividor by the given divident incrementally
        /// </summary>
        /// <param name="left">The divisor</param>
        /// <param name="right">The divident</param>
        /// <returns>The incremnetal division</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> Divide(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntDivide(left, right);
        }

        /// <summary>
        /// Divides the given dividor by the given divident incrementally
        /// </summary>
        /// <param name="left">The divisor</param>
        /// <param name="right">The divident</param>
        /// <returns>The incremnetal division</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> Divide(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongDivide(left, right);
        }

        /// <summary>
        /// Divides the given dividor by the given divident incrementally
        /// </summary>
        /// <param name="left">The divisor</param>
        /// <param name="right">The divident</param>
        /// <returns>The incremnetal division</returns>
        public static INotifyExpression<float> Divide(INotifyExpression<float> left, INotifyExpression<float> right)
        {
            return new ObservableFloatDivide(left, right);
        }

        /// <summary>
        /// Divides the given dividor by the given divident incrementally
        /// </summary>
        /// <param name="left">The divisor</param>
        /// <param name="right">The divident</param>
        /// <returns>The incremnetal division</returns>
        public static INotifyExpression<double> Divide(INotifyExpression<double> left, INotifyExpression<double> right)
        {
            return new ObservableDoubleDivide(left, right);
        }

        /// <summary>
        /// Subtracts the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental difference</returns>
        public static INotifyExpression<int> Subtract(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntMinus(left, right);
        }

        /// <summary>
        /// Subtracts the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental difference</returns>
        public static INotifyExpression<long> Subtract(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongMinus(left, right);
        }

        /// <summary>
        /// Subtracts the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental difference</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> Subtract(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntMinus(left, right);
        }

        /// <summary>
        /// Subtracts the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental difference</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> Subtract(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongMinus(left, right);
        }

        /// <summary>
        /// Subtracts the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental difference</returns>
        public static INotifyExpression<float> Subtract(INotifyExpression<float> left, INotifyExpression<float> right)
        {
            return new ObservableFloatMinus(left, right);
        }

        /// <summary>
        /// Subtracts the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental difference</returns>
        public static INotifyExpression<double> Subtract(INotifyExpression<double> left, INotifyExpression<double> right)
        {
            return new ObservableDoubleMinus(left, right);
        }

        /// <summary>
        /// Adds the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental sum</returns>
        public static INotifyExpression<int> Add(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntPlus(left, right);
        }

        /// <summary>
        /// Adds the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental sum</returns>
        public static INotifyExpression<long> Add(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongPlus(left, right);
        }

        /// <summary>
        /// Adds the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental sum</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> Add(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntPlus(left, right);
        }

        /// <summary>
        /// Adds the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental sum</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> Add(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongPlus(left, right);
        }

        /// <summary>
        /// Adds the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental sum</returns>
        public static INotifyExpression<float> Add(INotifyExpression<float> left, INotifyExpression<float> right)
        {
            return new ObservableFloatPlus(left, right);
        }

        /// <summary>
        /// Adds the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental sum</returns>
        public static INotifyExpression<double> Add(INotifyExpression<double> left, INotifyExpression<double> right)
        {
            return new ObservableDoublePlus(left, right);
        }

        /// <summary>
        /// Adds the given values incrementally
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental sum</returns>
        public static INotifyExpression<string> Add(INotifyExpression<string> left, INotifyExpression<string> right)
        {
            return new ObservableStringPlus(left, right);
        }

        /// <summary>
        /// Multiplies the given values incrementally
        /// </summary>
        /// <param name="left">The left factor</param>
        /// <param name="right">The right factor</param>
        /// <returns>The incremental product</returns>
        public static INotifyExpression<int> Multiply(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntMultiply(left, right);
        }

        /// <summary>
        /// Multiplies the given values incrementally
        /// </summary>
        /// <param name="left">The left factor</param>
        /// <param name="right">The right factor</param>
        /// <returns>The incremental product</returns>
        public static INotifyExpression<long> Multiply(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongMultiply(left, right);
        }

        /// <summary>
        /// Multiplies the given values incrementally
        /// </summary>
        /// <param name="left">The left factor</param>
        /// <param name="right">The right factor</param>
        /// <returns>The incremental product</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> Multiply(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntMultiply(left, right);
        }

        /// <summary>
        /// Multiplies the given values incrementally
        /// </summary>
        /// <param name="left">The left factor</param>
        /// <param name="right">The right factor</param>
        /// <returns>The incremental product</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> Multiply(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongMultiply(left, right);
        }

        /// <summary>
        /// Multiplies the given values incrementally
        /// </summary>
        /// <param name="left">The left factor</param>
        /// <param name="right">The right factor</param>
        /// <returns>The incremental product</returns>
        public static INotifyExpression<float> Multiply(INotifyExpression<float> left, INotifyExpression<float> right)
        {
            return new ObservableFloatMultiply(left, right);
        }

        /// <summary>
        /// Multiplies the given values incrementally
        /// </summary>
        /// <param name="left">The left factor</param>
        /// <param name="right">The right factor</param>
        /// <returns>The incremental product</returns>
        public static INotifyExpression<double> Multiply(INotifyExpression<double> left, INotifyExpression<double> right)
        {
            return new ObservableDoubleMultiply(left, right);
        }

        /// <summary>
        /// Computes an incremental modulo of the operands
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental modulo</returns>
        public static INotifyExpression<int> Modulo(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntModulo(left, right);
        }

        /// <summary>
        /// Computes an incremental modulo of the operands
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental modulo</returns>
        public static INotifyExpression<long> Modulo(INotifyExpression<long> left, INotifyExpression<long> right)
        {
            return new ObservableLongModulo(left, right);
        }

        /// <summary>
        /// Computes an incremental modulo of the operands
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental modulo</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> Modulo(INotifyExpression<uint> left, INotifyExpression<uint> right)
        {
            return new ObservableUIntModulo(left, right);
        }

        /// <summary>
        /// Computes an incremental modulo of the operands
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental modulo</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> Modulo(INotifyExpression<ulong> left, INotifyExpression<ulong> right)
        {
            return new ObservableULongModulo(left, right);
        }

        /// <summary>
        /// Computes an incremental modulo of the operands
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental modulo</returns>
        public static INotifyExpression<float> Modulo(INotifyExpression<float> left, INotifyExpression<float> right)
        {
            return new ObservableFloatModulo(left, right);
        }

        /// <summary>
        /// Computes an incremental modulo of the operands
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>The incremental modulo</returns>
        public static INotifyExpression<double> Modulo(INotifyExpression<double> left, INotifyExpression<double> right)
        {
            return new ObservableDoubleModulo(left, right);
        }

        /// <summary>
        /// Performs an incremental right shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        public static INotifyExpression<int> RightShift(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntRightShift(left, right);
        }

        /// <summary>
        /// Performs an incremental right shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        public static INotifyExpression<long> RightShift(INotifyExpression<long> left, INotifyExpression<int> right)
        {
            return new ObservableLongRightShift(left, right);
        }

        /// <summary>
        /// Performs an incremental right shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> RightShift(INotifyExpression<uint> left, INotifyExpression<int> right)
        {
            return new ObservableUIntRightShift(left, right);
        }

        /// <summary>
        /// Performs an incremental right shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> RightShift(INotifyExpression<ulong> left, INotifyExpression<int> right)
        {
            return new ObservableULongRightShift(left, right);
        }

        /// <summary>
        /// Performs an incremental left shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        public static INotifyExpression<int> LeftShift(INotifyExpression<int> left, INotifyExpression<int> right)
        {
            return new ObservableIntLeftShift(left, right);
        }

        /// <summary>
        /// Performs an incremental left shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        public static INotifyExpression<long> LeftShift(INotifyExpression<long> left, INotifyExpression<int> right)
        {
            return new ObservableLongLeftShift(left, right);
        }

        /// <summary>
        /// Performs an incremental left shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<uint> LeftShift(INotifyExpression<uint> left, INotifyExpression<int> right)
        {
            return new ObservableUIntLeftShift(left, right);
        }

        /// <summary>
        /// Performs an incremental left shift
        /// </summary>
        /// <param name="left">The value to be shifted</param>
        /// <param name="right">The shift</param>
        /// <returns>An incremental shift</returns>
        [CLSCompliant(false)]
        public static INotifyExpression<ulong> LeftShift(INotifyExpression<ulong> left, INotifyExpression<int> right)
        {
            return new ObservableULongLeftShift(left, right);
        }

        /// <summary>
        /// Incrementally determines whether the left value is greather than the right value
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>An incremental value determing whether the left operand is greather than the right operand</returns>
        public static INotifyExpression<bool> GreatherThan<T>(INotifyExpression<T> left, INotifyExpression<T> right)
            where T : IComparable<T>
        {
            return new ObservableGreatherThan<T>(left, right);
        }

        /// <summary>
        /// Incrementally determines whether the left value is greather than or equals to the right value
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>An incremental value determing whether the left operand is greather than or equals to the right operand</returns>
        public static INotifyExpression<bool> GreatherThanOrEquals<T>(INotifyExpression<T> left, INotifyExpression<T> right)
            where T : IComparable<T>
        {
            return new ObservableGreatherThanOrEquals<T>(left, right);
        }

        /// <summary>
        /// Incrementally determines whether the left value is less than the right value
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>An incremental value determing whether the left operand is less than the right operand</returns>
        public static INotifyExpression<bool> LessThan<T>(INotifyExpression<T> left, INotifyExpression<T> right)
            where T : IComparable<T>
        {
            return new ObservableLessThan<T>(left, right);
        }

        /// <summary>
        /// Incrementally determines whether the left value is less than or equals the right value
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>An incremental value determing whether the left operand is less than or equals the right operand</returns>
        public static INotifyExpression<bool> LessThanOrEquals<T>(INotifyExpression<T> left, INotifyExpression<T> right)
            where T : IComparable<T>
        {
            return new ObservableLessThanOrEquals<T>(left, right);
        }

        /// <summary>
        /// Incrementally determines whether the given values are equal
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>An incremental value determing whether the given values are equal</returns>
        public static INotifyExpression<bool> Equals<T>(INotifyExpression<T> left, INotifyExpression<T> right)
        {
            return new ObservableEquals<T>(left, right);
        }

        /// <summary>
        /// Incrementally determines whether the given values are not equal
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>An incremental value determing whether the given values are not equal</returns>
        public static INotifyExpression<bool> NotEquals<T>(INotifyExpression<T> left, INotifyExpression<T> right)
        {
            return new ObservableNotEquals<T>(left, right);
        }
    }
}
