using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using LudeonTK;
using RimWorld;
using Verse;

namespace StartsFromScratch
{
    [StaticConstructorOnStartup]
    public static class StartsFromScratch
    {
        static StartsFromScratch()
        {
            var harmony = new Harmony("CrazyDadong.StartsFromScratch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            
        }

        [DebugAction("StartsFromScratch", "Clear", actionType = DebugActionType.Action)]
        public static void Clear()
        {
            var researchManager = Find.ResearchManager;

            // 清空研究进度
            var progressField =
                typeof(ResearchManager).GetField("progress", BindingFlags.Instance | BindingFlags.NonPublic);
            var progress = (Dictionary<ResearchProjectDef, float>)progressField?.GetValue(researchManager);
            progress?.Clear();

            // 清空技术蓝图
            var techPrintsField =
                typeof(ResearchManager).GetField("techprints", BindingFlags.Instance | BindingFlags.NonPublic);
            var techPrints = (Dictionary<ResearchProjectDef, int>)techPrintsField?.GetValue(researchManager);
            techPrints?.Clear();
        }
    }
    
    // 补丁类，修补 Game 类的 InitNewGame 方法
    [HarmonyPatch(typeof(Game), "InitNewGame")]
    public static class GameInitNewGamePatch
    {
        // 在 InitNewGame 方法之后执行的代码
        public static void Postfix()
        {
            StartsFromScratch.Clear();
        }
    }
}