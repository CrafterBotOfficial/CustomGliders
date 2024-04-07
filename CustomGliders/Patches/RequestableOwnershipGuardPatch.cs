using HarmonyLib;
using Photon.Realtime;

namespace CustomGliders.Patches
{
    [HarmonyPatch(typeof(RequestableOwnershipGuard), "SetCurrentOwner", [typeof(Player)])]
    public static class RequestableOwnershipGuardPatch
    {
        [HarmonyPostfix]
        [HarmonyWrapSafe]
        private static void SetCurrentOwner_Postfix(RequestableOwnershipGuard __instance, Player player)
        {
            if (__instance.gameObject.GetComponent<GliderHoldable>() is GliderHoldable holdable && !player.IsLocal && player.CustomProperties.TryGetValue(SkinManager.GliderSkinKey, out object skinId))
            {
                Main.LogSource.LogInfo($"Player {player.NickName} has skinid {skinId}");
                if (SkinManager.Instance.Skins.ContainsKey(skinId as string ?? string.Empty))
                {
                    Main.LogSource.LogMessage("We have this skin! Equiping now.");
                    SkinManager.Instance.EquipSkin(player, skinId as string);
                }
            }
        }
    }
}
