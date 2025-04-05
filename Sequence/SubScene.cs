﻿#if USE_MORN_SCENE
using System.Collections;
using MornScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MornArbor
{
    internal sealed class SubScene : SubBase
    {
        [SerializeField] private MornSceneObject _scene;

        protected override IEnumerator Load()
        {
            yield return SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(_scene);
            var sceneObject = new GameObject(nameof(SubSceneExitCodeProvider));
            var provider = sceneObject.AddComponent<SubSceneExitCodeProvider>();
            SceneManager.MoveGameObjectToScene(sceneObject, scene);
            while (string.IsNullOrEmpty(provider.ExitCode))
            {
                yield return null;
            }

            yield return SceneManager.UnloadSceneAsync(_scene);
            TransitionByExitCode(provider.ExitCode);
        }
    }
}
#endif