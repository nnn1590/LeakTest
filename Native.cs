namespace NNN1590.LeakTest;

using System.Runtime.InteropServices;

public static class Native {
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testCharPtr(ReturnCharPtrFunc func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testInt(ReturnIntFunc func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void testIntPtr(ReturnIntPtrFunc func);

	internal delegate int ReturnIntFunc();
	internal delegate IntPtr /* int* */ ReturnIntPtrFunc();
	[return: MarshalAs(UnmanagedType.LPStr)]
	internal delegate string? ReturnCharPtrFunc();

	internal static void FreeCoTaskMems(HashSet<IntPtr> coTaskMems) {
		foreach (IntPtr coTaskMem in coTaskMems) {
			Console.WriteLine($"FREEING {coTaskMem.ToInt64()}");
			Marshal.FreeCoTaskMem(coTaskMem);
			coTaskMems.Remove(coTaskMem);
		}
	}
}
