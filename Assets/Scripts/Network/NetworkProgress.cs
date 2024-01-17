using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NetworkProgress : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI hostProgressText;

    [SerializeField] private Button syncWithServerButton;
    [SerializeField] private Button syncOtherClientButton;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject dragSystem;

    public bool started = false;

    public NetworkVariable<float> hostProgress = new NetworkVariable<float>(0);

    public float clientProgress = 0;
    public float offset = 0.3f;

    public override void OnNetworkSpawn()
    {
        syncWithServerButton.onClick.AddListener(() =>
        {
            Debug.Log("Client Ask Sync: " + clientProgress.ToString());
            videoPlayer.time = hostProgress.Value + offset;
            VideoManager.Instance.Play();
        });
        syncOtherClientButton.onClick.AddListener(() =>
        {
            OnAskSyncButtonClick_ClientRpc();
        });

        started = true;
        dragSystem.SetActive(true);
        Debug.Log("Success Spawned!");
    }

    private void UpdateClientProgress()
    {
        if (!started)
        {
            return;
        }
        clientProgress = (float)videoPlayer.time;
        if (IsServer)
        {
            hostProgress.Value = clientProgress;
        }
    }

    private void OnHostProgressChanged(float previousValue, float newValue)
    {
    }

    public void SetHostProgress()
    {
        if (IsServer)
        {
            hostProgress.Value = clientProgress;
        }
    }

    [ClientRpc]
    public void OnAskSyncButtonClick_ClientRpc()
    {
        VideoManager.Instance.Play();
        if (IsHost) return;
        if (videoPlayer.isPrepared && videoPlayer.length > hostProgress.Value)
        {
            videoPlayer.time = hostProgress.Value + offset;
        }
        Debug.Log("Server Ask Sync: " + clientProgress.ToString(".00"));
    }

    public void SetClientProgress(float value)
    {
        clientProgress = value;
        SetHostProgress();
    }

    private void Update()
    {
        UpdateClientProgress();
        hostProgressText.text = VideoManager.TimeToString((int)hostProgress.Value);
    }
}