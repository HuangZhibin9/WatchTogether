using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> CreateRelay(int count)
    {
        try
        {
            Debug.Log("Create a relay contains : " + count + "Players");
            if (count <= 1)
            {
                count = 2;
            }
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(count - 1);
            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(relayCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "udp");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return relayCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelayAsync(string relayCode)
    {
        try
        {
            Debug.Log("Join relay by code: " + relayCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "udp");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}