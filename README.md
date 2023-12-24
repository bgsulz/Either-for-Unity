# Either for Unity
A Serializable union-esque type for Unity.

| [Examples](https://github.com/bgsulz/Either-for-Unity/blob/main/EXAMPLES.md) | [FAQ](https://github.com/bgsulz/Either-for-Unity/blob/main/FAQ.md) |
|---|---|

![Either Demo Screenshot](https://user-images.githubusercontent.com/38191432/159367481-41640d75-8610-41f0-9cec-003de6d0a36f.png)

```cs
using Extra.Either;
using UnityEngine;

public class PlayerDefenseCalculator : MonoBehaviour
{
    [SerializeField] private Either<float, FloatScriptableObject> defenseStat;

    public float EvaluateDamage(float attackPower)
    {
        float defense = defenseStat.Match<float>(
            ifFloat => ifFloat,
            ifSO => ifSO.FloatValue
        );

        return attackPower - defense;
    }
}
```

## How do I add this to Unity?
It's easy!

#### If you have Git...
1. Open the Unity Editor. On the top ribbon, click Window > Package Manager.
2. Click the + button in the upper left corner, and click "Add package from git url..."
3. Enter "https://github.com/bgsulz/Either-for-Unity.git"
4. Enjoy!

#### If you don't have Git (or want to modify the code)...
1. Click the big green "Code" button and click Download ZIP.
2. Extract the contents of the .zip into your Unity project's Assets folder.
3. Enjoy!
