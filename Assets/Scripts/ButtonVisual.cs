using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVisual : MonoBehaviour
{
    [BoxGroup("PlayButton")]
    [SerializeField] private Button playButton;

    [BoxGroup("PlayButton")]
    [SerializeField] private Button pauseButton;

    // Update is called once per frame
    private void Update()
    {
        if (VideoManager.Instance.isPlaying)
        {
            ShowPauseButton();
        }
        else
        {
            ShowPlayButton();
        }
    }

    private void ShowPlayButton()
    {
        playButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    private void ShowPauseButton()
    {
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
}