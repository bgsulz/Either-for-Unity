## How do I assign a value to an `Either`?

Usually, you can just assign as if it's a normal variable.

```cs
[SerializeField] private Either<float, FloatScriptableObject> defenseStat;

private void Start()
{
    defenseStat = 99;
}
```

The preceding code converts 99 to a new `Either<float, FloatScriptableObject>` and assigns it to `defenseStat.` It also sets its `Index` to 0, changing the selected type to `float`.

Notice that assignment initializes a new object *and* overrides the selected type. In certain cases, this can be a problem.

1. This is a lot slower and takes up a lot more memory than direct mutation.
2. You may want to assign to the `FloatScriptableObject` value when it is the selected type instead of overriding the selected type to `float.`

#### Method 1 (Recommended): Inherit and create setter

If you intend to reuse a specific `Either` type, inheriting into your own type is *highly* recommended.

```cs
[Serializable]
public class Stat : Either<float, FloatValueChip>
{
    public void Set(float input)
    {
        switch (Index)
        {
            case 0:
                AsT0 = input;
                break;
            case 1:
                AsT1.Value = input;
                break;
        }
    }
}

[SerializeField] private Stat defenseStat;

private void Start()
{
    defenseStat.Set(99);
}
```

Inheriting an `Either` type breaks the implicit type conversions; you can add them back manually if needed by [overriding the conversion operators of your new type.](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators) *(More documentation on this coming soon.)*

#### Method 2: Use generic Set method

If, for whatever reason, the inheritance approach isn't a good fit, you can use the included generic `Set` method.

```cs
[SerializeField] private Either<float, FloatScriptableObject> defenseStat;

private void Start()
{
    defenseStat.Set(99f,
        null, // If float is selected
        (so, val) => so.FloatValue = val // If FloatScriptableObject is selected
    );
}
```

This takes in the following parameters:

- The value to be assigned.
- A list of `Actions` describing how to assign that value to each type stored in the `Either` object.
	- *Passing in `null` instead of an action will simply convert the value and assign it directly.*
	
The Action corresponding to the `Either` object's selected type will run. If that Action is `null`, it will attempt to assign the value directly.

This retains the selected type, but it's a real headache. Unless you absolutely cannot do so, inherit and create a custom setter.