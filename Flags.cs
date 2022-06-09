namespace NNN1590.LeakTest;

using System.Runtime.InteropServices;
using static Native;

public static class Flags {
	[Flags]
	public enum TestType {
		ReturnCharPtrFunc = 0x1,
		ReturnIntFunc = 0x2,
		ReturnIntPtrFunc = 0x4
	}
	[Flags]
	public enum RunType {
		LeakingAndNotLeaking = 0x1,
		NotLeakingAndLeaking = 0x2,
		LeakingOnly = 0x4,
		NotLeakingOnly = 0x8
	}
}
