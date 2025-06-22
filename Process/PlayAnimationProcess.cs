﻿using Arbor;
using MornUtil;
using UnityEngine;

namespace MornArbor
{
    public class PlayAnimationProcess : ProcessBase
    {
        [SerializeField] private BindAnimatorClip _bind;
        [SerializeField] private float _duration;
        [SerializeField] private StateLink _onComplete;
        private float _startTime;
        public override float Progress =>
            _bind.Clip ? Mathf.Clamp01((Time.time - _startTime) * _bind.Animator.speed / _bind.Clip.length) : 1f;

        public override void OnStateBegin()
        {
            _bind.Animator.CrossFadeInFixedTime(_bind.Clip.name, _duration);
            _startTime = Time.time;
        }

        public override void OnStateUpdate()
        {
            if (Progress >= 1f)
            {
                Transition(_onComplete);
            }
        }
    }
}