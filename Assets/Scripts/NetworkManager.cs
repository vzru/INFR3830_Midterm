using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject paddle1;
    public GameObject paddle2;

    //    ///////////////////////////////////////////////////////////////////////////////
    //    // DLL IMPORT
    //    ///////////////////////////////////////////////////////////////////////////////
    //
    //    const string DLL_NAME = "Network_Plugin";
    //    //net code
    //    [DllImport(DLL_NAME)]
    //    static extern IntPtr CreateClient();                                            //Creates a client
    //    [DllImport(DLL_NAME)]
    //    static extern void DeleteClient(IntPtr client);                                 //Destroys a client
    //    [DllImport(DLL_NAME)]
    //    static extern bool Connect(string ipAddr, IntPtr client);                          //Connects to c++ Server
    //    [DllImport(DLL_NAME)]
    //    static extern void SetupPacketReception(Action<IntPtr, int, bool> action);
    //    [DllImport(DLL_NAME)]
    //    static extern void StartUpdating(IntPtr client);                                //Starts updating
    //    [DllImport(DLL_NAME)]
    //    static extern bool SendDataPacket(IntPtr ptr, int length, bool TCP, IntPtr client);
    //    [DllImport(DLL_NAME)]
    //    static extern bool SendDebugOutput(string data);
    //    [DllImport(DLL_NAME)]
    //    static extern void ShowConsole(IntPtr client, bool open);
    //    [DllImport(DLL_NAME)]
    //    static extern int GetError(IntPtr client);
    //    [DllImport(DLL_NAME)]
    //    static extern int GetErrorLoc(IntPtr client);
    //
    //    public static string ip = "127.0.0.1";
    //    private static IntPtr Client;
    //    //private static int playerNumber = -1;
    //    //private static bool endGame = false;
    //    //public static GameState gameState = GameState.LOBBY;
    //    static public bool isConnected = false;
    //
    //    ///////////////////////////////////////////////////////////////////////////////
    //    // SEND PACKET
    //    ///////////////////////////////////////////////////////////////////////////////
    //
    //    public static void SendPacketBuild(Vector3 pos, int entityLife)
    //    {
    //        Debug.Log("BUILDING TYPE: " + (EntityType)type);
    //            int loc = InitialOffset;
    //            int Receiver = 0;
    //
    //            //Receiver = ~(((int)PlayerMask.SERVER) + (1 << (GameSceneController.Instance.playerNumber + 1)));
    //            Receiver = ~(1 << (GameSceneController.Instance.playerNumber + 1));
    //
    //            PackData(ref sendByteArray, ref loc, pos.x);
    //            PackData(ref sendByteArray, ref loc, pos.y);
    //            PackData(ref sendByteArray, ref loc, pos.z);
    //
    //            SendIntPtr(ref sendByteArray, loc, true, Receiver, (int)PacketType.BUILD);
    //    }
    //
    //    ///////////////////////////////////////////////////////////////////////////////
    //    // RECEIVE PACKET
    //    ///////////////////////////////////////////////////////////////////////////////
    //
    //    static void receivePacket(IntPtr ptr, int length, bool TCP)
    //    {
    //        if (length < 5000 && length >= InitialOffset)
    //        {
    //
    //            //SendDebugOutput("C# RECEIVED PACKET");
    //            if (TCP)
    //            {
    //                Marshal.Copy(ptr, tcpByteArray, 0, length);
    //                //SendDebugOutput("C# DECODED TCP PACKET");
    //                deconstructPacket(ref tcpByteArray, length);
    //            }
    //            else
    //            {
    //                Marshal.Copy(ptr, udpByteArray, 0, length);
    //                //SendDebugOutput("C# DECODED UDP PACKET");
    //                deconstructPacket(ref udpByteArray, length);
    //            }
    //        }
    //        else
    //        {
    //            SendDebugOutput("Invalid Packet Received");
    //        }
    //    }
    //
}
    //// Update is called once per frame
    //private void Update()
    //{
    //    if (connected)
    //    {
    //        connectionStatus.text = "Connection Status: Connected";
    //        sendButton.interactable = true;
    //        rtsButton.interactable = true;
    //        fpsButton.interactable = true;
    //    }
    //    else
    //    {
    //        connectionStatus.text = "Connection Status: Unable to Connect";
    //        sendButton.interactable = false;
    //        rtsButton.interactable = false;
    //        fpsButton.interactable = false;
    //        readyButton.interactable = false;
    //    }
    //    if (loadGame)
    //    {
    //        loadGame = false;
    //        GameSceneController.Instance.SwapScene(2);
    //    }
    //    if (countdown)
    //    {
    //        rtsButton.interactable = false;
    //        fpsButton.interactable = false;
    //    }
    //    else
    //    {
    //        rtsButton.interactable = true;
    //        fpsButton.interactable = true;
    //    }
    //
    //    while (queueMessages.Count > 0)
    //    {
    //        chatLog.text += queueMessages.Dequeue();
    //    }
    //
    //    readyStatus.text = "Status: Role " + GameSceneController.Instance.type.ToString();
    //    if (GameSceneController.Instance.type == PlayerType.Spectator)
    //    {
    //        readyButton.interactable = false;
    //        Ready = false;
    //    }
    //    else
    //    {
    //        readyButton.interactable = true;
    //    }
    //
    //    //countdown
    //    if (countdown)
    //    {
    //        countdownSeconds -= Time.deltaTime;
    //        if (countdownSeconds < 0.0f) { countdownSeconds = 0.0f; }
    //
    //        readyStatus.text = "Status: Start in " + ((int)countdownSeconds).ToString() + " Sec";
    //    }
    //
    //    //update roles
    //    if (rolesUpdated)
    //    {
    //        for (int counter = 0; counter < NetworkManager.allUsers.Count; counter++)
    //        {
    //            StringBuilder output = new StringBuilder();
    //
    //            output.Append(NetworkManager.allUsers[counter].username);
    //            output.Append(" - ");
    //            if (NetworkManager.allUsers[counter].type == PlayerType.FPS) { output.Append("FPS - "); }
    //            else if (NetworkManager.allUsers[counter].type == PlayerType.RTS) { output.Append("RTS - "); }
    //            else { output.Append("Spectator - "); }
    //
    //            if (NetworkManager.allUsers[counter].readyStatus)
    //            {
    //                output.Append("Ready");
    //            }
    //            else
    //            {
    //                output.Append("Not Ready");
    //            }
    //
    //            //set as slot
    //            if (slots.Length > counter)
    //            {
    //                slots[counter].text = output.ToString();
    //                slots[counter].gameObject.SetActive(true);
    //            }
    //            else
    //            {
    //                Debug.LogWarning("More than allowed slots of players tried to connect");
    //            }
    //        }
    //        OnRoleUpdate(false);
    //    }
    //}
    //
    //public static void ConnectToServer(string ipAddr, string username = "Nameless")
    //{
    //    if (ipAddr != "")
    //    {
    //        ip = ipAddr;
    //    }
    //    // Client Connections
    //    if (!Connect(ip, Client))
    //    {
    //        Debug.Log("Error Loc: " + GetErrorLoc(Client).ToString() + " , Error: " + GetError(Client).ToString());
    //        OnConnected(false);
    //    }
    //}
    //
    //public void OnConnected(bool success)
    //{
    //    if (success)
    //    {
    //        connected = true;
    //
    //    }
    //    else
    //    {
    //        connected = false;
    //    }
    //    OnRoleUpdate(true);
    //}
    //
    //public void LoadGame()
    //{
    //    loadGame = true;
    //}
    //
    //public static void OnConnected(bool success)
    //{
    //    StartManager.Instance.OnConnected(success);
    //}
