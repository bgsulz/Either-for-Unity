using System;
using UnityEngine;

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
}

[Serializable]
public class Either<T0, T1, T2, T3> : Either
{
    public override int TypeCount => 4;
    public override string[] TypeNames => new[] { typeof(T0).Name, typeof(T1).Name, typeof(T2).Name, typeof(T3).Name };

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
}