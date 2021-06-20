using CrowdedRoles.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdedRoles.CmdsRoles.Patches
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
    class EndGameConditionsPatch
    {
        public static void Prefix(ShipStatus __instance)
        {
			if (!GameData.Instance)
			{
				return;
			}
			if (__instance.Systems.ContainsKey(SystemTypes.LifeSupp))
			{
				LifeSuppSystemType lifeSuppSystemType = __instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
				if (lifeSuppSystemType.Countdown < 0f)
				{
					__instance.EndGameForSabotage();
					lifeSuppSystemType.Countdown = 10000f;
				}
			}
			ISystemType systemType2;

			if ((__instance.Systems.ContainsKey(SystemTypes.Reactor) &&
				 (systemType2 = __instance.Systems[SystemTypes.Reactor]) != null || __instance.Systems.ContainsKey(SystemTypes.Laboratory) && (systemType2 = __instance.Systems[SystemTypes.Laboratory]) != null) && systemType2.TryCast<ICriticalSabotage>() != null)
			{
				ICriticalSabotage criticalSabotage = systemType2.Cast<ICriticalSabotage>();
				if (criticalSabotage.Countdown < 0f)
				{
					__instance.EndGameForSabotage();
					criticalSabotage.ClearSabotage();
				}
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < GameData.Instance.PlayerCount; i++)
			{
				GameData.PlayerInfo playerInfo = GameData.Instance.AllPlayers[i];
				if (!playerInfo.Disconnected)
				{
					if (playerInfo.IsImpostor && !playerInfo.Is<Copier>())
					{
						num3++;
					}
					if (!playerInfo.IsDead)
					{
						if (playerInfo.IsImpostor)
						{
							num2++;
						}
						else
						{
							num++;
						}
					}
				}
			}
			if (num2 <= 0 && (!DestroyableSingleton<TutorialManager>.InstanceExists || num3 > 0))
			{
				if (!DestroyableSingleton<TutorialManager>.InstanceExists)
				{
					__instance.enabled = false;
					ShipStatus.RpcEndGame(GameOverReason.HumansByVote, !SaveManager.BoughtNoAds);
					return;
				}
				DestroyableSingleton<HudManager>.Instance.ShowPopUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameOverImpostorDead, (UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>)Array.Empty<Object>()));
				ShipStatus.ReviveEveryone();
				return;
			}
			else
			{
				if (num > num2)
				{
					List<PlayerTask> Tasks = new List<PlayerTask>();
					Tasks.Clear();
					foreach (PlayerTask i in PlayerControl.LocalPlayer.myTasks)
                    {
						Tasks.Add(i);
                    }
					if (!DestroyableSingleton<TutorialManager>.InstanceExists)
					{
						if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks)
						{
							__instance.enabled = false;
							ShipStatus.RpcEndGame(GameOverReason.HumansByTask, !SaveManager.BoughtNoAds);
							return;
						}
					}
					else if (Tasks.TrueForAll((PlayerTask t) => t.IsComplete))
					{
						DestroyableSingleton<HudManager>.Instance.ShowPopUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameOverTaskWin, (UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>)Array.Empty<object>()));
						__instance.Begin();
					}
					return;
				}
				if (!DestroyableSingleton<TutorialManager>.InstanceExists)
				{
					__instance.enabled = false;
					GameOverReason endReason;
					switch (TempData.LastDeathReason)
					{
						case DeathReason.Exile:
							endReason = GameOverReason.ImpostorByVote;
							break;
						case DeathReason.Kill:
							endReason = GameOverReason.ImpostorByKill;
							break;
						default:
							endReason = GameOverReason.ImpostorByVote;
							break;
					}
					ShipStatus.RpcEndGame(endReason, !SaveManager.BoughtNoAds);
					return;
				}
				DestroyableSingleton<HudManager>.Instance.ShowPopUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameOverImpostorKills, (UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>)Array.Empty<object>()));
				ShipStatus.ReviveEveryone();
				return;
			}
		}
    }
}
