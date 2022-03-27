using System;
using UnityEngine;

namespace Extra.Either
{
    [Serializable]
    public class Either<T0, T1> : Either
    {
        public override int TypeCount => 2;
        public override string[] TypeNames => new[] { typeof(T0).Name, typeof(T1).Name };

        [SerializeField] private T0 asT0;
        /// <summary>
        /// The stored value of the first type in this object's generic type signature.
        /// </summary>
        public T0 AsT0
        {
            get => asT0;
            protected set => asT0 = value;
        }

        [SerializeField] private T1 asT1;
        /// <summary>
        /// The stored value of the second type in this object's generic type signature.
        /// </summary>
        public T1 AsT1
        {
            get => asT1;
            protected set => asT1 = value;
        }

        public Either(int index = 0, T0 asT0 = default, T1 asT1 = default) : base(index) =>
            (AsT0, AsT1) =
            (asT0, asT1);

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

            throw EitherNoneOfTypeFound(typeof(T).Name);
        }

        /// <summary>
        /// Tries to set the selected value.
        /// <bold>Passing in null instead of an action resolves to simple assignment.</bold>
        /// </summary>
        /// <param name="value">The value to be assigned to this object's selected value.</param>
        /// <param name="ifT0">Describes how to set an object of the first type to a value of type T.</param>
        /// <param name="ifT1">Describes how to set an object of the second type to a value of type T.</param>
        /// <typeparam name="T">The type of the value to be assigned.</typeparam>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual void Set<T>(T value, Action<T0, T> ifT0, Action<T1, T> ifT1)
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
                default:
                    throw EitherIndexOutOfRange(Index, TypeCount);
            }
        }

        /// <summary>
        /// Operates on the selected value.
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
                default: throw EitherIndexOutOfRange(Index, TypeCount);
            }
        }

        /// <summary>
        /// Converts the selected value to a value of type T.
        /// </summary>
        /// <param name="ifT0">If the first type is selected, converts that value to type T.</param>
        /// <param name="ifT1">If the second type is selected, converts that value to type T.</param>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The selected value converted to type T.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T Match<T>(Func<T0, T> ifT0, Func<T1, T> ifT1)
        {
            return Index switch
            {
                0 => ifT0(AsT0),
                1 => ifT1(AsT1),
                _ => throw EitherIndexOutOfRange(Index, TypeCount)
            };
        }

        public override T MatchAs<TI, T>(Func<TI, T> func)
        {
            return Index switch
            {
                0 => AsT0 is TI ti0 ? func(ti0) : throw EitherNotInterfaceType(Index, typeof(TI).Name),
                1 => AsT1 is TI ti1 ? func(ti1) : throw EitherNotInterfaceType(Index, typeof(TI).Name),
                _ => throw EitherIndexOutOfRange(Index, TypeCount)
            };
        }

        public override T ValueAs<T>()
        {
            return Index switch
            {
                0 => AsT0 is T t0 ? t0 : throw EitherNotConvertibleType(Index, typeof(T).Name),
                1 => AsT1 is T t1 ? t1 : throw EitherNotConvertibleType(Index, typeof(T).Name),
                _ => throw EitherIndexOutOfRange(Index, TypeCount)
            };
        }

        public override string ToString() =>
            Match(
                t0 => t0.ToString(),
                t1 => t1.ToString()
            );

        public static implicit operator T0(Either<T0, T1> either) => either.AsT0;
        public static implicit operator T1(Either<T0, T1> either) => either.AsT1;

        public static implicit operator Either<T0, T1>(T0 value) => new(0, asT0: value);
        public static implicit operator Either<T0, T1>(T1 value) => new(1, asT1: value);
    }
}