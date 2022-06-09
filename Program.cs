namespace NNN1590.LeakTest;

public class Program {
	public static int Main(string[] args) {

		int loopCount = 2000000;
		Console.Error.WriteLine($"===== LeakTest =====");

		Flags.TestType testType = 0;
		Flags.RunType  runType  = Flags.RunType.LeakingAndNotLeaking;
		foreach (string arg in args) {
			if (arg.ToLower().Equals("intfunc")) testType |= Flags.TestType.ReturnIntFunc;
			if (arg.ToLower().Equals("intptr" )) testType |= Flags.TestType.ReturnIntPtrFunc;
			if (arg.ToLower().Equals("charptr")) testType |= Flags.TestType.ReturnCharPtrFunc;

			if (arg.ToLower().Equals("leakingandnotleaking")) runType = Flags.RunType.LeakingAndNotLeaking;
			if (arg.ToLower().Equals("notleakingandleaking")) runType = Flags.RunType.NotLeakingAndLeaking;
			if (arg.ToLower().Equals("leakingonly"         )) runType = Flags.RunType.LeakingOnly;
			if (arg.ToLower().Equals("notleakingonly"      )) runType = Flags.RunType.NotLeakingOnly;
		}
		if (testType.HasFlag(Flags.TestType.ReturnIntFunc    )) Console.Error.WriteLine("Enabled: ReturnIntFunc");
		if (testType.HasFlag(Flags.TestType.ReturnIntPtrFunc )) Console.Error.WriteLine("Enabled: ReturnIntPtrFunc");
		if (testType.HasFlag(Flags.TestType.ReturnCharPtrFunc)) Console.Error.WriteLine("Enabled: ReturnCharPtrFunc");
		Console.Error.WriteLine($"Running: {runType}");
		Console.Error.WriteLine("Press any key to continue..."); Console.ReadKey(); Console.Error.WriteLine();
		switch (runType) {
			case Flags.RunType.LeakingAndNotLeaking: new Leaking(loopCount).Run(testType);    new NotLeaking(loopCount).Run(testType); break;
			case Flags.RunType.NotLeakingAndLeaking: new NotLeaking(loopCount).Run(testType); new Leaking(loopCount).Run(testType);    break;
			case Flags.RunType.LeakingOnly:          new Leaking(loopCount).Run(testType);    break;
			case Flags.RunType.NotLeakingOnly:       new NotLeaking(loopCount).Run(testType); break;
		}

		return 0;
	}
}
