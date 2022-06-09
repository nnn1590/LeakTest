namespace NNN1590.LeakTest;

using System.Runtime.InteropServices;

public class Program {
	private static readonly HashSet<IntPtr> coTaskMems;

	static Program() {
		coTaskMems = new HashSet<IntPtr>();
	}

	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	static extern void testCharPtr(ReturnCharPtrFunc func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	static extern void testInt(ReturnIntFunc func);
	[DllImport("leaktest", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
	static extern void testIntPtr(ReturnIntPtrFunc func);

	delegate int ReturnIntFunc();
	delegate IntPtr /* int* */ ReturnIntPtrFunc();
	[return: MarshalAs(UnmanagedType.LPStr)]
	delegate string? ReturnCharPtrFunc();

	private static int ReturnIntFuncImpl() => 128;

	static IntPtr result;
	private static IntPtr /* int* */ ReturnIntPtrFuncImpl() {
		result = Marshal.AllocCoTaskMem(sizeof(int));
		coTaskMems.Add(result);
		Marshal.WriteInt32(result, 256);
		return result;
	}

	[return: MarshalAs(UnmanagedType.LPStr)]
	private static string? ReturnCharPtrFuncImpl() { return "Hello, World!"; }

	public static int Main(string[] args) {
		ReturnCharPtrFunc returnCharPtrFunc = ReturnCharPtrFuncImpl;
		ReturnIntPtrFunc returnIntPtrFunc = ReturnIntPtrFuncImpl;
		ReturnIntFunc returnIntFunc = ReturnIntFuncImpl;

		int loopCount = 2000000;
		Console.Error.WriteLine($"===== LeakTest ====="); Console.Error.WriteLine("Press any key to continue..."); Console.ReadKey(); Console.Error.WriteLine();

		// These will leak:
		Console.Error.WriteLine($"testInt"); for (int i = 0; i < loopCount; i++) testInt(returnIntFunc); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();
		Console.Error.WriteLine($"testIntPtr"); for (int i = 0; i < loopCount; i++) testIntPtr(returnIntPtrFunc); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();
		Console.Error.WriteLine($"testCharPtr"); for (int i = 0; i < loopCount; i++) testCharPtr(returnCharPtrFunc); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();
		Console.Error.WriteLine($"FreeCoTaskMems"); FreeCoTaskMems(); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();  // Not working?

		// These will not leak:
		Console.Error.WriteLine($"testIntPtr"); for (int i = 0; i < loopCount; i++) {var s = new String("aaa");
			testIntPtr(returnIntPtrFunc);
			Marshal.FreeCoTaskMem(result);
		} Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();

		return 0;
	}

	static void FreeCoTaskMems() {
		foreach (IntPtr coTaskMem in coTaskMems) {
			Console.WriteLine($"FREEING {coTaskMem.ToInt64()}");
			Marshal.FreeCoTaskMem(coTaskMem);
			coTaskMems.Remove(coTaskMem);
		}
	}
}
