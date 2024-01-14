using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;
    [SerializeField] private VideoPlayer videoPlayer;
    [ShowInInspector, ReadOnly] private bool isPlaying = false;

    [BoxGroup("Bottom UI Show")]
    [SerializeField] private GameObject bottomUI;

    [BoxGroup("Bottom UI Show"), ReadOnly]
    [SerializeField] private bool isBottomUIVisible = false;

    [BoxGroup("Bottom UI Show"), ReadOnly]
    [SerializeField] private float bottomUIHideTime = 0f;

    [BoxGroup("Bottom UI Show")]
    [SerializeField] private float bottomUIHideTimeMaximun = 3f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.PauseOrPlayButtonPressed += PauseOrPlay;
        PointerMove.PointerMoveEvent += OnPointerMove;

        bottomUI.SetActive(false);
    }

    private void Update()
    {
        if (isBottomUIVisible && !InBottomUI.isInBottomUIZone)
        {
            bottomUIHideTime -= Time.deltaTime;
            if (bottomUIHideTime <= 0)
            {
                isBottomUIVisible = false;
                StartCoroutine(HideBottomUI());
            }
        }
    }

    private void PauseOrPlay()
    {
        if (InBottomUI.isInBottomUIZone)
            if (isPlaying)
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

    public void OnPointerMove()
    {
        UnityEngine.Cursor.visible = true;
        bottomUI.SetActive(true);
        bottomUI.GetComponent<CanvasGroup>().alpha = 0.97f;
        isBottomUIVisible = true;
        bottomUIHideTime = bottomUIHideTimeMaximun;
    }

    private IEnumerator HideBottomUI()
    {
        float gap = Time.deltaTime;
        UnityEngine.Cursor.visible = false;
        bool done = false;
        WaitForSeconds wait = new WaitForSeconds(gap);
        while (!isBottomUIVisible && !done)
        {
            bottomUI.GetComponent<CanvasGroup>().alpha -= gap;
            if (bottomUI.GetComponent<CanvasGroup>().alpha <= 0.03f)
            {
                bottomUI.SetActive(false);
                done = true;
            }
            yield return wait;
        }
    }
}