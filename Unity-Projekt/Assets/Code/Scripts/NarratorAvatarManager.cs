using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarratorAvatarManager : MonoBehaviour
{
    [System.Serializable]
    public struct ClipImagePair
    {
        public AudioClip audioClip;
        public CanvasGroup canvasGroup;
    }

    [SerializeField] List<ClipImagePair> clipImagePairs; // list of audioclip and image pairs
    [SerializeField] CanvasGroup defaultCanvasGroup; // default narrator image

    private NarratorAudioManager narratorAudioManager;
    private CanvasGroup currentCanvasGroup; // currently visible image
    private bool isFading = false;

    private void Start()
    {
        // grab NarratorAudioManager instance
        narratorAudioManager = NarratorAudioManager.instance;

        // hide all narrator images at the start
        foreach (var pair in clipImagePairs)
        {
            pair.canvasGroup.alpha = 0f;
        }

        // make sure default narrator image is hidden
        if (defaultCanvasGroup != null)
        {
            defaultCanvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        if (narratorAudioManager != null)
        {
            if (narratorAudioManager.audioSource.isPlaying)
            {
                var currentClip = narratorAudioManager.audioSource.clip;
                var targetPair = clipImagePairs.Find(pair => pair.audioClip == currentClip);

                // if audio clip is found in list -> show corresponding image
                if (targetPair.canvasGroup != null)
                {
                    ShowImage(targetPair.canvasGroup);
                }
                // if audioclip is not found in list -> show default narrator image
                else if (defaultCanvasGroup != null)
                {
                    ShowImage(defaultCanvasGroup);
                }
            }
            else if (!isFading && currentCanvasGroup != null)
            {
                // if audio clip is done playing -> fade out current image
                StartCoroutine(FadeOutImage(currentCanvasGroup, 1f));
                isFading = true;
            }
        }
    }

    private void ShowImage(CanvasGroup targetCanvasGroup)
    {
        // prevent null reference
        if (currentCanvasGroup != null && currentCanvasGroup != targetCanvasGroup)
        {
            StartCoroutine(FadeOutImage(currentCanvasGroup, 1f));
        }

        // fade in new narrator image
        if (targetCanvasGroup.alpha == 0f) // fade in only if not already visible
        {
            StartCoroutine(FadeInImage(targetCanvasGroup, 1f));
        }

        currentCanvasGroup = targetCanvasGroup; // update current image reference
        isFading = false; // reset isFading
    }

    private IEnumerator FadeInImage(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // make sure image is visible
    }

    private IEnumerator FadeOutImage(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f; // make sure image is hidden
    }
}
