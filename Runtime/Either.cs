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
        public abstract T As<T>();

        /// <summary>
        /// A helper function for pattern patching.
        /// </summary>
        /// <returns>The input parameter as type T.</returns>
        public static T Self<T0, T>(T0 input) where T0 : T => input;
    }

    [Serializable]
    public class Either<T0, T1> : Either
    {
        public override int TypeCount => 2;
        public override string[] TypeNames => new[] { typeof(T0).Name, typeof(T1).Name };

        /// <summary>
        /// The stored value of the first type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T0 AsT0 { get; private set; }

        /// <summary>
        /// The stored value of the second type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T1 AsT1 { get; private set; }

        public Either(T0 asT0 = default, T1 asT1 = default) =>
            (AsT0, AsT1) =
            (asT0, asT1);

        public override T As<T>()
        {
            if (AsT0 is T t0)
            {
                Index = 0;
                return t0;
            }

            if (AsT1 is T t1)
            {
                Index = 1;
                return t1;
            }

            return default;
        }

        /// <summary>
        /// Operates on the stored value of the currently selected type.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, operates on that value.</param>
        /// <param name="ifT1">If the second type is selected, operates on that value.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Do(Action<T0> ifT0, Action<T1> ifT1)
        {
            switch (Index)
            {
                case 0:
                    ifT0(AsT0);
                    break;
                case 1:
                    ifT1(AsT1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Index {Index} is out of range; this Either only has {TypeCount} types.");
            }
        }

        /// <summary>
        /// Converts the stored value of the currently selected type to a value of type T.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, converts that value to type T.</param>
        /// <param name="ifT1">If the second type is selected, converts that value to type T.</param>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The stored value of the currently selected type converted to type T.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T Match<T>(Func<T0, T> ifT0, Func<T1, T> ifT1)
        {
            return Index switch
            {
                0 => ifT0(AsT0),
                1 => ifT1(AsT1),
                _ => throw new ArgumentOutOfRangeException(
                    $"Index {Index} is out of range; this Either only has {TypeCount} types.")
            };
        }

        public static implicit operator T0(Either<T0, T1> either) => either.AsT0;
        public static implicit operator T1(Either<T0, T1> either) => either.AsT1;
    }

    [Serializable]
    public class Either<T0, T1, T2> : Either
    {
        public override int TypeCount => 3;
        public override string[] TypeNames => new[] { typeof(T0).Name, typeof(T1).Name, typeof(T2).Name };

        /// <summary>
        /// The stored value of the first type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T0 AsT0 { get; private set; }

        /// <summary>
        /// The stored value of the second type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T1 AsT1 { get; private set; }

        /// <summary>
        /// The stored value of the third type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T2 AsT2 { get; private set; }

        public Either(T0 asT0 = default, T1 asT1 = default, T2 asT2 = default) =>
            (AsT0, AsT1, AsT2) =
            (asT0, asT1, asT2);

        public override T As<T>()
        {
            if (AsT0 is T t0)
            {
                Index = 0;
                return t0;
            }

            if (AsT1 is T t1)
            {
                Index = 1;
                return t1;
            }

            if (AsT2 is T t2)
            {
                Index = 2;
                return t2;
            }

            return default;
        }

        /// <summary>
        /// Operates on the stored value of the currently selected type.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, operates on that value.</param>
        /// <param name="ifT1">If the second type is selected, operates on that value.</param>
        /// <param name="ifT2">If the third type is selected, operates on that value.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Do(Action<T0> ifT0, Action<T1> ifT1, Action<T2> ifT2)
        {
            switch (Index)
            {
                case 0:
                    ifT0(AsT0);
                    break;
                case 1:
                    ifT1(AsT1);
                    break;
                case 2:
                    ifT2(AsT2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Index {Index} is out of range; this Either only has {TypeCount} types.");
            }
        }

        /// <summary>
        /// Converts the stored value of the currently selected type to a value of type T.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, converts that value to type T.</param>
        /// <param name="ifT1">If the second type is selected, converts that value to type T.</param>
        /// <param name="ifT2">If the third type is selected, converts that value to type T.</param>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The stored value of the currently selected type converted to type T.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T Match<T>(Func<T0, T> ifT0, Func<T1, T> ifT1, Func<T2, T> ifT2)
        {
            return Index switch
            {
                0 => ifT0(AsT0),
                1 => ifT1(AsT1),
                2 => ifT2(AsT2),
                _ => throw new ArgumentOutOfRangeException(
                    $"Index {Index} is out of range; this Either only has {TypeCount} types.")
            };
        }

        public static implicit operator T0(Either<T0, T1, T2> either) => either.AsT0;
        public static implicit operator T1(Either<T0, T1, T2> either) => either.AsT1;
        public static implicit operator T2(Either<T0, T1, T2> either) => either.AsT2;
    }

    [Serializable]
    public class Either<T0, T1, T2, T3> : Either
    {
        public override int TypeCount => 4;

        public override string[] TypeNames =>
            new[] { typeof(T0).Name, typeof(T1).Name, typeof(T2).Name, typeof(T3).Name };

        /// <summary>
        /// The stored value of the first type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T0 AsT0 { get; private set; }

        /// <summary>
        /// The stored value of the second type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T1 AsT1 { get; private set; }

        /// <summary>
        /// The stored value of the third type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T2 AsT2 { get; private set; }

        /// <summary>
        /// The stored value of the fourth type in this object's generic type signature.
        /// </summary>
        [field: SerializeField]
        public T3 AsT3 { get; private set; }

        public Either(T0 asT0 = default, T1 asT1 = default, T2 asT2 = default, T3 asT3 = default) =>
            (AsT0, AsT1, AsT2, AsT3) =
            (asT0, asT1, asT2, asT3);

        public override T As<T>()
        {
            if (AsT0 is T t0)
            {
                Index = 0;
                return t0;
            }

            if (AsT1 is T t1)
            {
                Index = 1;
                return t1;
            }

            if (AsT2 is T t2)
            {
                Index = 2;
                return t2;
            }

            if (AsT3 is T t3)
            {
                Index = 3;
                return t3;
            }

            return default;
        }

        /// <summary>
        /// Operates on the stored value of the currently selected type.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, operates on that value.</param>
        /// <param name="ifT1">If the second type is selected, operates on that value.</param>
        /// <param name="ifT2">If the third type is selected, operates on that value.</param>
        /// <param name="ifT3">If the fourth type is selected, operates on that value.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Do(Action<T0> ifT0, Action<T1> ifT1, Action<T2> ifT2, Action<T3> ifT3)
        {
            switch (Index)
            {
                case 0:
                    ifT0(AsT0);
                    break;
                case 1:
                    ifT1(AsT1);
                    break;
                case 2:
                    ifT2(AsT2);
                    break;
                case 3:
                    ifT3(AsT3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Index {Index} is out of range; this Either only has {TypeCount} types.");
            }
        }

        /// <summary>
        /// Converts the stored value of the currently selected type to a value of type T.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, converts that value to type T.</param>
        /// <param name="ifT1">If the second type is selected, converts that value to type T.</param>
        /// <param name="ifT2">If the third type is selected, converts that value to type T.</param>
        /// <param name="ifT3">If the fourth type is selected, converts that value to type T.</param>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The stored value of the currently selected type converted to type T.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T Match<T>(Func<T0, T> ifT0, Func<T1, T> ifT1, Func<T2, T> ifT2, Func<T3, T> ifT3)
        {
            return Index switch
            {
                0 => ifT0(AsT0),
                1 => ifT1(AsT1),
                2 => ifT2(AsT2),
                3 => ifT3(AsT3),
                _ => throw new ArgumentOutOfRangeException(
                    $"Index {Index} is out of range; this Either only has {TypeCount} types.")
            };
        }

        public static implicit operator T0(Either<T0, T1, T2, T3> either) => either.AsT0;
        public static implicit operator T1(Either<T0, T1, T2, T3> either) => either.AsT1;
        public static implicit operator T2(Either<T0, T1, T2, T3> either) => either.AsT2;
        public static implicit operator T3(Either<T0, T1, T2, T3> either) => either.AsT3;
    }
}