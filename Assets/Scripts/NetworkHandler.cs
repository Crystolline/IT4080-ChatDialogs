using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;
    }


    private bool hasPrinted = false;
    private void PrintMe()
    {
        if (hasPrinted)
        {
            return;
        }
        Debug.Log("I AM");
        hasPrinted = true;
        if (IsServer)
        {
            Debug.Log($" the Server! {NetworkManager.ServerClientId}");
        }
        if (IsHost)
        {
            Debug.Log($" the Host! {NetworkManager.ServerClientId}/{NetworkManager.LocalClientId}");
        }
        if (IsClient)
        {
            Debug.Log($" a Client! {NetworkManager.LocalClientId}");
        }
        if (!IsServer && !IsClient)
        {
            Debug.Log(" Nothing yet!");
            hasPrinted = false;
        }
    }

    private void OnClientStarted()
    {
        Debug.Log("!! Client Started !!");
        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDisconnected;
        NetworkManager.OnClientStopped += ClientOnServerStopped;
        PrintMe();
    }

    private void ClientOnClientConnected(ulong clientId)
    {
        PrintMe();
        if (clientId != NetworkManager.LocalClientId)
        {
            Debug.Log($"Some other client with id {clientId} has connected to the server");
        }
        else
        {
            Debug.Log($"I {clientId} have connected to the server");
        }
    }

    private void ClientOnClientDisconnected(ulong clientId)
    {
        if (clientId != NetworkManager.LocalClientId)
        {
            Debug.Log($"Some other client with id {clientId} has disconnected from the server");
        }
        else
        {
            Debug.Log($"I {clientId} have disconnected from the server");
        }
    }

    private void ClientOnServerStopped(bool indicator)
    {
        Debug.Log("!! Client Stopped !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDisconnected;
        NetworkManager.OnServerStopped -= ClientOnServerStopped;
    }

    private void OnServerStarted()
    {
        Debug.Log("!! Server Started !!");
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDisconnected;
        NetworkManager.OnServerStopped += ServerOnServerStopped;
        PrintMe();
    }

    private void ServerOnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected to the server");
    }

    private void ServerOnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected from the server");
    }

    private void ServerOnServerStopped(bool indicator)
    {
        Debug.Log("!! Server Stopped !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ServerOnClientDisconnected;
        NetworkManager.OnServerStopped -= ServerOnServerStopped;
    }
}
