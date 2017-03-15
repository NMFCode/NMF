using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    [DebuggerDisplay("{Representation}")]
    public struct MeasuredConfiguration<T> : IEquatable<MeasuredConfiguration<T>>
    {
        public T Configuration { get; private set; }

        public IDictionary<string, double> Measurements { get; private set; }

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

        public MeasuredConfiguration(T configuration, IDictionary<string, double> measurements) : this()
        {
            Configuration = configuration;
            Measurements = measurements;
        }

        public bool Equals(MeasuredConfiguration<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Configuration, other.Configuration);
        }

        public override bool Equals(object obj)
        {
            if (obj is MeasuredConfiguration<T>)
            {
                return Equals((MeasuredConfiguration<T>)obj);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(MeasuredConfiguration<T> obj1, MeasuredConfiguration<T> obj2)
        {
            if (obj1 == null)
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(MeasuredConfiguration<T> obj1, MeasuredConfiguration<T> obj2)
        {
            return !(obj1 == obj2);
        }

        public override int GetHashCode()
        {
            if (Configuration == null) return 0;
            return Configuration.GetHashCode();
        }
    }
}
