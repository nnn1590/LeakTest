namespace NNN1590.LeakTest;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

public static class Native {
	private static IntPtr dllImportResolverHandle;
	private static readonly bool initialized;
	static Native() {
		if (!initialized) {
			NativeLibrary.SetDllImportResolver(typeof(Native).Assembly, (string libraryName, Assembly assembly, DllImportSearchPath? searchPath) => {
				dllImportResolverHandle = IntPtr.Zero;
				// ネイティブライブラリとこのアプリ(.NET)の.dllの名前が(大文字小文字を区別しないシステムでは)かぶるため ちょっと無理やり置き換える
				// | ネイティブ: libteaktest.so | .NET: LeakTest.dll | ←こうなるため。Windowsでは[DllImport("lesktest")]とした際、後者が読み込まれてしまい EntryPointNotFoundException が発生する
				// (ライブラリファイル名が(libteaktest.soではなく)「leaktest.dll」だったりしたら詰む)
				if (OperatingSystem.IsWindows() && libraryName.Equals("leaktest")) libraryName = "libleaktest.so";
				if (NativeLibrary.TryLoad(libraryName, assembly, searchPath, out dllImportResolverHandle)) {
					return dllImportResolverHandle;
				}
				return dllImportResolverHandle;
			});
			initialized = true;
		}
	}

	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testCharPtr(ReturnCharPtrFuncPtr func);
	[DllImport("leaktest", EntryPoint = "testCharPtr", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testCharPtrReal(ReturnCharPtrFunc func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testCharPtr(ReturnCharPtrFuncUnsafeVoidPtr func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testCharPtr(ReturnCharPtrFuncUnsafeCharPtr func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testInt(ReturnIntFunc func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testIntPtr(ReturnIntPtrFunc func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern TestStruct1 testStruct(TestStruct1 func);

	internal delegate int ReturnIntFunc();
	internal delegate IntPtr /* int* */ ReturnIntPtrFunc();
	[return: MarshalAs(UnmanagedType.LPStr)]
	internal delegate string? ReturnCharPtrFunc();
	internal delegate string? ReturnCharPtrFunc2();
	internal delegate IntPtr ReturnCharPtrFuncPtr();
	internal unsafe delegate void* ReturnCharPtrFuncUnsafeVoidPtr();
	internal unsafe delegate byte* ReturnCharPtrFuncUnsafeCharPtr();

	internal static void FreeCoTaskMems(HashSet<IntPtr> coTaskMems) {
		foreach (IntPtr coTaskMem in coTaskMems) {
			Console.WriteLine($"FREEING {coTaskMem.ToInt64()}");
			Marshal.FreeCoTaskMem(coTaskMem);
			coTaskMems.Remove(coTaskMem);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct TestStruct1 {
		[MarshalAs(UnmanagedType.LPStr)] public string? str1;
		[MarshalAs(UnmanagedType.LPStr)] public string? str2;
		[MarshalAs(UnmanagedType.LPStr)] public string? str3;
		[MarshalAs(UnmanagedType.LPStr)] public string? str4;
		[MarshalAs(UnmanagedType.LPStr)] public string? str5;
		[MarshalAs(UnmanagedType.LPStr)] public string? str6;
		[MarshalAs(UnmanagedType.LPStr)] public string? str7;
		[MarshalAs(UnmanagedType.LPStr)] public string? str8;
		[MarshalAs(UnmanagedType.LPStr)] public string? str9;
		[MarshalAs(UnmanagedType.LPStr)] public string? str10;
	}

	// Seems this leaks sometime
	internal static void testCharPtr(ReturnCharPtrFunc func) {
		IntPtr iPtr = Marshal.StringToCoTaskMemAnsi(func());
		try {
			testCharPtr(() => iPtr);
		} finally {
			Marshal.FreeCoTaskMem(iPtr);
		}
		return;
	}

	static unsafe byte* utf8StringPtr;
	// static unsafe byte* returnUtf8StringPtr() => utf8StringPtr;

	internal static void testCharPtrWithUnsafe(ReturnCharPtrFunc func) {
		byte[] utf8String;
		string? str = func();
		utf8String = (str == null) ? (new byte[] { 0 }) : Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(str));
		unsafe {
			fixed (byte* _utf8StringPtr = utf8String) utf8StringPtr = _utf8StringPtr;
			testCharPtr(() => utf8StringPtr);
			// This leaks sometime:
			// 	ReturnCharPtrFuncUnsafeCharPtr pointer = returnUtf8StringPtr;
			// 	testCharPtr(pointer);
			// However, using lambda expressions and global variables won't leak memory (local variables will leak memory sometime):
			// 	(global) static unsafe byte* utf8StringPtr;
			//
			// 	fixed (byte* _utf8StringPtr = utf8String) utf8StringPtr = _utf8StringPtr;
			// 	testCharPtr(() => utf8StringPtr);
		}
	}
}
