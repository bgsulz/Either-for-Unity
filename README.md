# Either for Unity
A Serializable union-esque type for Unity.

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

## How do I add this to Unity?
It's easy!

1. Open the Unity Editor. On the top ribbon, click Window > Package Manager.
2. Click the + button in the upper left corner, and click "Add package from git url..."
3. Enter "https://github.com/bgsulz/Either-for-Unity.git"
4. Enjoy!
