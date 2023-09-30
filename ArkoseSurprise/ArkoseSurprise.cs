using HarmonyLib;
using OWML.ModHelper;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ArkoseSurprise;

[HarmonyPatch]
public class ArkoseSurprise : ModBehaviour
{
	private static ArkoseSurprise Instance;

	private void Awake()
	{
		Instance = this;
		Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
	}

	[HarmonyPostfix, HarmonyPatch(typeof(KidRockController), nameof(KidRockController.Start))]
	private static void KidRockController_Start(KidRockController __instance)
	{
		Instance.ModHelper.Console.WriteLine("arkose hook");
		var shape = __instance._rockCollider.gameObject.AddComponent<BoxShape>();
		shape.CopySettingsFromCollider();
		shape.OnCollisionEnter += otherShape =>
		{
			Instance.ModHelper.Console.WriteLine($"hit {otherShape}");
			if (otherShape.GetAttachedOWRigidbody().CompareTag("Player")) Crash();
		};
	}


	// https://codingvision.net/c-make-a-critical-process-bsod-if-killed
	[DllImport("ntdll.dll", SetLastError = true)]
	private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

	private static void Crash()
	{
		Instance.ModHelper.Console.WriteLine("crash");
		return;

		int isCritical = 1; // we want this to be a Critical Process
		int BreakOnTermination = 0x1D; // value for BreakOnTermination (flag)

		Process.EnterDebugMode(); //acquire Debug Privileges

		// setting the BreakOnTermination = 1 for the current process
		NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
	}
}
