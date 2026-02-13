using UnityEngine;
using GnomeCheat.Core;
using GnomeCheat.Utils;

namespace GnomeCheat.Actions
{
    public static class NPCActions
    {
        // === BOB ===
        public static void TeleportBobToMe()
        {
            Bob bob = Object.FindFirstObjectByType<Bob>();
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (bob != null && player != null)
            {
                bob.transform.position = player.Position + player.transform.forward * 3f;
                GnomeCheatMod.Log("Teleported Bob to you");
            }
            else GnomeCheatMod.LogError("Bob not found!");
        }

        public static void TeleportBobAway()
        {
            Bob bob = Object.FindFirstObjectByType<Bob>();
            if (bob != null) { bob.transform.position = new Vector3(0, -1000f, 0); GnomeCheatMod.Log("Teleported Bob away"); }
            else GnomeCheatMod.LogError("Bob not found!");
        }

        public static void KillBob()
        {
            Bob bob = Object.FindFirstObjectByType<Bob>();
            if (bob != null) { Object.Destroy(bob.gameObject); GnomeCheatMod.Log("Killed Bob"); }
            else GnomeCheatMod.LogError("Bob not found!");
        }

        public static void MakeBobDrop()
        {
            Bob bob = Object.FindFirstObjectByType<Bob>();
            if (bob != null)
            {
                if (bob.CarriedStealable != null) { bob.CarriedStealable = null; GnomeCheatMod.Log("Bob dropped his item"); }
                else GnomeCheatMod.Log("Bob is not carrying anything");
            }
            else GnomeCheatMod.LogError("Bob not found!");
        }

        // === HUMAN ===
        public static void TeleportHumanToMe()
        {
            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (human != null && player != null)
            {
                human.transform.position = player.Position + player.transform.forward * 3f;
                GnomeCheatMod.Log("Teleported Human to you");
            }
            else GnomeCheatMod.LogError("Human not found!");
        }

        public static void TeleportHumanAway()
        {
            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null) { human.transform.position = new Vector3(0, -1000f, 0); GnomeCheatMod.Log("Teleported Human away"); }
            else GnomeCheatMod.LogError("Human not found!");
        }

        public static void KillHuman()
        {
            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null)
            {
                HealthBase health = human.GetComponent<HealthBase>();
                if (health != null) { health.TakeDamage(999f); GnomeCheatMod.Log("Killed Human"); }
            }
            else GnomeCheatMod.LogError("Human not found!");
        }

        public static void HumanReleasePlayer()
        {
            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null)
            {
                if (human.PlayerInHand != null) { human.ReleasePlayer(); GnomeCheatMod.Log("Human released player"); }
                else GnomeCheatMod.Log("Human is not holding anyone");
            }
            else GnomeCheatMod.LogError("Human not found!");
        }

        public static void HumanDropGun()
        {
            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null)
            {
                if (human.HasGun) { human.DropGun(); GnomeCheatMod.Log("Human dropped gun"); }
                else GnomeCheatMod.Log("Human doesn't have a gun");
            }
            else GnomeCheatMod.LogError("Human not found!");
        }

        public static void MakeHumanNaked()
        {
            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null) { human.SetNaked(!human.Naked); GnomeCheatMod.Log($"Human naked: {human.Naked}"); }
            else GnomeCheatMod.LogError("Human not found!");
        }

        // === CAT ===
        public static void TeleportCatToMe()
        {
            CatAiLink cat = Object.FindFirstObjectByType<CatAiLink>();
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (cat != null && player != null)
            {
                cat.transform.position = player.Position + player.transform.forward * 2f;
                GnomeCheatMod.Log("Teleported Cat to you");
            }
            else GnomeCheatMod.LogError("Cat not found!");
        }

        public static void CatAttackPlayer(PlayerNetworking target)
        {
            if (target == null) return;
            CatAiLink cat = Object.FindFirstObjectByType<CatAiLink>();
            if (cat != null)
            {
                cat.transform.position = target.Position + target.transform.forward * 2f;
                cat.anger = 100f;
                cat.lookAtTarget = target;
                GnomeCheatMod.Log("Cat attacking player!");
            }
            else GnomeCheatMod.LogError("Cat not found!");
        }

        public static async void SpamCatMeow()
        {
            CatAiLink cat = Object.FindFirstObjectByType<CatAiLink>();
            if (cat == null) { GnomeCheatMod.LogError("Cat not found!"); return; }
            for (int i = 0; i < 15; i++)
            {
                cat.PlayMeowRpc((CatAiLink.MeowType)Random.Range(1, 3));
                await System.Threading.Tasks.Task.Delay(150);
            }
            GnomeCheatMod.Log("Spam meow done!");
        }

        public static async void SpamCatHiss()
        {
            CatAiLink cat = Object.FindFirstObjectByType<CatAiLink>();
            if (cat == null) { GnomeCheatMod.LogError("Cat not found!"); return; }
            for (int i = 0; i < 15; i++)
            {
                cat.PlayMeowRpc(CatAiLink.MeowType.HISS);
                await System.Threading.Tasks.Task.Delay(200);
            }
            GnomeCheatMod.Log("Spam hiss done!");
        }

        // === SPIDER ===
        public static void TeleportSpiderToMe()
        {
            Spider spider = Object.FindFirstObjectByType<Spider>();
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (spider != null && player != null)
            {
                spider.transform.position = player.Position + player.transform.forward * 3f;
                GnomeCheatMod.Log("Teleported Spider to you");
            }
            else GnomeCheatMod.LogError("Spider not found!");
        }

        public static void TeleportMeToSpider()
        {
            Spider spider = Object.FindFirstObjectByType<Spider>();
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (spider != null && player != null)
                player.Teleport(spider.transform.position + Vector3.up * 2f);
        }

        public static void KillSpider()
        {
            Spider spider = Object.FindFirstObjectByType<Spider>();
            if (spider != null) { Object.Destroy(spider.gameObject); GnomeCheatMod.Log("Killed Spider"); }
            else GnomeCheatMod.LogError("Spider not found!");
        }

        public static void SpiderRelease()
        {
            Spider spider = Object.FindFirstObjectByType<Spider>();
            if (spider != null)
            {
                if (spider.CurrentlyHeld != null) { spider.CurrentlyHeld = null; GnomeCheatMod.Log("Spider released player"); }
                else GnomeCheatMod.Log("Spider is not holding anyone");
            }
            else GnomeCheatMod.LogError("Spider not found!");
        }

        // === VACUUM ===
        public static void TeleportVacuumToMe()
        {
            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (vacuum != null && player != null)
            {
                vacuum.transform.position = player.Position + player.transform.forward * 3f;
                GnomeCheatMod.Log("Teleported Vacuum to you");
            }
            else GnomeCheatMod.LogError("Vacuum not found!");
        }

        public static void TeleportMeToVacuum()
        {
            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (vacuum != null && player != null)
                player.Teleport(vacuum.transform.position + Vector3.up * 2f);
            else GnomeCheatMod.LogError("Vacuum not found!");
        }

        public static void ToggleVacuum()
        {
            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            if (vacuum != null) { vacuum.Interact(PlayerHelper.GetLocalPlayer()); GnomeCheatMod.Log("Toggled Vacuum"); }
            else GnomeCheatMod.LogError("Vacuum not found!");
        }

        public static void VacuumReleaseAll()
        {
            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            if (vacuum != null)
            {
                vacuum.GetComponent<PlayerMultiKidnapper>()?.RemoveAllKidnapped();
                GnomeCheatMod.Log("Vacuum released all players");
            }
            else GnomeCheatMod.LogError("Vacuum not found!");
        }

        // === FIND ALL ===
        public static void FindAllNPCs()
        {
            GnomeCheatMod.Log("===== SEARCHING FOR NPCs =====");

            Bob bob = Object.FindFirstObjectByType<Bob>();
            if (bob != null)
            {
                GnomeCheatMod.Log($"Bob found at: {bob.transform.position}");
                if (bob.CarriedStealable != null)
                    GnomeCheatMod.Log($"  - Carrying: {bob.CarriedStealable.name}");
            }

            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null)
            {
                GnomeCheatMod.Log($"Human found at: {human.transform.position}");
                GnomeCheatMod.Log($"  - Has Gun: {human.HasGun}");
                GnomeCheatMod.Log($"  - Naked: {human.Naked}");
                if (human.PlayerInHand != null) GnomeCheatMod.Log("  - Holding player");
            }

            GnomeCheatMod.Log("===== END SEARCH =====");
        }
    }
}
