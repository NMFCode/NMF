using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    internal abstract class LensPut<TBase, TValue>
    {
		public static LensPut<TBase, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, TValue value);

        public abstract LensPut<TBase, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, TValue> : LensPut<TBase, TValue>
    {
        public PersistentLensPut(Action<TBase, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, TValue> Put { get; set; }

        public override LensPut<TBase, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, TValue value)
        {
            Put(arg1, value);
        }
    }

    internal class NonPersistentLens<TBase, TValue> : LensPut<TBase, TValue>
    {
        public Func<TBase, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, TValue value)
        {
            Target.Value = Put(arg1, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, TValue>
    {
		public static LensPut<TBase, T2, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, TValue value);

        public abstract LensPut<TBase, T2, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, TValue> : LensPut<TBase, T2, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, TValue> Put { get; set; }

        public override LensPut<TBase, T2, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, TValue value)
        {
            Put(arg1, arg2, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, TValue> : LensPut<TBase, T2, TValue>
    {
        public Func<TBase, T2, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, TValue value)
        {
            Target.Value = Put(arg1, arg2, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, TValue>
    {
		public static LensPut<TBase, T2, T3, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, TValue value);

        public abstract LensPut<TBase, T2, T3, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, TValue> : LensPut<TBase, T2, T3, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, TValue value)
        {
            Put(arg1, arg2, arg3, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, TValue> : LensPut<TBase, T2, T3, TValue>
    {
        public Func<TBase, T2, T3, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, TValue> : LensPut<TBase, T2, T3, T4, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, TValue> : LensPut<TBase, T2, T3, T4, TValue>
    {
        public Func<TBase, T2, T3, T4, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, TValue> : LensPut<TBase, T2, T3, T4, T5, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, TValue> : LensPut<TBase, T2, T3, T4, T5, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
    internal abstract class LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>
    {
		public static LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> FromLensPutAttribute(LensPutAttribute lensAttribute, MethodInfo method, INotifyReversableValue<TBase> target)
		{
			MethodInfo lensMethod;
			if (!method.IsStatic && lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(TValue) }, out lensMethod) && lensMethod != null && !lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue, TBase>>(lensMethod), target);
                }
			}
            else if (lensAttribute.InitializeProxyMethod(method, new Type[] { typeof(TBase), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(TValue) }, out lensMethod) && lensMethod != null && lensMethod.IsStatic)
            {
                if (lensMethod.ReturnType == typeof(void))
                {
                    return new PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>(ReflectionHelper.CreateDelegate<Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>>(lensMethod));
                }
                else
                {
                    return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>(ReflectionHelper.CreateDelegate<Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue, TBase>>(lensMethod), target);
                }
            }
            else
            {
                throw new InvalidOperationException($"The lens put method for method {method.Name} has the wrong signature.");
            }
		}

		public virtual bool CanApply { get { return true; } }

        public abstract void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, TValue value);

        public abstract LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target);
    }

    internal class PersistentLensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>
    {
        public PersistentLensPut(Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> put)
        {
            Put = put;
        }

        public Action<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> Put { get; set; }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return this;
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, TValue value)
        {
            Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, value);
        }
    }

    internal class NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> : LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>
    {
        public Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue, TBase> Put { get; private set; }

        public INotifyReversableValue<TBase> Target { get; private set; }

        public NonPersistentLens(Func<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue, TBase> put, INotifyReversableValue<TBase> basePut)
        {
            Put = put;
            Target = basePut;
        }

        public override LensPut<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue> ApplyNewTarget(INotifyReversableValue<TBase> target)
        {
            return new NonPersistentLens<TBase, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TValue>(Put, target);
        }

        public override void SetValue(TBase arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, TValue value)
        {
            Target.Value = Put(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, value);
        }

		public override bool CanApply { get { return Target != null && Target.IsReversable; } }
    }
}
