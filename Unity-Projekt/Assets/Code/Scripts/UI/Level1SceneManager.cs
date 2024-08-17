using System.Collections;
using UnityEngine;

public class Level1SceneManager : MonoBehaviour
{
    [SerializeField] CanvasGroup narratorCanvasGroup;

    private void Start()
    {
        Debug.Log("Level1SceneManager Start");
        if (narratorCanvasGroup != null)
        {
            narratorCanvasGroup.alpha = 1f;
        }

        StartCoroutine(FadeOutAfterDelay(3f, 2f));
    }

    private IEnumerator FadeOutAfterDelay(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);
        FadeOutImage(fadeDuration);
    }

    public void FadeOutImage(float duration)
    {
        if (narratorCanvasGroup != null)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }
        else
        {
            Debug.LogError("Narrator CanvasGroup is not assigned in Level1SceneManager.");
        }
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startAlpha = narratorCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            narratorCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            yield return null;
        }

        narratorCanvasGroup.alpha = 0f;
    }
}
