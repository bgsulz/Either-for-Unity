using System;
using UnityEngine;

namespace Extra.Either
{
    /// <summary>
    /// A Serializable, Inspector-friendly class that stores values of multiple types.
    /// </summary>
    [Serializable]
    public abstract class Either
    {
        /// <summary>
        /// Determines selected type.
        /// </summary>
        [SerializeField] private int index;

        /// <summary>
        /// Determines selected type. Automatically clamps from 0 to TypeCount.
        /// </summary>
        public int Index
        {
            get => Mathf.Clamp(index, 0, TypeCount);
            set => index = value;
        }

        /// <summary>
        /// How many types this object stores.
        /// </summary>
        public abstract int TypeCount { get; }

        /// <summary>
        /// The name of each type that this object stores, for use in this object's property drawer.
        /// </summary>
        public abstract string[] TypeNames { get; }

        /// <summary>
        /// Retrieves a stored value by type.
        /// e.g. calling As&#60;float&#62; on an Either&#60;int, float&#62; will return its float value.
        /// </summary>
        /// <typeparam name="T">The type to locate.</typeparam>
        /// <returns>The first stored value of the given type.</returns>
        public abstract T GetStoredValueOfType<T>();

        /// <summary>
        /// Operates on the currently selected value as an interface.
        /// Should be used if all types implement a common interface.
        /// </summary>
        /// <param name="action">The operation to execute.</param>
        /// <typeparam name="TI">The interface as which to treat the selected value.</typeparam>
        public abstract void DoAs<TI>(Action<TI> action);

        protected void TryOperate<T, TI>(T input, int tryIndex, Action<TI> action, bool isInterface = false)
        {
            if (input is TI ti) action(ti);
            else
            {
                throw isInterface
                    ? EitherNotInterfaceType(tryIndex, typeof(TI).Name)
                    : EitherNotConvertibleType(tryIndex, typeof(TI).Name);
            }
        }

        protected void TryAssign<T, TI>(T input, int tryIndex, out TI toAssign, bool isInterface = false)
        {
            if (input is TI ti) toAssign = ti;
            else
            {
                throw isInterface
                    ? EitherNotInterfaceType(tryIndex, typeof(TI).Name)
                    : EitherNotConvertibleType(tryIndex, typeof(TI).Name);
            }
        }

        /// <summary>
        /// Treats the selected value as an interface and converts it to a value of type T.
        /// Should be used if all types implement a common interface.
        /// </summary>
        /// <param name="func">The conversion function.</param>
        /// <typeparam name="TI">The interface as which to treat the selected value.</typeparam>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The selected value converted to type T.</returns>
        public abstract T MatchAs<TI, T>(Func<TI, T> func);

        public abstract T ValueAs<T>();

        protected static ArgumentOutOfRangeException EitherIndexOutOfRange(int index, int count) =>
            new(nameof(index), $"Index {index} is out of range; this Either only has {count} types.");

        protected static InvalidCastException EitherNotInterfaceType(int index, string interfaceName) =>
            new($"Value of index {index} does not implement {interfaceName}.");

        protected static InvalidCastException EitherNotConvertibleType(int index, string typeName) =>
            new($"Value of index {index} is not convertible to type {typeName}.");

        protected static InvalidCastException EitherNoneOfTypeFound(string typeName) =>
            new($"No value is of type {typeName}.");

        protected Either(int index = 0) => Index = index;
    }
}