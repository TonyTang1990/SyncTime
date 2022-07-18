/*
 * Description:             NTPClient.cs
 * Author:                  #AUTHOR#
 * Create Date:             #CREATEDATE#
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// NTPClient.cs
/// NTP客户端
/// </summary>
public class NTPClient
{
    /// <summary>
    /// 对时网络地址(直接传IP地址时无值)
    /// </summary>
    public string Host
    {
        get;
        private set;
    }

    /// <summary>
    /// 对时IP连接地址
    /// </summary>
    public IPEndPoint IPEnd
    {
        get;
        private set;
    }

    /// <summary>
    /// NTP连接端口号
    /// </summary>
    private const int PortNumber = 123;

    /// <summary>
    /// NTP Socket
    /// </summary>
    private Socket mNTPSocket;
    
    /// <summary>
    /// NTP请求数据
    /// </summary>
    private byte[] mNtpSendData;

    /// <summary>
    /// NTP接受数据
    /// </summary>
    private byte[] mNtpReceiveData;

    public NTPClient()
    {
        mNTPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        mNtpSendData = new byte[48];
        // Setting the Leap Indicator, Version Number and Mode values
        // LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)
        mNtpSendData[0] = 0x1B;
        mNtpReceiveData = new byte[48];
    }

    /// <summary>
    /// 初始化对时地址
    /// </summary>
    /// <param name="host"></param>
    public bool InitByHost(string host)
    {
        Host = host;
        IPEnd = null;
        var ipAdresses = Dns.GetHostAddresses(Host);
        if(ipAdresses != null && ipAdresses.Length > 0)
        {
            IPEnd = new IPEndPoint(ipAdresses[0], PortNumber);
            return true;
        }
        else
        {
            Debug.LogError($"网络地址:{host}的IP解析错误!");
            return false;
        }
    }

    /// <summary>
    /// 初始化对时IP地址
    /// </summary>
    /// <param name="ip"></param>
    public bool InitByIP(string ip)
    {
        Host = null;
        IPEnd = null;
        var ipAdress = IPAddress.Parse(ip);
        if (ipAdress != null)
        {
            IPEnd = new IPEndPoint(ipAdress, PortNumber);
            return true;
        }
        else
        {
            Debug.LogError($"IP地址:{ip}解析错误!");
            return false;
        }
    }

    /// <summary>
    /// 网络对时
    /// </summary>
    /// <returns></returns>
    public bool SyncTime()
    {
        if(IPEnd != null)
        {
            Byte
            mNTPSocket.Connect(IPEnd);
            mNTPSocket.ReceiveTimeout = 3000;
            mNTPSocket.Send(ntpData);
            mNTPSocket.Receive(ntpData);
            mNTPSocket.Close();
            const byte serverReplyTime = 40;
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);
            intPart = swapEndian(intPart);
            fractPart = swapEndian(fractPart);
            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000UL);
        }
        else
        {
            Debug.LogError($"未初始化IP地址,网络对时失败!");
            return false;
        }
    }
}