using System;
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
    /// 同步网络UTC时间
    /// </summary>
    private static DateTime mSyncUTCTime;

    /// <summary>
    /// 同步网络时间
    /// </summary>
    /// <returns></returns>
    public static bool SyncTime()
    {
        // 未来有服务器的话，改为和服务器对时
        DateTime? syncDateTime = null;
        for(int i = 0, length = NTPHostList.Count; i < length; i++)
        {
            NTPClient.InitByHost(NTPHostList[i]);
            if(NTPClient.SyncTime(out syncDateTime))
            {
                IsTimeSyncSuccess = true;
                SetNowUTCTime((DateTime)syncDateTime);
                return true;
            }
        }
        IsTimeSyncSuccess = false;
        Debug.LogError($"所有NTP地址都同步时间失败!");
        return false;
    }

    /// <summary>
    /// 设置当前UTC时间
    /// </summary>
    /// <param name="nowDateTime"></param>
    public static void SetNowUTCTime(DateTime nowDateTime)
    {
        mSyncUTCTime = nowDateTime;
        if(mSyncUTCTime != null)
        {
            Debug.Log($"设置当前时间:{mSyncUTCTime.ToString()}");
        }
        else
        {
            Debug.Log($"清空当前同步时间!");
        }
    }

    /// <summary>
    /// 是否同步网络时间成功
    /// </summary>
    /// <returns></returns>
    public static bool IsSyncTimeSuccess()
    {
        return IsTimeSyncSuccess;
    }

    /// <summary>
    /// 获取当前同步UTC时间(没有成功同步返回本地UTC时间)
    /// </summary>
    /// <returns></returns>
    public static DateTime GetSyncNowUTCTime()
    {
        if(IsTimeSyncSuccess)
        {
            return mSyncUTCTime;
        }
        return GetLocalNowUTCTime();
    }

    /// <summary>
    /// 获取本地当前UTC时间
    /// </summary>
    /// <returns></returns>
    public static DateTime GetLocalNowUTCTime()
    {
        return DateTime.UtcNow;
    }
}
