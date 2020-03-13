using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;

enum PacketType
{
    // initialization connection
    INIT = 0,
    USER,
    // game state
    STATE,

    // Entity Data
    ENTITY
};

public class NetworkManager : MonoBehaviour
{
    public static GameObject ball;
    public static GameObject paddle1;
    public static GameObject paddle2;

    static int PacketSize = 512;
    static int InitialOffset = 12;
    static int PACKET_LENGTH = 0;
    static int PACKET_SENDER = 4;
    static int PACKET_TYPE = 8;
    ///////////////////////////////////////////////////////////////////////////////
    // DLL IMPORT
    ///////////////////////////////////////////////////////////////////////////////

    const string DLL_NAME = "NetworkPlugin";
    //net code
    [DllImport(DLL_NAME)]
    static extern IntPtr CreateClient();                                            //Creates a client
    [DllImport(DLL_NAME)]
    static extern void DeleteClient(IntPtr client);                                 //Destroys a client
    [DllImport(DLL_NAME)]
    static extern bool Connect(IntPtr client, string ipAddress);                          //Connects to c++ Server
    [DllImport(DLL_NAME)]
    static extern void SetupPacketReception(Action<IntPtr, int, bool> action);
    [DllImport(DLL_NAME)]
    static extern void Update(IntPtr client);                                //Starts updating
    [DllImport(DLL_NAME)]
    static extern bool SendDataPacket(IntPtr ptr, int length, bool TCP, IntPtr client);
    [DllImport(DLL_NAME)]
    static extern void ShowConsole(IntPtr client, bool open);

    public static string ip = "127.0.0.1";
    private static IntPtr Client;
    private static int playerNum = 0;
    //private static bool endGame = false;
    //public static GameState gameState = GameState.LOBBY;
    static public bool isConnected = false;

    static byte[] sendByteArray = new byte[PacketSize];
    static byte[] tcpByteArray = new byte[PacketSize];
    static byte[] udpByteArray = new byte[PacketSize];

    private void Awake()
    {
        Client = CreateClient();
        SetupPacketReception(receivePacket);
        Update(Client);
        ShowConsole(Client, false);
    }

    private void OnDestroy()
    {
        DeleteClient(Client);
    }

    ///////////////////////////////////////////////////////////////////////////////
    // SEND PACKET
    ///////////////////////////////////////////////////////////////////////////////
    static bool SendIntPtr(ref byte[] bytes, int length, bool TCP, int receiver, int packetType)
    {
        bool returnVal = false;
        if (isConnected)
        {
            BitConverter.GetBytes(length).CopyTo(bytes, PACKET_LENGTH);
            BitConverter.GetBytes(receiver).CopyTo(bytes, PACKET_SENDER);
            BitConverter.GetBytes(packetType).CopyTo(bytes, PACKET_TYPE);

            IntPtr ptr = Marshal.AllocCoTaskMem(length);

            Marshal.Copy(bytes, 0, ptr, length);

            //SendDataFunc
            returnVal = SendDataPacket(ptr, length, TCP, Client);

            Marshal.FreeCoTaskMem(ptr);
        }
        return returnVal;
    }

    public static void SendPacketLoc(Vector3 pos)
    {
        int loc = InitialOffset;

        PackData(ref sendByteArray, ref loc, pos.x);
        PackData(ref sendByteArray, ref loc, pos.y);
        PackData(ref sendByteArray, ref loc, pos.z);

        SendIntPtr(ref sendByteArray, loc, true, (playerNum == 0 ? 1 : 0), (int)PacketType.ENTITY);
    }

    ///////////////////////////////////////////////////////////////////////////////
    // RECEIVE PACKET
    ///////////////////////////////////////////////////////////////////////////////

    static void receivePacket(IntPtr ptr, int length, bool TCP)
    {
        Debug.Log("Got Packet");
        if (length < PacketSize && length >= InitialOffset)
        {

            //SendDebugOutput("C# RECEIVED PACKET");
            if (TCP)
            {
                Debug.Log("Got TCP Packet");
                Marshal.Copy(ptr, tcpByteArray, 0, length);
                //SendDebugOutput("C# DECODED TCP PACKET");
                deconstructPacket(ref tcpByteArray, length);
            }
            else
            {
                Marshal.Copy(ptr, udpByteArray, 0, length);
                //SendDebugOutput("C# DECODED UDP PACKET");
                deconstructPacket(ref udpByteArray, length);
            }
        }
    }

    static void deconstructPacket(ref byte[] bytes, int length)
    {
        int type = BitConverter.ToInt32(bytes, PACKET_TYPE);
        int sender = BitConverter.ToInt32(bytes, PACKET_SENDER);
        int loc = InitialOffset;

        //SendDebugOutput("Type: " + type.ToString() + " , Sender: " + sender.ToString());
        switch (type)
        {
            case (int)PacketType.INIT:
                {
                    if (sender == -1)
                    {
                        //SendDebugOutput("C# GOT INIT FROM SERVER");
                        Debug.Log("Got Init Packet");
                        int index = 0;
                        UnpackInt(ref bytes, ref loc, ref index);
                        playerNum = index;
                        StartManager.Instance.SetLoadGame(true);
                    }
                }
                break;
            case (int)PacketType.ENTITY:
                //SendDebugOutput("Update");
                if (sender != playerNum)
                {
                    float posX = 0;
                    float posY = 0;
                    float posZ = 0;
                    UnpackFloat(ref bytes, ref loc, ref posX);
                    UnpackFloat(ref bytes, ref loc, ref posY);
                    UnpackFloat(ref bytes, ref loc, ref posZ);
                    if (sender == 0)
                    {
                        paddle1.transform.position = new Vector3(posX, posY, posZ);
                    }
                    else
                    {
                        paddle2.transform.position = new Vector3(posX, posY, posZ);
                    }
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public static void ConnectToServer(string ipAddr)
    {
        if (ipAddr != "")
        {
            ip = ipAddr;
        }
        // Client Connections
        if (!Connect(Client, ip))
        {
            Debug.Log("Failed to Connect");
        }
    }

    public static void PackData(ref byte[] bytes, ref int loc, bool data)
    {
        BitConverter.GetBytes(data).CopyTo(bytes, loc);
        loc += Marshal.SizeOf(data);
    }
    public static void PackData(ref byte[] bytes, ref int loc, int data)
    {
        BitConverter.GetBytes(data).CopyTo(bytes, loc);
        loc += Marshal.SizeOf(data);
    }
    public static void PackData(ref byte[] bytes, ref int loc, float data)
    {
        BitConverter.GetBytes(data).CopyTo(bytes, loc);
        loc += Marshal.SizeOf(data);
    }
    public static void PackData(ref byte[] bytes, ref int loc, char data)
    {
        bytes[loc] = (byte)data;
        loc += Marshal.SizeOf(data);
    }

    public static void UnpackBool(ref byte[] byteArray, ref int loc, ref bool output)
    {
        output = BitConverter.ToBoolean(byteArray, loc);
        loc += Marshal.SizeOf(output);
    }

    public static void UnpackInt(ref byte[] byteArray, ref int loc, ref int output)
    {
        output = BitConverter.ToInt32(byteArray, loc);
        loc += Marshal.SizeOf(output);
    }

    public static void UnpackFloat(ref byte[] byteArray, ref int loc, ref float output)
    {
        output = BitConverter.ToSingle(byteArray, loc);
        loc += Marshal.SizeOf(output);
    }
    public static void UnpackChar(ref byte[] byteArray, ref int loc, ref char output)
    {
        output = (char)byteArray[loc];
        loc += Marshal.SizeOf(output);
    }
}