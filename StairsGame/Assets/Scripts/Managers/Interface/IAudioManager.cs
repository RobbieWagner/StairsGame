using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

namespace RobbieWagnerGames.Managers
{
    public interface IAudioManager
    {
        public static void PlayOneShot(EventReference sound, Vector3 worldPosition) =>
            RuntimeManager.PlayOneShot(sound, worldPosition);

        public static EventInstance CreateSoundEventInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            return eventInstance;
        }
    }
}
