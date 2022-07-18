using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 二进制辅助静态工具类
/// </summary>
public static class ByteUtilities
{
    /// <summary>
    /// 当前设备是否是小端
    /// </summary>
    /// <returns></returns>
    public static unsafe bool IsLittleEndian()
    {
        int i = 1;
        byte* b = (byte*)&i;
        return b[0] == 1;
    }

    /// <summary>
    /// 小端存储与大端存储的转换
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static uint SwapEndian(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
        ((x & 0x0000ff00) << 8) +
        ((x & 0x00ff0000) >> 8) +
        ((x & 0xff000000) >> 24));
    }
}
