## Player Stats
Share stats between GameObjects with an atomic ScriptableObject, or quickly try out a new constant value.

_Inspired by Ryan Hipple's FloatReference class._

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

## Sound Manager
Allow for a rapid choice between a simple clip, or an extended "Sound" class, or even an atomic ScriptableObject.

Notice: _It's easy to inherit Either to simplify data types!_

```cs
using System;
using Extra.Audio;
using Extra.Either;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable] public class SoundData : Either<AudioClip, Sound, SoundScriptableObject> { }

    [SerializeField] private SoundData coinSound;

    private void Start() => Play(coinSound, GetComponent<AudioSource>());

    public static void Play(SoundData soundData, AudioSource source)
    {
        soundData.Do(
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
```
