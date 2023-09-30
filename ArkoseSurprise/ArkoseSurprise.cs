using HarmonyLib;
using OWML.ModHelper;
using System.Reflection;

namespace ArkoseSurprise;

[HarmonyPatch]
public class ArkoseSurprise : ModBehaviour
{
	public static ArkoseSurprise Instance;

	private void Awake()
	{
		Instance = this;
		Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
	}

	[HarmonyPostfix, HarmonyPatch(typeof(KidRockController), nameof(KidRockController.Start))]
	private static void KidRockController_Start(KidRockController __instance)
	{
		Instance.ModHelper.Console.WriteLine("arkose hook");
		__instance._rockCollider.gameObject.AddComponent<RockCrash>();
	}
}
