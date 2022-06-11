namespace NNN1590.LeakTest;

using System.Runtime.InteropServices;
using static Native;

public class NotLeaking {
	// private readonly HashSet<IntPtr> coTaskMems;
	private readonly int loopCount;

	static NotLeaking() {}

	internal NotLeaking(int loopCount) {
		// coTaskMems = new HashSet<IntPtr>();
		this.loopCount = loopCount;
	}

	private static int ReturnIntFuncImpl() => 128;

	IntPtr returnIntPtrFuncResult;
	private IntPtr /* int* */ ReturnIntPtrFuncImpl() {
		returnIntPtrFuncResult = Marshal.AllocCoTaskMem(sizeof(int));
		// coTaskMems.Add(returnIntPtrFuncResult);
		Marshal.WriteInt32(returnIntPtrFuncResult, 256);
		return returnIntPtrFuncResult;
	}

	IntPtr returnCharPtrFuncPtrResult;
	// [return: MarshalAs(UnmanagedType.LPStr)]
	private IntPtr ReturnCharPtrFuncPtrImpl() {
		returnCharPtrFuncPtrResult = Marshal.StringToCoTaskMemAnsi("Hello, World!");
		return returnCharPtrFuncPtrResult;
	}

	[return: MarshalAs(UnmanagedType.LPStr)]
	private string? ReturnCharPtrFuncImpl() => "Hello, World!";

	[return: MarshalAs(UnmanagedType.LPStr)]
	private static string? ReturnCharPtrFuncImplStatic() => "Hello, World!";

	public void Run(Flags.TestType testType) {
		ReturnCharPtrFuncPtr returnCharPtrFuncPtr = ReturnCharPtrFuncPtrImpl;
		ReturnCharPtrFunc returnCharPtrFunc = ReturnCharPtrFuncImpl;
		ReturnIntPtrFunc returnIntPtrFunc = ReturnIntPtrFuncImpl;
		ReturnIntFunc returnIntFunc = ReturnIntFuncImpl;

		// These will not leak:
		if (testType.HasFlag(Flags.TestType.ReturnIntFunc    )) { Console.Error.WriteLine($"testInt"); for (int i = 0; i < loopCount; i++) testInt(returnIntFunc); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }

		if (testType.HasFlag(Flags.TestType.ReturnCharPtrFunc)) { Console.Error.WriteLine($"testCharPtr"); for (int i = 0; i < loopCount; i++) {
#if false
			testCharPtr(returnCharPtrFuncPtr);
			Marshal.FreeCoTaskMem(returnCharPtrFuncPtrResult);
#else
			testCharPtrWithUnsafe(returnCharPtrFunc);
#endif
		} Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }

		if (testType.HasFlag(Flags.TestType.ReturnIntPtrFunc )) { Console.Error.WriteLine($"testIntPtr"); for (int i = 0; i < loopCount; i++) {
			testIntPtr(returnIntPtrFunc);
			Marshal.FreeCoTaskMem(returnIntPtrFuncResult);
		} Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }

		if (testType.HasFlag(Flags.TestType.TestStruct1      )) { Console.Error.WriteLine($"testStruct"); for (int i = 0; i < loopCount; i++) {
			testStruct(new TestStruct1() {
				str1 = "GO",
				str2 = "野獣先輩",
				str3 = "KMR",
				str4 = "MUR",
				str5 = "遠野",
				str6 = "平野源五郎",
				str7 = "拓也さん",
				str8 = "多田野数人",
				str9 = "DB",
				str10 = "HTN"
			});
		} Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }
		// Trying to free CoTaskMems (Not working?)
		// Console.Error.WriteLine($"FreeCoTaskMems"); FreeCoTaskMems(coTaskMems); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();
	}

	public static void RunStatic(Flags.TestType testType, int loopCount) {
		ReturnCharPtrFunc returnCharPtrFunc = ReturnCharPtrFuncImplStatic;

		// These will not leak:
		if (testType.HasFlag(Flags.TestType.ReturnCharPtrFunc)) { Console.Error.WriteLine($"testCharPtr"); for (int i = 0; i < loopCount; i++) {
#if false
			testCharPtr(returnCharPtrFuncPtr);
			Marshal.FreeCoTaskMem(returnCharPtrFuncPtrResult);
#else
			testCharPtr(returnCharPtrFunc);
#endif
		} Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }
	}
}
