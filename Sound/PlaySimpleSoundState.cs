﻿using Arbor;
using MornSound;
using UnityEngine;
using VContainer;

namespace MornArbor
{
    public sealed class PlaySimpleSoundState : StateBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        [Inject] private IMornSoundSimple _soundSimple;

        public override void OnStateBegin()
        {
            _soundSimple.Play(_audioClip);
        }
    }
}