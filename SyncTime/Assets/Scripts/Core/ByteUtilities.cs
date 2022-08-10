/*
 * Description:             ByteUtilities.cs
 * Author:                  TonyTang
 * Create Date:             2022/07/16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 二进制辅助静态工具类
/// </summary>
public static class ByteUtilities
{
    /// <summary>
    /// 是否是小端
    /// </summary>
    public static bool IsLittleEndian = CheckLittleEndian();

    /// <summary>
    /// 当前设备是否是小端
    /// </summary>
    /// <returns></returns>
    public static unsafe bool CheckLittleEndian()
    {
        int i = 1;
        byte* b = (byte*)&i;
        return b[0] == 1;
    }

    /// <summary>
    /// UInt16大小端转换
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ushort SwapEndianU16(ushort value)
    {
        return (ushort)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
    }

    /// <summary>
    /// UInt32大小端转换
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static uint SwapEndianU32(uint value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
               (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }

    /// <summary>
    /// UInt64大小端32位转换
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ulong SwapEndianU32(ulong value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
               (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }

    /// <summary>
    /// UInt64大小端64位转换
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ulong SwapEndianU64(ulong value)
    {
        return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
               (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
               (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
               (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
    }
}
