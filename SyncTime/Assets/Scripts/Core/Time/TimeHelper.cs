/*
 * Description:             TimeHelper.cs
 * Author:                  TonyTang
 * Create Date:             2022/07/16
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 时间静态类
/// </summary>
public static class TimeHelper
{
    /// <summary>
    /// Unix时间戳基准时间
    /// </summary>
    private static DateTime UnixBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 同步网络UTC时间
    /// </summary>
    private static DateTime? mSyncUTCTime;

    /// <summary>
    /// 同步网络本地时间
    /// </summary>
    private static DateTime? mSyncLocalTime;

    /// <summary>
    /// 同步时间时的本地运行时间(未同步成功为0)
    /// </summary>
    private static float mSyncRealtime;

    /// <summary>
    /// 同步网络UTC时间戳(未同步成功为0)
    /// </summary>
    private static long mSyncUTCTimeStamp;

    /// <summary>
    /// 设置当前UTC时间
    /// </summary>
    /// <param name="nowDateTime"></param>
    public static void SetNowUTCTime(DateTime? nowDateTime)
    {
        mSyncUTCTime = nowDateTime;
        mSyncLocalTime = nowDateTime != null ? ((DateTime)nowDateTime).ToLocalTime() : nowDateTime;
        if (mSyncUTCTime != null)
        {
            mSyncRealtime = Time.realtimeSinceStartup;
            var offsetTimeSpan = (DateTime)mSyncUTCTime - UnixBaseTime;
            mSyncUTCTimeStamp = (long)offsetTimeSpan.TotalSeconds;
            Debug.Log($"同步当前UTC时间:{mSyncUTCTime.ToString()}");
            Debug.Log($"同步当前时区时间:{mSyncLocalTime.ToString()}");
            Debug.Log($"同步时间时本地运行时间:{mSyncRealtime}");
            Debug.Log($"同步时间UTC时间戳:{mSyncUTCTimeStamp}");
        }
        else
        {
            mSyncRealtime = 0;
            mSyncUTCTimeStamp = 0;
            Debug.Log($"清空当前同步时间!");
        }
    }

    /// <summary>
    /// 获取当前同步UTC时间戳(没有成功同步返回本地UTC时间戳)
    /// </summary>
    /// <returns></returns>
    public static long GetNowUTCTimeStamp()
    {
        var nowUTCTime = GetNowUTCTime();
        var offsetSeconds = nowUTCTime - UnixBaseTime;
        return (long)offsetSeconds.TotalSeconds;
    }

    /// <summary>
    /// 获取当前同步UTC时间(没有成功同步返回本地UTC时间)
    /// </summary>
    /// <returns></returns>
    public static DateTime GetNowUTCTime()
    {
        if(mSyncUTCTime != null)
        {
            var syncUTCTime = (DateTime)mSyncUTCTime;
            return syncUTCTime.AddSeconds(Time.realtimeSinceStartup - mSyncRealtime);
        }
        else
        {
            return GetLocalNowUTCTime();
        }
    }

    /// <summary>
    /// 获取当前同步本地时区时间(没有成功同步返回本地时区时间)
    /// </summary>
    /// <returns></returns>
    public static DateTime GetNowLocalTime()
    {
        if (mSyncLocalTime != null)
        {
            var syncLocalTime = (DateTime)mSyncLocalTime;
            return syncLocalTime.AddSeconds(Time.realtimeSinceStartup - mSyncRealtime);
        }
        else
        {
            return GetLocalNowTime();
        }
    }

    /// <summary>
    /// 获取本地当前UTC时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetLocalNowUTCTimeStamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    /// <summary>
    /// 获取本地当前UTC时间
    /// </summary>
    /// <returns></returns>
    public static DateTime GetLocalNowUTCTime()
    {
        return DateTime.UtcNow;
    }

    /// <summary>
    /// 获取本地当前时间
    /// </summary>
    /// <returns></returns>
    public static DateTime GetLocalNowTime()
    {
        return DateTime.Now;
    }
}
