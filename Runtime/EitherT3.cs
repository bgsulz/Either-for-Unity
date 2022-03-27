using System;
using UnityEngine;

namespace Extra.Either
{
    [Serializable]
    public class Either<T0, T1, T2, T3> : Either
    {
        public override int TypeCount => 4;

        public override string[] TypeNames =>
            new[] { typeof(T0).Name, typeof(T1).Name, typeof(T2).Name, typeof(T3).Name };

        [SerializeField] private T0 asT0;
        /// <summary>
        /// The stored value of the first type in this object's generic type signature.
        /// </summary>
        public T0 AsT0
        {
            get => asT0;
            private set => asT0 = value;
        }

        [SerializeField] private T1 asT1;
        /// <summary>
        /// The stored value of the second type in this object's generic type signature.
        /// </summary>
        public T1 AsT1
        {
            get => asT1;
            private set => asT1 = value;
        }

        [SerializeField] private T2 asT2;
        /// <summary>
        /// The stored value of the third type in this object's generic type signature.
        /// </summary>
        public T2 AsT2
        {
            get => asT2;
            private set => asT2 = value;
        }

        [SerializeField] private T3 asT3;
        /// <summary>
        /// The stored value of the fourth type in this object's generic type signature.
        /// </summary>
        public T3 AsT3
        {
            get => asT3;
            private set => asT3 = value;
        }

        public Either(int index = 0, T0 asT0 = default, T1 asT1 = default, T2 asT2 = default, T3 asT3 = default) :
            base(index) =>
            (AsT0, AsT1, AsT2, AsT3) =
            (asT0, asT1, asT2, asT3);

        public override T GetStoredValueOfType<T>()
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

            throw EitherNoneOfTypeFound(typeof(T).Name);
        }

        /// <summary>
        /// Tries to set the selected value.
        /// <bold>Passing in null instead of an action resolves to simple assignment.</bold>
        /// </summary>
        /// <param name="value">The value to be assigned to this object's selected value.</param>
        /// <param name="ifT0">Describes how to set an object of the first type to a value of type T.</param>
        /// <param name="ifT1">Describes how to set an object of the second type to a value of type T.</param>
        /// <param name="ifT2">Describes how to set an object of the third type to a value of type T.</param>
        /// <param name="ifT3">Describes how to set an object of the fourth type to a value of type T.</param>
        /// <typeparam name="T">The type of the value to be assigned.</typeparam>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual void Set<T>(T value, Action<T0, T> ifT0, Action<T1, T> ifT1, Action<T2, T> ifT2, Action<T3, T> ifT3)
        {
            switch (Index)
            {
                case 0:
                    if (ifT0 == null) TryAssign(value, Index, out asT0);
                    else ifT0(AsT0, value);
                    break;
                case 1:
                    if (ifT1 == null) TryAssign(value, Index, out asT1);
                    else ifT1(AsT1, value);
                    break;
                case 2:
                    if (ifT2 == null) TryAssign(value, Index, out asT2);
                    else ifT2(AsT2, value);
                    break;
                case 3:
                    if (ifT3 == null) TryAssign(value, Index, out asT3);
                    else ifT3(AsT3, value);
                    break;
                default:
                    throw EitherIndexOutOfRange(Index, TypeCount);
            }
        }

        /// <summary>
        /// Operates on the selected value.
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
                    throw EitherIndexOutOfRange(Index, TypeCount);
            }
        }

        public override void DoAs<TI>(Action<TI> action)
        {
            switch (Index)
            {
                case 0:
                    TryOperate(AsT0, Index, action, true);
                    break;
                case 1:
                    TryOperate(AsT1, Index, action, true);
                    break;
                case 2:
                    TryOperate(AsT2, Index, action, true);
                    break;
                case 3:
                    TryOperate(AsT3, Index, action, true);
                    break;
                default: throw EitherIndexOutOfRange(Index, TypeCount);
            }
        }

        /// <summary>
        /// Converts the selected value to a value of type T.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, converts that value to type T.</param>
        /// <param name="ifT1">If the second type is selected, converts that value to type T.</param>
        /// <param name="ifT2">If the third type is selected, converts that value to type T.</param>
        /// <param name="ifT3">If the fourth type is selected, converts that value to type T.</param>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The selected value converted to type T.</returns>
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

        public override T MatchAs<TI, T>(Func<TI, T> func)
        {
            return Index switch
            {
                0 => AsT0 is TI ti0 ? func(ti0) : throw EitherNotInterfaceType(Index, typeof(TI).Name),
                1 => AsT1 is TI ti1 ? func(ti1) : throw EitherNotInterfaceType(Index, typeof(TI).Name),
                2 => AsT2 is TI ti2 ? func(ti2) : throw EitherNotInterfaceType(Index, typeof(TI).Name),
                3 => AsT3 is TI ti3 ? func(ti3) : throw EitherNotInterfaceType(Index, typeof(TI).Name),
                _ => throw EitherIndexOutOfRange(Index, TypeCount)
            };
        }

        public override T ValueAs<T>()
        {
            return Index switch
            {
                0 => AsT0 is T t0 ? t0 : throw EitherNotConvertibleType(Index, typeof(T).Name),
                1 => AsT1 is T t1 ? t1 : throw EitherNotConvertibleType(Index, typeof(T).Name),
                2 => AsT2 is T t2 ? t2 : throw EitherNotConvertibleType(Index, typeof(T).Name),
                3 => AsT2 is T t3 ? t3 : throw EitherNotConvertibleType(Index, typeof(T).Name),
                _ => throw EitherIndexOutOfRange(Index, TypeCount)
            };
        }

        public override string ToString() =>
            Match(
                t0 => t0.ToString(),
                t1 => t1.ToString(),
                t2 => t2.ToString(),
                t3 => t3.ToString()
            );

        public static implicit operator T0(Either<T0, T1, T2, T3> either) => either.AsT0;
        public static implicit operator T1(Either<T0, T1, T2, T3> either) => either.AsT1;
        public static implicit operator T2(Either<T0, T1, T2, T3> either) => either.AsT2;
        public static implicit operator T3(Either<T0, T1, T2, T3> either) => either.AsT3;

        public static implicit operator Either<T0, T1, T2, T3>(T0 value) => new(0, asT0: value);
        public static implicit operator Either<T0, T1, T2, T3>(T1 value) => new(1, asT1: value);
        public static implicit operator Either<T0, T1, T2, T3>(T2 value) => new(2, asT2: value);
        public static implicit operator Either<T0, T1, T2, T3>(T3 value) => new(3, asT3: value);
    }
}