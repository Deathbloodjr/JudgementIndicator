using Blittables;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgementIndicator.Patches
{
    internal class JudgementIndicatorPatch
    {
        static float totalOffset = 0.0f;
        static int totalInputs = 0;
        static float previousTime = 0.0f;

        static OnpuStatus previousOnpu1P;
        static OnpuStatus previousOnpu2P;

        [HarmonyPatch(typeof(EnsoGameManager), "ProcExecMain")]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void EnsoGameManager_ProcExecMain_Postfix(EnsoGameManager __instance)
        {
            var frameResults = __instance.ensoParam.GetFrameResults();
            int num = 0;
            foreach (HitResultInfo info in frameResults.hitResultInfo)
            {
                if (num >= frameResults.hitResultInfoNum)
                {
                    break;
                }
                num++;
                if (info.hasOnpu != 0 && info.player == 0 && info.hitResult > -2 && info.hitResult != -1 && info.hitResult < 3)
                {
                    if (info.onpuType != 6 && info.onpuType != 9 && info.onpuType != 10 && info.onpuType != 11 && info.onpuType != 12)
                    {
                        previousOnpu1P = info.onpu;
                    }
                }
                else if (info.hasOnpu != 0 && info.player == 1 && info.hitResult > -2 && info.hitResult != -1 && info.hitResult < 3)
                {
                    if (info.onpuType != 6 && info.onpuType != 9 && info.onpuType != 10 && info.onpuType != 11 && info.onpuType != 12)
                    {
                        previousOnpu2P = info.onpu;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(HitEffect), "playAnimationHitKa")]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static void HitEffect_playAnimationHitKa_Prefix(HitEffect __instance)
        {
            if (!__instance.checkAnimationClipHitEffect())
            {
                return;
            }
            int num = (int)__instance.frameResults.gameDrawInfoNum[__instance.playerNo];
            for (int i = num - 1; i >= 0; i--)
            {
                GameDrawInfo gameDrawInfo = __instance.frameResults.GetGameDrawInfo(__instance.playerNo, i);
                if ((gameDrawInfo.onpu.justTime == previousOnpu1P.justTime && __instance.playerNo == 0) ||
                    (gameDrawInfo.onpu.justTime == previousOnpu2P.justTime && __instance.playerNo == 1))
                {
                    if (gameDrawInfo.pos > 0.1f)
                    {
                        continue;
                    }
                    var newPosition = __instance.AnimationClipsHitEffect[4].gameObject.transform.position;
                    newPosition.x = gameDrawInfo.pos * 1422f * 1 + -342f;

                    __instance.AnimationClipsHitEffect[1].gameObject.transform.SetPositionAndRotation(newPosition, UnityEngine.Quaternion.identity);
                }
            }
        }

        [HarmonyPatch(typeof(HitEffect), "playAnimationHitKaBig")]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static void HitEffect_playAnimationHitKaBig_Prefix(HitEffect __instance)
        {
            if (!__instance.checkAnimationClipHitEffect())
            {
                return;
            }
            int num = (int)__instance.frameResults.gameDrawInfoNum[__instance.playerNo];
            for (int i = num - 1; i >= 0; i--)
            {
                GameDrawInfo gameDrawInfo = __instance.frameResults.GetGameDrawInfo(__instance.playerNo, i);
                if ((gameDrawInfo.onpu.justTime == previousOnpu1P.justTime && __instance.playerNo == 0) ||
                    (gameDrawInfo.onpu.justTime == previousOnpu2P.justTime && __instance.playerNo == 1))
                {
                    if (gameDrawInfo.pos > 0.1f)
                    {
                        continue;
                    }
                    var newPosition = __instance.AnimationClipsHitEffect[4].gameObject.transform.position;
                    newPosition.x = gameDrawInfo.pos * 1422f * 1 + -342f;

                    __instance.AnimationClipsHitEffect[2].gameObject.transform.SetPositionAndRotation(newPosition, UnityEngine.Quaternion.identity);
                }
            }
        }
    }
}
