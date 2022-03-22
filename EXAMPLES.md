## Sound Player
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