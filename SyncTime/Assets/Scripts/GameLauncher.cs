/*
 * Description:             GameLauncher.cs
 * Author:                  TonyTang
 * Create Date:             2022/7/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameLauncher.cs
/// 游戏入口
/// </summary>
public class GameLauncher : MonoBehaviour
{
    /// <summary>
    /// 同步网络时间按钮
    /// </summary>
    public Button SyncTimeBtn;

    /// <summary>
    /// 网络同步本地时区时间戳文本
    /// </summary>
    public Text SyncNowLocalTimeStampTxt;

    /// <summary>
    /// 网络同步UTC时间文本
    /// </summary>
    public Text SyncNowTimeTxt;

    /// <summary>
    /// 网络同步本地时区时间文本
    /// </summary>
    public Text SyncNowLocalTimeTxt;

    /// <summary>
    /// 本地UTC时间戳文本
    /// </summary>
    public Text LocalNowUTCTimeStampTxt;

    /// <summary>
    /// 本地UTC时间文本
    /// </summary>
    public Text LocalNowUTCTimeTxt;

    /// <summary>
    /// 本地时区时间文本
    /// </summary>
    public Text LocalNowLocalTimeTxt;

    private void Start()
    {
        NTPClient.Singleton.SetHostList(NTPHostConfig.NTPHostList);
    }

    /// <summary>
    /// 响应同步时间按钮点击
    /// </summary>
    public void OnSyncTimeBtnClick()
    {
        Debug.Log($"OnSyncTimeBtnClick()");
        NTPClient.Singleton.SyncTime();
    }

    public void Update()
    {
        if (SyncNowLocalTimeStampTxt != null)
        {
            SyncNowLocalTimeStampTxt.text = $"对时UTC时间戳:{TimeHelper.GetNowUTCTimeStamp().ToString()}";
        }
        if (SyncNowTimeTxt != null)
        {
            SyncNowTimeTxt.text = $"对时UTC时间:{TimeHelper.GetNowUTCTime().ToString()}";
        }
        if (SyncNowLocalTimeTxt != null)
        {
            SyncNowLocalTimeTxt.text = $"对时本地时区时间:{TimeHelper.GetNowLocalTime().ToString()}";
        }
        if (LocalNowUTCTimeStampTxt != null)
        {
            LocalNowUTCTimeStampTxt.text = $"本地UTC时间戳:{TimeHelper.GetLocalNowUTCTimeStamp().ToString()}";
        }
        if (LocalNowUTCTimeTxt != null)
        {
            LocalNowUTCTimeTxt.text = $"本地UTC时间:{TimeHelper.GetLocalNowUTCTime().ToString()}";
        }
        if (LocalNowLocalTimeTxt != null)
        {
            LocalNowLocalTimeTxt.text = $"本地时区时间:{TimeHelper.GetLocalNowTime().ToString()}";
        }
    }
}