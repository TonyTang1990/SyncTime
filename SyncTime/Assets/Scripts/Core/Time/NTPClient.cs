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

    /// <summary>
    /// <summary>
    /// 服务器接收客户端时间请求时间起始位置
    /// </summary>
    private const int ServerReceivedTimePos = 32;

    /// <summary>
    /// 服务器回复时间起始位置
    /// </summary>
    private const int ServerReplyTimePos = 40;

    /// <summary>
    /// UTC时间戳基准时间
    /// </summary>
    private readonly DateTime UTCBaseTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
    /// <param name="syncDateTime"></param>
    /// <returns></returns>
    public bool SyncTime(out DateTime? syncDateTime)
    {
        syncDateTime = null;
        if (IPEnd != null)
        {
            try
            {
                mNTPSocket.Connect(IPEnd);
                mNTPSocket.ReceiveTimeout = 3000;
                // 客户端发送时间
                ulong clientSendTime = (ulong)DateTime.UtcNow.Millisecond;
                Debug.Log($"客户端发送时间:{clientSendTime}");
                mNTPSocket.Send(mNtpSendData);
                Array.Clear(mNtpReceiveData, 0, mNtpReceiveData.Length);
                var recceiveByteNumbers = mNTPSocket.Receive(mNtpReceiveData);
                Debug.Log($"接受返回字节数:{recceiveByteNumbers}");
                // 客户端接收时间
                ulong clientReceiveTime = (ulong)DateTime.UtcNow.Millisecond;
                Debug.Log($"客户端接收时间:{clientReceiveTime}");
                mNTPSocket.Close();
                // 服务器接受消息时间
                var serverReceivedTime = GetMilliSeconds(mNtpReceiveData, ServerReceivedTimePos);
                Debug.Log($"服务器接受消息时间:{serverReceivedTime}");
                // 服务器返回消息时间
                var serverReplyTime = GetMilliSeconds(mNtpReceiveData, ServerReplyTimePos);
                Debug.Log($"服务器返回消息时间:{serverReplyTime}");
                // 网路延时 = (客户端接收时间 - 客户端发送时间) - (服务器返回消息时间 - 服务器接受消息时间)
                // 时间差 = 服务器接受消息时间 - 客户端发送时间 - 网络延时 / 2 = ((服务器接受消息时间 - 客户端发送时间) + (服务器返回消息时间 - 客户端接收时间)) / 2
                // 当前同步服务器时间 = 客户端接收时间 + 时间差
                var syncTime = clientReceiveTime + ((serverReceivedTime - clientSendTime) + (serverReplyTime - clientReceiveTime)) / 2;
                Debug.Log($"IP地址:{IPEnd.ToString()},当前同步时间:{syncTime}");
                syncDateTime = UTCBaseTime.AddMilliseconds(syncTime);
                return true;
            }
            catch(SocketException e)
            {
                Debug.LogError($"IP地址:{IPEnd.ToString()}连接异常:{e.Message},ErrorCode:{e.ErrorCode}!");
                return false;
            }
        }
        else
        {
            Debug.LogError($"未初始化IP地址,网络对时失败!");
            return false;
        }
    }

    /// <summary>
    /// 获取指定偏移的时间戳
    /// </summary>
    /// <param name="byteDatas"></param>
    /// <param name="byteOffset"></param>
    /// <returns></returns>
    private ulong GetMilliSeconds(byte[] byteDatas, int byteOffset)
    {
        // 64bit时间戳，高32bit表示整数部分，低32bit表示小数部分
        ulong intPart = BitConverter.ToUInt32(mNtpReceiveData, ServerReplyTimePos);
        ulong fractPart = BitConverter.ToUInt32(mNtpReceiveData, ServerReplyTimePos + 4);
        intPart = ByteUtilities.ToLittleEndian(intPart);
        fractPart = ByteUtilities.ToLittleEndian(fractPart);
        return (intPart * 1000) + ((fractPart * 1000) / 0x100000000UL);
    }
}