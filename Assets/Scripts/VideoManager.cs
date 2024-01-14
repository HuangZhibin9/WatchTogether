using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;
    [SerializeField] private VideoPlayer videoPlayer;
    [ShowInInspector, ReadOnly] private bool isPlaying = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.PauseOrPlayButtonPressed += PauseOrPlayer;
    }

    public void PauseOrPlayer()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            isPlaying = false;
        }
        else
        {
            videoPlayer.Play();
            isPlaying = true;
        }
    }
}