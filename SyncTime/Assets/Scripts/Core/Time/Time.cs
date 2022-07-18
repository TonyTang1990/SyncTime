using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 时间静态类
/// </summary>
public static class Time
{
    /// <summary>
    /// NTP网址列表
    /// </summary>
    private static List<string> NTPHostList = new List<string>
    {
        "ntp1.aliyun.com",
        "ntp2.aliyun.com",
        "ntp3.aliyun.com",
        "ntp4.aliyun.com",
        "ntp5.aliyun.com",
        "ntp6.aliyun.com",
        "ntp7.aliyun.com",
    };

    /// <summary>
    /// NPC网络对时同步对象
    /// </summary>
    private static NTPClient NTPClient = new NTPClient();

    /// <summary>
    /// 时间同步是否成功
    /// </summary>
    private static bool IsTimeSyncSuccess;

    /// <summary>
    /// 同步网络时间
    /// </summary>
    /// <returns></returns>
    public static bool SyncTime()
    {
        // 未来有服务器的话，改为和服务器对时
        for(int i = 0, length = NTPHostList.Count; i < length; i++)
        {
            NTPClient.InitByHost(NTPHostList[i]);
            if(NTPClient.SyncTime())
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 设置当前时间戳(UTC)
    /// </summary>
    /// <param name="nowTimeStamp"></param>
    public static void SetNowTime(int nowTimeStamp)
    {

    }
}
