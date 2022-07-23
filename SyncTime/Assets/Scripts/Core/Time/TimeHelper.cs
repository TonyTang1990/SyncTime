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
    /// 同步网络UTC时间
    /// </summary>
    private static DateTime? mSyncUTCTime;

    /// <summary>
    /// 同步网络本地时间
    /// </summary>
    private static DateTime? mSyncLocalTime;

    /// <summary>
    /// 同步时间时的本地运行时间
    /// </summary>
    private static float mSyncRealtime;

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
            Debug.Log($"同步时间时本地运行时间:{mSyncRealtime}");
            Debug.Log($"同步当前UTC时间:{mSyncUTCTime.ToString()}");
            Debug.Log($"同步当前时区时间:{mSyncLocalTime.ToString()}");
        }
        else
        {
            Debug.Log($"清空当前同步时间!");
        }
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
