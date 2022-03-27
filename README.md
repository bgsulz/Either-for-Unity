# Either for Unity
A Serializable union-esque type for Unity.

![Either Demo Screenshot](https://user-images.githubusercontent.com/38191432/159367481-41640d75-8610-41f0-9cec-003de6d0a36f.png)

```cs
using Extra.Either;
using UnityEngine;

public class PlayerDefenseCalculator : MonoBehaviour
{
    [SerializeField] private Either<float, FloatScriptableObject> defenseStat;

    public float EvaluateDamage(float attackPower)
    {
        var defense = defenseStat.Match(
            f => f,
            so => so.FloatValue
        );

        return attackPower - defense;
    }
}
```

| [See Examples](https://github.com/bgsulz/Either-for-Unity/blob/main/EXAMPLES.md) | [Read FAQ](https://github.com/bgsulz/Either-for-Unity/blob/main/FAQ.md) |
|---|---|

## How do I add this to Unity?
It's easy!

1. Open the Unity Editor. On the top ribbon, click Window > Package Manager.
2. Click the + button in the upper left corner, and click "Add package from git url..."
3. Enter "https://github.com/bgsulz/Either-for-Unity.git"
4. Enjoy!
