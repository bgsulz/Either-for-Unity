## Player Stats
Share stats between GameObjects with an atomic ScriptableObject, or quickly try out a new constant value.

_Inspired by Ryan Hipple's FloatReference class._

```cs
using UnityEngine;
using Extra.Either;

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
![Either Demo Screenshot](https://user-images.githubusercontent.com/38191432/159398887-358422b2-47e8-4d67-ad8c-7b65c9443696.png)

## Sound Manager
Allow for a rapid choice between a simple clip, or an extended "Sound" class, or even an atomic ScriptableObject.

Notice: _It's easy to inherit Either to simplify data types!_

```cs
using System;
using UnityEngine;
using Extra.Either;

public class AudioManager : MonoBehaviour
{
    [Serializable] public class SoundData : Either<AudioClip, Sound, SoundScriptableObject> 
    {
        public void Play(AudioSource source)
        {
            Do(
                clip => source.clip = clip,
                sound => ConfigureSource(source, sound),
                soundSO => ConfigureSource(source, soundSO.Value)
            );

            source.Play();
        }

        public static void ConfigureSource(AudioSource source, Sound sound)
        {
            source.clip = sound.Clip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
        }   
    }

    [SerializeField] private SoundData coinSound;

    private void Start() => coinSound.Play(GetComponent<AudioSource>());
}
```
![Either Sound Demo Screenshot](https://user-images.githubusercontent.com/38191432/159398910-f0681ef7-2a6f-4124-b3dd-411d41913759.png)

## Optional Object Pool
Effortlessly choose between plain instantiation or taking from a pool. Don't make a whole new pool just to test out a Prefab.

Notice: _Look at the Instantiate method of Spawnable. Delegate responsibility to your Either-derived class for easier usage._

```cs
using UnityEngine;
using Extra.Either;

[Serializable] 
public class Spawnable : Either<GameObject, PoolableBehaviour> 
{
    public void Instantiate(Vector3 position = default, Quaternion rotation = default)
    {
        if (rotation == default) rotation = Quaternion.identity;

        Do(
            go => Object.Instantiate(go, position, rotation)
            poolable => PoolManager.InstantiateOfType(poolable, position, rotation)
        );
    }
}

//

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private Spawnable hitParticles;

    public void Spawn()
    {
        hitParticles.Instantiate(transform.position);
    }
}

```

## Generic Pseudo-Varargs

The `Optional<T>` utility type allows for pseudo-optional parameters as generic arguments. Use the `None.Instance` property to simulate passing in no argument.

```cs
using Extra.Either;
using UnityEngine;

public class SpawnParticleEvent : ScriptableObjectEvent<ParticleSystem, Optional<Vector3>> { }

public class DamageController : MonoBehaviour
{
    [SerializeField] private SpawnParticleEvent channel;
    [SerializeField] private ParticleSystem hurtParticles, screenParticles;

    public void Damage()
    {
        channel.Invoke(hurtParticles, transform.position);
        channel.Invoke(screenParticles, None.Instance);
    }
}

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private SpawnParticleEvent channel;

    private void OnEnable() => channel.AddListener(SpawnParticles);
    private void OnDisable() => channel.RemoveListener(SpawnParticles);

    private void SpawnParticles(ParticleSystem particles, Optional<Vector3> maybePosition)
    {
        var position = maybePosition.Match(
            v3 => v3,
            none => Vector3.zero
        );

        Instantiate(particles, position, Quaternion.identity);
    }
}
```
