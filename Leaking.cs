namespace NNN1590.LeakTest;

using System.Runtime.InteropServices;
using static Native;

public class Leaking {
	private readonly HashSet<IntPtr> coTaskMems;
	private readonly int loopCount;

	internal Leaking(int loopCount) {
		coTaskMems = new HashSet<IntPtr>();
		this.loopCount = loopCount;
	}

	private static int ReturnIntFuncImpl() => 128;

	private IntPtr /* int* */ ReturnIntPtrFuncImpl() {
		IntPtr result = Marshal.AllocCoTaskMem(sizeof(int));
		coTaskMems.Add(result);
		Marshal.WriteInt32(result, 256);
		return result;
	}

	[return: MarshalAs(UnmanagedType.LPStr)]
	private string? ReturnCharPtrFuncImpl() => "Hello, World!";

	public void Run(Flags.TestType testType) {
		ReturnCharPtrFunc returnCharPtrFunc = ReturnCharPtrFuncImpl;
		ReturnIntPtrFunc returnIntPtrFunc = ReturnIntPtrFuncImpl;
		ReturnIntFunc returnIntFunc = ReturnIntFuncImpl;

		// These will leak (except testInt):
		if (testType.HasFlag(Flags.TestType.ReturnIntFunc    )) { Console.Error.WriteLine($"testInt");     for (int i = 0; i < loopCount; i++) testInt(returnIntFunc);             Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }
		if (testType.HasFlag(Flags.TestType.ReturnIntPtrFunc )) { Console.Error.WriteLine($"testIntPtr");  for (int i = 0; i < loopCount; i++) testIntPtr(returnIntPtrFunc);       Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }
		if (testType.HasFlag(Flags.TestType.ReturnCharPtrFunc)) { Console.Error.WriteLine($"testCharPtr"); for (int i = 0; i < loopCount; i++) testCharPtrReal(returnCharPtrFunc); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine(); }
		if (testType.HasFlag(Flags.TestType.TestStruct1      )) { Console.Error.WriteLine($"testStruct");  for (int i = 0; i < loopCount; i++) {
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
		Console.Error.WriteLine($"FreeCoTaskMems"); FreeCoTaskMems(coTaskMems); Console.Error.WriteLine("DONE"); Console.ReadKey(); Console.Error.WriteLine();
	}
}