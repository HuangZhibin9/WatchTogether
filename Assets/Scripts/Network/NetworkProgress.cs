using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkProgress : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI clientProgressText;
    [SerializeField] private TextMeshProUGUI hostProgressText;

    [SerializeField] private Button syncWithServerButton;
    [SerializeField] private Button syncAllClientButton;

    public bool started = false;

    public NetworkVariable<float> hostProgress = new NetworkVariable<float>(0);

    public float clientProgress = 0;

    public override void OnNetworkSpawn()
    {
        syncWithServerButton.onClick.AddListener(() =>
        {
            Debug.Log("Client Ask Sync: " + clientProgress.ToString());
            TextToServer_ServerRpc();
            clientProgress = hostProgress.Value;
        });
        syncAllClientButton.onClick.AddListener(() =>
        {
            OnHostSyncButtonClick_ClientRpc();
        });

        if (IsServer)
        {
            syncAllClientButton.gameObject.SetActive(true);
            syncWithServerButton.gameObject.SetActive(false);
        }
        else
        {
            syncAllClientButton.gameObject.SetActive(false);
            syncWithServerButton.gameObject.SetActive(true);
        }

        started = true;

        Debug.Log("Success Spawned!");
    }

    private void UpdateClientProgress()
    {
        if (!started)
        {
            return;
        }
        clientProgress += Time.deltaTime;
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
    public void OnHostSyncButtonClick_ClientRpc()
    {
        if (!IsServer)
        {
            clientProgress = hostProgress.Value;
            Debug.Log("Server Ask Sync: " + clientProgress.ToString(".00"));
        }
        Debug.Log("Ask Client Sync: " + clientProgress.ToString(".00"));
    }

    [ServerRpc]
    public void TextToServer_ServerRpc()
    {
        Debug.Log("Client Ask Sync: ");
    }

    public void SetClientProgress(float value)
    {
        clientProgress = value;
        SetHostProgress();
    }

    private void Update()
    {
        UpdateClientProgress();

        clientProgressText.text = "Local Progress: " + clientProgress.ToString("000.00");
        hostProgressText.text = "Host Progress: " + hostProgress.Value.ToString("000.00");
    }
}