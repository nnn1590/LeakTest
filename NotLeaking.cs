namespace NNN1590.LeakTest;

using System.Runtime.InteropServices;
using static Native;

public class NotLeaking {
	// private readonly HashSet<IntPtr> coTaskMems;
	private readonly int loopCount;

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

	string? returnCharPtrFuncResult;
	[return: MarshalAs(UnmanagedType.LPStr)]
	private string? ReturnCharPtrFuncImpl() {
		returnCharPtrFuncResult = "Hello, World!";
		return returnCharPtrFuncResult;
	}

	public void Run(Flags.TestType testType) {
		ReturnCharPtrFunc returnCharPtrFunc = ReturnCharPtrFuncImpl;
		ReturnIntPtrFunc returnIntPtrFunc = ReturnIntPtrFuncImpl;
		ReturnIntFunc returnIntFunc = ReturnIntFuncImpl;

		// These will not leak:
		if (testType.HasFlag(Flags.TestType.ReturnIntFunc    )) { Console.Error.WriteLine($"testInt"); for (int i = 0; i < loopCount; i++) testInt(returnIntFunc); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }
		if (testType.HasFlag(Flags.TestType.ReturnIntPtrFunc )) { Console.Error.WriteLine($"testCharPtr"); for (int i = 0; i < loopCount; i++) {
			testCharPtr(returnCharPtrFunc);
		} Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }

		if (testType.HasFlag(Flags.TestType.ReturnCharPtrFunc)) { Console.Error.WriteLine($"testIntPtr"); for (int i = 0; i < loopCount; i++) {
			testIntPtr(returnIntPtrFunc);
			Marshal.FreeCoTaskMem(returnIntPtrFuncResult);
		} Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }

		// Trying to free CoTaskMems (Not working?)
		// Console.Error.WriteLine($"FreeCoTaskMems"); FreeCoTaskMems(coTaskMems); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();
	}
}