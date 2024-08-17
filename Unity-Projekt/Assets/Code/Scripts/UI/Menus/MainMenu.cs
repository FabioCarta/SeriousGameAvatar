using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image BackGround;
    [SerializeField] Image BlackScreen;


    [SerializeField] AudioClip caughtClip;
    [SerializeField] AudioClip mateClip;
    [SerializeField] AudioClip boneFishClip;
    [SerializeField] CanvasGroup welcomeAvatar;
    [SerializeField] CanvasGroup disappointedAvatar;
    [SerializeField] CanvasGroup happyAvatar;


    private NarratorAudioManager narratorAudioManager;

    private void Start()
    {
        if (welcomeAvatar != null) welcomeAvatar.alpha = 0f;
        if (disappointedAvatar != null) disappointedAvatar.alpha = 0f;
        if (happyAvatar != null) happyAvatar.alpha = 0f;


        narratorAudioManager = NarratorAudioManager.instance;
    }

    private void Update()
    {
        CheckNarratorAudioManagerAndShowImage();
    }

    private void CheckNarratorAudioManagerAndShowImage()
    {
        if (narratorAudioManager != null && narratorAudioManager.audioSource.isPlaying)
        {

            AudioClip currentClip = narratorAudioManager.audioSource.clip;
            Debug.Log(currentClip.name);

            if (currentClip == caughtClip || currentClip == boneFishClip)
            {
                ShowImage(disappointedAvatar);
                HideImage(happyAvatar);
            }
            else if (currentClip == mateClip)
            {
                ShowImage(happyAvatar);
                HideImage(disappointedAvatar);
            }
        }
        else
        {
            HideImage(disappointedAvatar);
            HideImage(happyAvatar);
        }
    }

    private void ShowImage(CanvasGroup currentAvatar)
    {
        currentAvatar.alpha = 1f;

    }

    private void HideImage(CanvasGroup currentAvatar)
    {
        currentAvatar.alpha = 0f;
    }

    public void PlayGame()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        if (NarratorAudioManager.instance.skipIntroBoolean)
        {
            StartCoroutine(BlendInImage(BlackScreen, 3));
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Level_1");
        }
        else
        {
            StartCoroutine(BlendInImage(BackGround, 1));
            yield return new WaitForSeconds(1);

            if (!NarratorAudioManager.instance.startBoolean)
            {
                NarratorAudioManager.instance.skipIntroBoolean = true;
                NarratorAudioManager.instance.startBoolean = true;
                NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.startClip);
                StartCoroutine(FadeInImage(welcomeAvatar, 2f));
            }

            yield return new WaitForSeconds(23);
            StartCoroutine(BlendInImage(BlackScreen, 3));

            RectTransform imageRectTransform = welcomeAvatar.GetComponent<RectTransform>();
            StartCoroutine(AnimateScaleAndPosition(imageRectTransform, new Vector3(0.2f, 0.2f, 0.2f), new Vector2(646, 222), 3f));

            yield return new WaitForSeconds(3f);
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("Level_1");
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator BlendInImage(Image image, int duration)
    {
        int timeSteps = 100;
        timeSteps *= duration;
        image.enabled = true;
        float alphaAdd = ((float)1 / (float)timeSteps);
        Color currentColor = image.color;
        for (int i = 0; i < timeSteps; i++)
        {
            currentColor.a += alphaAdd;
            image.color = currentColor;
            yield return new WaitForSeconds(alphaAdd);
        }
    }

    IEnumerator FadeInImage(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }
    }

    IEnumerator AnimateScaleAndPosition(RectTransform rectTransform, Vector3 targetScale, Vector2 targetPosition, float duration)
    {
        Vector3 initialScale = rectTransform.localScale;
        Vector2 initialPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
        rectTransform.anchoredPosition = targetPosition;
    }
}
