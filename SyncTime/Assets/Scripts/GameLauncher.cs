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
    /// 网络同步时间文本
    /// </summary>
    public Text SyncNowTimeTxt;

    /// <summary>
    /// 本地时间文本
    /// </summary>
    public Text LocalNowTimeTxt;

    /// <summary>
    /// 响应同步时间按钮点击
    /// </summary>
    public void OnSyncTimeBtnClick()
    {
        Debug.Log($"OnSyncTimeBtnClick()");
        Time.SyncTime();
    }

    public void Update()
    {
        if (SyncNowTimeTxt != null)
        {
            SyncNowTimeTxt.text = $"当前同步网络时间:{Time.GetSyncNowUTCTime().ToString()}";
        }
        if (LocalNowTimeTxt != null)
        {
            LocalNowTimeTxt.text = $"当前本地同步时间:{Time.GetLocalNowUTCTime().ToString()}";
        }
    }
}