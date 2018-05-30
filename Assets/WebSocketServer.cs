using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System;

public class WebSocketServer : MonoBehaviour
{
    public ClientManager clients;
    private WebSocket webSocket;

    public void Start()
    {
        StartCoroutine(ConnectToWebSocket());
    }
    
    private IEnumerator ConnectToWebSocket()
    {
        webSocket = new WebSocket("ws://localhost:100");
        webSocket.OnMessage += (sender, e) => InterpretMessage(e.Data);

        webSocket.Connect();
        
        webSocket.Send(JsonConvert.SerializeObject(
            new Dictionary<string, object> { { "connect", "server" } }
        ));

        while (true) {
            webSocket.Send(JsonConvert.SerializeObject(
                new Dictionary<string, object> { { "data", new Dictionary<string, object> { { "clients", clients.GetClients() } } } }
            ));

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDestroy()
    {
        webSocket.Close();
    }

    private void InterpretMessage(string data)
    {
        Debug.Log(data);

        Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        int clientId = Convert.ToInt32(message["client_id"].ToString());
        clients.AddClient(clientId);

        if (message.ContainsKey("client_data")) {
            Dictionary<string, object> clientData = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["client_data"].ToString());
            clients.UpdateClient(clientId, float.Parse(clientData["x"].ToString()), float.Parse(clientData["y"].ToString()));
        }
    }
}
