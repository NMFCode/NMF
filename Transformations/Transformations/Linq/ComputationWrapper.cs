using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Linq
{
    /// <summary>
    /// Wraps computations from the type TIn to the type TOut
    /// </summary>
    /// <typeparam name="TIn">The input type for the wrapped computation</typeparam>
    /// <typeparam name="TOut">The output type for the wrapped computations</typeparam>
    public struct TransformationComputationWrapper<TIn, TOut> : IEquatable<TransformationComputationWrapper<TIn, TOut>>
        where TIn : class
        where TOut : class
    {
        private Computation c;

        /// <summary>
        /// Wraps the given computation into a typed wrapping structure
        /// </summary>
        /// <param name="inner">The computation that is to be wrapped</param>
        public TransformationComputationWrapper(Computation inner)
        {
            this.c = inner;
        }

        /// <summary>
        /// Gets the input of the represented computation
        /// </summary>
        public TIn Input
        {
            get
            {
                if (c != null)
                {
                    return c.GetInput(0) as TIn;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the output of the represented computation
        /// </summary>
        public TOut Output
        {
            get
            {
                if (c != null)
                {
                    return c.Output as TOut;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the underlying computation
        /// </summary>
        public Computation Computation
        {
            get { return c; }
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="other">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public bool Equals(TransformationComputationWrapper<TIn, TOut> other)
        {
            return other.c == c;
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="obj">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj is TransformationComputationWrapper<TIn, TOut>)
                return Equals((TransformationComputationWrapper<TIn, TOut>)obj);
            return false;
        }

        /// <summary>
        /// Gets a hash code representation of the current value
        /// </summary>
        /// <returns>A hash code representation of the value represented by the current instance</returns>
        public override int GetHashCode()
        {
            return c != null ? c.GetHashCode() : 0;
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>True, if both wrappers represent the same computation, otherwise false</returns>
        public static bool operator ==(TransformationComputationWrapper<TIn, TOut> left, TransformationComputationWrapper<TIn, TOut> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as not equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>False, if both wrappers represent the same computation, otherwise true</returns>
        public static bool operator !=(TransformationComputationWrapper<TIn, TOut> left, TransformationComputationWrapper<TIn, TOut> right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Wraps computations from the type TIn to the type TOut
    /// </summary>
    /// <typeparam name="TIn1">The first input type for the wrapped computation</typeparam>
    /// <typeparam name="TIn2">The second input type for the wrapped computation</typeparam>
    /// <typeparam name="TOut">The output type for the wrapped computations</typeparam>
    
    public struct TransformationComputationWrapper<TIn1, TIn2, TOut> : IEquatable<TransformationComputationWrapper<TIn1, TIn2, TOut>>
        where TIn1 : class
        where TIn2 : class
        where TOut : class
    {
        private Computation c;

        /// <summary>
        /// Wraps the given computation into a typed wrapping structure
        /// </summary>
        /// <param name="inner">The computation that is to be wrapped</param>
        public TransformationComputationWrapper(Computation inner)
        {
            this.c = inner;
        }

        /// <summary>
        /// Gets the first input of the represented computation
        /// </summary>
        public TIn1 Input1
        {
            get
            {
                if (c != null)
                {
                    return c.GetInput(0) as TIn1;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the second input of the represented computation
        /// </summary>
        public TIn2 Input2
        {
            get
            {
                if (c != null)
                {
                    return c.GetInput(1) as TIn2;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the output of the represented computation
        /// </summary>
        public TOut Output
        {
            get
            {
                if (c != null)
                {
                    return c.Output as TOut;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the underlying computation
        /// </summary>
        public Computation Computation
        {
            get { return c; }
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="other">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public bool Equals(TransformationComputationWrapper<TIn1, TIn2, TOut> other)
        {
            return other.c == c;
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="obj">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj is TransformationComputationWrapper<TIn1, TIn2, TOut>)
                return Equals((TransformationComputationWrapper<TIn1, TIn2, TOut>)obj);
            return false;
        }

        /// <summary>
        /// Gets a hash code representation of the current value
        /// </summary>
        /// <returns>A hash code representation of the value represented by the current instance</returns>
        public override int GetHashCode()
        {
            return c != null ? c.GetHashCode() : 0;
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>True, if both wrappers represent the same computation, otherwise false</returns>
        public static bool operator ==(TransformationComputationWrapper<TIn1, TIn2, TOut> left, TransformationComputationWrapper<TIn1, TIn2, TOut> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as not equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>False, if both wrappers represent the same computation, otherwise true</returns>
        public static bool operator !=(TransformationComputationWrapper<TIn1, TIn2, TOut> left, TransformationComputationWrapper<TIn1, TIn2, TOut> right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Wraps computations from the type TIn to the type TOut
    /// </summary>
    /// <typeparam name="TIn">The input type for the wrapped computation</typeparam>
    public struct InPlaceComputationWrapper<TIn> : IEquatable<InPlaceComputationWrapper<TIn>>
        where TIn : class
    {
        private Computation c;

        /// <summary>
        /// Wraps the given computation into a typed wrapping structure
        /// </summary>
        /// <param name="inner">The computation that is to be wrapped</param>
        public InPlaceComputationWrapper(Computation inner)
        {
            this.c = inner;
        }

        /// <summary>
        /// Gets the input of the represented computation
        /// </summary>
        public TIn Input
        {
            get
            {
                if (c != null)
                {
                    return c.GetInput(0) as TIn;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the underlying computation
        /// </summary>
        public Computation Computation
        {
            get { return c; }
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="other">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public bool Equals(InPlaceComputationWrapper<TIn> other)
        {
            return other.c == c;
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="obj">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj is InPlaceComputationWrapper<TIn>)
                return Equals((InPlaceComputationWrapper<TIn>)obj);
            return false;
        }

        /// <summary>
        /// Gets a hash code representation of the current value
        /// </summary>
        /// <returns>A hash code representation of the value represented by the current instance</returns>
        public override int GetHashCode()
        {
            return c != null ? c.GetHashCode() : 0;
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>True, if both wrappers represent the same computation, otherwise false</returns>
        public static bool operator ==(InPlaceComputationWrapper<TIn> left, InPlaceComputationWrapper<TIn> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as not equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>False, if both wrappers represent the same computation, otherwise true</returns>
        public static bool operator !=(InPlaceComputationWrapper<TIn> left, InPlaceComputationWrapper<TIn> right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Wraps computations from the type TIn to the type TOut
    /// </summary>
    /// <typeparam name="TIn1">The first input type for the wrapped computation</typeparam>
    /// <typeparam name="TIn2">The second input type for the wrapped computation</typeparam>
    public struct InPlaceComputationWrapper<TIn1, TIn2> : IEquatable<InPlaceComputationWrapper<TIn1, TIn2>>
        where TIn1 : class
        where TIn2 : class
    {
        private Computation c;

        /// <summary>
        /// Wraps the given computation into a typed wrapping structure
        /// </summary>
        /// <param name="inner">The computation that is to be wrapped</param>
        public InPlaceComputationWrapper(Computation inner)
        {
            this.c = inner;
        }

        /// <summary>
        /// Gets the first input of the represented computation
        /// </summary>
        public TIn1 Input1
        {
            get
            {
                if (c != null)
                {
                    return c.GetInput(0) as TIn1;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the second input of the represented computation
        /// </summary>
        public TIn2 Input2
        {
            get
            {
                if (c != null)
                {
                    return c.GetInput(1) as TIn2;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the underlying computation
        /// </summary>
        public Computation Computation
        {
            get { return c; }
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="other">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public bool Equals(InPlaceComputationWrapper<TIn1, TIn2> other)
        {
            return other.c == c;
        }

        /// <summary>
        /// Gets a value indicating whether the current transformation computation wrapper and the provided instance should be treated as equal
        /// </summary>
        /// <param name="obj">The other instance</param>
        /// <returns>True, if the current object represents the same value as the given parameter, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj is InPlaceComputationWrapper<TIn1, TIn2>)
                return Equals((InPlaceComputationWrapper<TIn1, TIn2>)obj);
            return false;
        }

        /// <summary>
        /// Gets a hash code representation of the current value
        /// </summary>
        /// <returns>A hash code representation of the value represented by the current instance</returns>
        public override int GetHashCode()
        {
            return c != null ? c.GetHashCode() : 0;
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>True, if both wrappers represent the same computation, otherwise false</returns>
        public static bool operator ==(InPlaceComputationWrapper<TIn1, TIn2> left, InPlaceComputationWrapper<TIn1, TIn2> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Gets a value indicating whether the two instances of the transformationwrapper should be as not equal
        /// </summary>
        /// <param name="left">The first computation wrapper</param>
        /// <param name="right">The second computation wrapper</param>
        /// <returns>False, if both wrappers represent the same computation, otherwise true</returns>
        public static bool operator !=(InPlaceComputationWrapper<TIn1, TIn2> left, InPlaceComputationWrapper<TIn1, TIn2> right)
        {
            return !left.Equals(right);
        }
    }
}
