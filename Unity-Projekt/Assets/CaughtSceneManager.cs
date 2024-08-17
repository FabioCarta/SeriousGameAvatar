using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaughtSceneManager : MonoBehaviour
{
    [SerializeField] CanvasGroup disappointedAvatar;
    private NarratorAudioManager narratorAudioManager;

    private void Start()
    {
        narratorAudioManager = NarratorAudioManager.instance;
        disappointedAvatar.alpha = 0f;
    }

    private void Update()
    {
        if (narratorAudioManager != null)
        {
            if (narratorAudioManager.audioSource.isPlaying)
            {
                disappointedAvatar.alpha = 1f;
                Debug.Log("Playing");
            }
            else
            {
                disappointedAvatar.alpha = 0f;
                Debug.Log("Not Playing");
            }
        }
    }

}
