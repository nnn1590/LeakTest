namespace NNN1590.LeakTest;

public static class Flags {
	[Flags]
	public enum TestType {
		None = 0x0,
		ReturnCharPtrFunc = 0x1,
		ReturnIntFunc = 0x2,
		ReturnIntPtrFunc = 0x4,
		TestStruct1 = 0x8
	}
	[Flags]
	public enum RunType {
		None = 0x0,
		LeakingAndNotLeaking = 0x1,
		NotLeakingAndLeaking = 0x2,
		LeakingOnly = 0x4,
		NotLeakingOnly = 0x8
	}
}
