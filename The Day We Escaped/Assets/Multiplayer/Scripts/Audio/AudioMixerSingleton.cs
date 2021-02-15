using UnityEngine;
using UnityEngine.Audio;

namespace Multiplayer.Scripts.Audio
{
    public class AudioMixerSingleton : BoltSingletonPrefab<AudioMixerSingleton>
    {
        private const float _LowerVolumeBound = -80.0f;
        private const float _UpperVolumeBound = 0.0f;

        private const string _MasterVolumeParameter = "MasterVolume";
        private const string _MusicVolumeParameter = "SfxVolume";
        private const string _SfxVolumeParameter = "MusicVolume";

        [SerializeField] private AudioMixer _mixer;

        private float _lastSfxVolume;

        public void MuteSfx()
        {
            _mixer.GetFloat(_SfxVolumeParameter, out _lastSfxVolume);
            _mixer.SetFloat(_SfxVolumeParameter, _LowerVolumeBound);
        }
        
        public void UnMuteSfx()
        {
            _mixer.SetFloat(_SfxVolumeParameter, _lastSfxVolume);
        }
    }
}