using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MateSceneManager : MonoBehaviour
{
    [SerializeField] CanvasGroup happyAvatar;
    private NarratorAudioManager narratorAudioManager;

    private void Start()
    {
        narratorAudioManager = NarratorAudioManager.instance;
        happyAvatar.alpha = 0f;
    }

    private void Update()
    {
        if (narratorAudioManager != null)
        {
            if (narratorAudioManager.audioSource.isPlaying)
            {
                ShowImage(happyAvatar);
            }
        }
    }


    private void ShowImage(CanvasGroup happyAvatar)
    {
        if (happyAvatar.alpha == 0f)
        {
            StartCoroutine(FadeInImage(happyAvatar, 1f));
        }
    }

    private IEnumerator FadeInImage(CanvasGroup happyAvatar, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            happyAvatar.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }
        happyAvatar.alpha = 1f;
    }

}
