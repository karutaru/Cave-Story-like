using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class SceneLoadingScript : EventBase
{
    [Header("‹N“®‚µ‚½Žž‚ÌSE")]
    public AudioClip isOnSE;

    AudioSource audioSource;

    public string loadingBefore;
    public string loadingAfter;

    private void Start()
    {
        if (!TryGetComponent(out audioSource)) return;
    }


    public override void SceneLoadingObject()
    {
        isEventPlay = false;
        MMSceneLoadingManager.LoadScene(loadingBefore, loadingAfter);
    }
}
