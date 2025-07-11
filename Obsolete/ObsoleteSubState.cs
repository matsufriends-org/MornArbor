﻿using System;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using MornEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornArbor
{
    [Obsolete("SubStateへ移行")]
    internal sealed class ObsoleteSubState : SubBase
    {
        [SerializeField] private ArborFSMInternal _prefab;
        [SerializeField] private bool _instantiate;
        [SerializeField, ShowIf(nameof(_instantiate))] private Transform _parent;
        [Inject] private IObjectResolver _resolver;
        private ArborFSMInternal _instance;

        public override void OnStateAwake()
        {
            base.OnStateAwake();
            if (_instantiate == false && _prefab != null)
            {
                _prefab.enabled = false;
            }
        }

        [Button("Linkクリア")]
        public void Clear()
        {
            var list = new List<ExitCode>();
            SetExitCodeLinks(list);
        }

        [Button("Link再読み込み")]
        public void Reload()
        {
            if (_prefab != null)
            {
                var list = new List<ExitCode>();
                foreach (var subState in _prefab.GetComponents<SubStateExitState>())
                {
                    list.Add(subState.ExitCode);
                }

                SetExitCodeLinks(list);
            }
        }

        protected override IEnumerator Load()
        {
            if (_instance != null)
            {
                Debug.LogError("SubState is already loaded.");
                yield break;
            }

            _instance = _instantiate ? _resolver.Instantiate(_prefab, _parent) : _prefab;
            _instance.enabled = true;
            _instance.Transition(_instance.startStateID, TransitionTiming.Immediate);
            var provider = _instance.gameObject.GetComponent<SubStateExitCodeProvider>()
                           ?? _instance.gameObject.AddComponent<SubStateExitCodeProvider>();
            provider.OnUpdateOnce += Callback;
        }

        private void Callback(ExitCode exitCode)
        {
            TransitionByExitCode(exitCode);
        }

        public override void OnStateEnd()
        {
            if (_instance != null)
            {
                _instance.enabled = false;
                if (_instantiate)
                {
                    Destroy(_instance.gameObject);
                }

                _instance = null;
            }
        }
    }
}