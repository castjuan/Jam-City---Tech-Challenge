using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Class to destroy audios when stopped playing and fade out if necessary

public class AudioInstance : MonoBehaviour
{
    AudioSource source;
    public void Start() {
        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!source.isPlaying) Destroy(gameObject);     
    }

    public void DestroyInstance() => StartCoroutine(DestroySequence(0.25f));

    public void DestroyInstance(float time) => StartCoroutine(DestroySequence(time));

    IEnumerator DestroySequence(float time) {
        int currentLean = LeanTween.value(gameObject, source.volume, 0, time).setOnUpdate(
                (float val) =>
                {
                    source.volume = val;
                }
            ).setEaseOutSine().id;

        while (LeanTween.isTweening(currentLean)) yield return null;

        if (!gameObject)
            yield break;
        else
            source.Stop();
    }
}
