using System;
using System.Runtime.InteropServices;

namespace Shu.Util
{
    #region 转换 Float Int Uint
    /// <summary>
    /// 对象的各个成员在非托管内存中的精确位置被显式控制。
    /// 每个成员使用 FieldOffsetAttribute 指示该字段在类型中的位置
    /// 转换 Float Int Uint
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Convert_Float_Int_Uint
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public uint uiUint;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public int iInt;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public float fFloat;
    }
    #endregion

    #region 转换 Double Long Ulong
    /// <summary>
    /// 对象的各个成员在非托管内存中的精确位置被显式控制。
    /// 每个成员使用 FieldOffsetAttribute 指示该字段在类型中的位置
    /// 转换 Double Long Ulong
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Convert_Double_Long_Ulong
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public ulong ulUlong;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public long lLong;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public double dDouble;
    }
    #endregion
	
	public static class Convert_Struct
    {
		#region Struct 转换 Bytes
		/// <summary>Structs to bytes.</summary>		
		/// <param name='obj'>对象</param>
		/// <returns> 返回 bytes. </returns>
		public static byte[] StructToBytes<T>(T obj)
		{
			int size = Marshal.SizeOf(obj);
			byte[] bytes = new byte[size];
			IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
			try
			{
				Marshal.StructureToPtr(obj, buffer, true);
				return bytes;
			}
			finally
			{
				Marshal.FreeHGlobal(buffer);
			}
		}
		
		static byte[] StructToBytes(object structObj)
		{   
			int size = Marshal.SizeOf(structObj);
			IntPtr buffer = Marshal.AllocHGlobal(size);   
			try   
			{   
				Marshal.StructureToPtr(structObj, buffer, false);   
				byte[] bytes = new byte[size];
				Marshal.Copy(buffer, bytes, 0, size);
				return bytes;
			}   
			finally   
			{   
				Marshal.FreeHGlobal(buffer);   
			} 
		  }
		#endregion
		
		#region Bytes 转换 Struct
		/// <summary>Bytes to Structs.</summary>		
		/// <param name='obj'>Bytes</param>
		/// <returns> 返回 结构. </returns>
		public static T BytesToStruct<T>(byte[] bytes)
		{
			IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
			try
			{
				return (T)Marshal.PtrToStructure(buffer, typeof(T));
			}
			finally
			{
				Marshal.FreeHGlobal(buffer);
			}
		} 
		
		static object BytesToStruct(byte[] bytes, Type strcutType)   
  		{   
  			int size = Marshal.SizeOf(strcutType);
			IntPtr buffer = Marshal.AllocHGlobal(size);
			try
			{   
				Marshal.Copy(bytes, 0, buffer, size);
				return Marshal.PtrToStructure(buffer, strcutType);
			}
			finally  
			{
				Marshal.FreeHGlobal(buffer);
			}
		}
		#endregion
	}
}
