using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;
    [SerializeField] private VideoPlayer videoPlayer;
    [ReadOnly] public bool videoClipPrepared = false;
    [ShowInInspector, ReadOnly] public bool isPlaying = false;

    [BoxGroup("Bottom UI Show")]
    [SerializeField] private GameObject bottomUI;

    [BoxGroup("Bottom UI Show"), ReadOnly]
    [SerializeField] private bool isBottomUIVisible = false;

    [BoxGroup("Bottom UI Show"), ReadOnly]
    [SerializeField] private float bottomUIHideTime = 0f;

    [BoxGroup("Bottom UI Show")]
    [SerializeField] private float bottomUIHideTimeMaximun = 3f;

    [BoxGroup("PlayButton")]
    [SerializeField] private Button playButton;

    [BoxGroup("PlayButton")]
    [SerializeField] private Button pauseButton;

    [BoxGroup("ProgressBar")]
    [SerializeField] private Slider progressBar;

    [BoxGroup("ProgressBar")]
    [SerializeField] private bool isDrag = false;

    [BoxGroup("ProgressTime")]
    [SerializeField] private TextMeshProUGUI timeText;

    [BoxGroup("ProgressTime")]
    [SerializeField, ReadOnly] private string videoMaxTime;

    [BoxGroup("Volume")]
    private float preVolume;

    [BoxGroup("Volume")]
    [SerializeField] private Button muteButton;

    [BoxGroup("Volume")]
    [SerializeField] private Button unmuteButton;

    [BoxGroup("Volume")]
    [SerializeField] private Slider volumeBar;

    [BoxGroup("Volume")]
    [SerializeField] private TextMeshProUGUI volumeText;

    [BoxGroup("Volume")]
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        ResetProgress();

        ReadVolume();
    }

    private void Start()
    {
        InputManager.Instance.PauseOrPlayButtonPressed += PauseOrPlay;
        InputManager.Instance.PauseOrPlayButtonPressed += MouseButtonPressed;
        InputManager.Instance.PauseOrPlayButtonReleased += MouseButtonReleased;
        PointerMove.PointerMoveEvent += OnPointerMove;

        progressBar.onValueChanged.AddListener(OnProgressBarChanged);
        volumeBar.onValueChanged.AddListener(SetVolume);
        playButton.onClick.AddListener(Play);
        pauseButton.onClick.AddListener(Pause);
        muteButton.onClick.AddListener(() =>
        {
            preVolume = audioSource.volume;
            audioSource.volume = 0;
            volumeBar.value = 0;
            volumeText.text = "0";
            unmuteButton.gameObject.SetActive(true);
            muteButton.gameObject.SetActive(false);
        });
        unmuteButton.onClick.AddListener(() =>
        {
            audioSource.volume = preVolume;
            volumeBar.value = preVolume;
            volumeText.text = ((int)(preVolume * 100)).ToString();
            muteButton.gameObject.SetActive(true);
            unmuteButton.gameObject.SetActive(false);
        });
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

    private void PauseOrPlay(InputAction.CallbackContext context)
    {
        if (context.control.path == "/Mouse/leftButton" && InBottomUI.isInBottomUIZone)
        {
            Debug.Log("Return");
            return;
        }
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

    [Button]
    public void OnVideoPrepared()
    {
        videoMaxTime = TimeToString((int)videoPlayer.length);
        videoClipPrepared = true;
        StartCoroutine(UpdateProgressText());
        StartCoroutine(UpdateProgressBar());
        Play();
    }

    public void Play()
    {
        videoPlayer.Play();
        isPlaying = true;
    }

    public void Pause()
    {
        videoPlayer.Pause();
        isPlaying = false;
    }

    public void OnPointerMove()
    {
        UnityEngine.Cursor.visible = true;
        bottomUI.SetActive(true);
        bottomUI.GetComponent<CanvasGroup>().alpha = 0.97f;
        isBottomUIVisible = true;
        bottomUIHideTime = bottomUIHideTimeMaximun;
    }

    public void OnProgressBarChanged(float value)
    {
        videoPlayer.time = value * videoPlayer.length;
    }

    public void MouseButtonPressed(InputAction.CallbackContext context)
    {
        isDrag = true;
    }

    public void MouseButtonReleased()
    {
        isDrag = false;
    }

    public void ResetProgress()
    {
        timeText.text = "00:00:00 / 00:00:00";
        progressBar.value = 0;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
        volumeText.text = ((int)(value * 100)).ToString();
        SaveVolume();
    }

    public void ReadVolume()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            float value = PlayerPrefs.GetFloat("volume");
            audioSource.volume = value;
            volumeBar.value = value;
            volumeText.text = ((int)(value * 100)).ToString();
        }
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", audioSource.volume);
        PlayerPrefs.Save();
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

    private IEnumerator UpdateProgressBar()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (videoClipPrepared)
        {
            if (!isDrag)
            {
                progressBar.SetValueWithoutNotify((float)(videoPlayer.time / videoPlayer.length));
            }
            yield return wait;
        }
    }

    private IEnumerator UpdateProgressText()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (videoClipPrepared)
        {
            if (!isDrag)
            {
                string currentTime = TimeToString((int)videoPlayer.time);
                timeText.text = currentTime + " / " + videoMaxTime;
            }
            yield return wait;
        }
    }

    public static string TimeToString(int time)
    {
        int currentTime = time;
        int hour = Mathf.FloorToInt(currentTime / 3600f);
        currentTime = currentTime - hour * 3600;
        int minute = Mathf.FloorToInt(currentTime / 60f);
        currentTime = currentTime - minute * 60;
        int second = currentTime;

        string s = hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
        return s;
    }
}