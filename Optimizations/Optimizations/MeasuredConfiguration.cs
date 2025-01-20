using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NMF.Optimizations
{
    /// <summary>
    /// Denotes a measured configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{Representation}")]
    public struct MeasuredConfiguration<T> : IEquatable<MeasuredConfiguration<T>>
    {
        /// <summary>
        /// The actual configuration
        /// </summary>
        public T Configuration { get; private set; }

        /// <summary>
        /// The measurements for this configuration
        /// </summary>
        public IDictionary<string, double> Measurements { get; private set; }

        /// <summary>
        /// A representation of this measurement
        /// </summary>
        public string Representation
        {
            get
            {
                var sb = new StringBuilder();
                if (Configuration == null)
                {
                    sb.Append("(null)");
                }
                else
                {
                    sb.Append(Configuration.ToString());
                }
                foreach (var m in Measurements)
                {
                    sb.AppendFormat(", {0}={1:#,##0.0#}", m.Key, m.Value);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Creates a new measured configuration
        /// </summary>
        /// <param name="configuration">the configuration that has been measured</param>
        /// <param name="measurements">the actual measured metrics</param>
        public MeasuredConfiguration(T configuration, IDictionary<string, double> measurements) : this()
        {
            Configuration = configuration;
            Measurements = measurements;
        }

        /// <inheritdoc />
        public readonly bool Equals(MeasuredConfiguration<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Configuration, other.Configuration);
        }

        /// <inheritdoc />
        public override readonly bool Equals(object obj)
        {
            if (obj is MeasuredConfiguration<T> configuration)
            {
                return Equals(configuration);
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc />
        public static bool operator ==(MeasuredConfiguration<T> obj1, MeasuredConfiguration<T> obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <inheritdoc />
        public static bool operator !=(MeasuredConfiguration<T> obj1, MeasuredConfiguration<T> obj2)
        {
            return !(obj1 == obj2);
        }

        /// <inheritdoc />
        public override readonly int GetHashCode()
        {
            if (Configuration == null) return 0;
            return Configuration.GetHashCode();
        }
    }
}
