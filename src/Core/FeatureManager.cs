using UnityEngine;
using GnomeCheat.Utils;

namespace GnomeCheat.Core
{
    public class FeatureManager
    {
        public bool GodModeEnabled;
        public bool FlyModeEnabled;
        public float FlySpeed = 10f;
        public bool AntiKidnapEnabled;
        public bool InfiniteStaminaEnabled;
        public bool InfiniteHandsEnabled;
        public bool StrongArmsEnabled;
        public bool NoRagdollEnabled;
        public bool InvisibilityEnabled;
        public bool StalkModeEnabled;
        public int StalkTargetIndex = -1;
        public bool BobDisguiseEnabled;
        public bool MoleDisguiseEnabled;
        public bool NoclipEnabled;
        public float SpeedMultiplier = 1f;

        private bool disguiseWasActive;
        private bool invisWasActive;
        private float originalBaseSpeed = -1f;
        private float originalBoostSpeed = -1f;

        public void OnUpdate()
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            UpdateGodMode(local);
            UpdateInfiniteHands(local);
            UpdateStrongArms(local);
            UpdateNoRagdoll(local);
            UpdateInvisibility(local);
            UpdateFlyNoclip(local);
            UpdateAntiKidnap(local);
            UpdateInfiniteStamina(local);
            UpdateSpeed(local);
            UpdateStalkMode();
            UpdateDisguise(local);
        }

        private void UpdateGodMode(PlayerNetworking local)
        {
            if (!GodModeEnabled) return;
            if (local.Health != null)
                local.Health.Respawn();
        }

        private void UpdateInfiniteHands(PlayerNetworking local)
        {
            if (!InfiniteHandsEnabled) return;

            PlayerHandController hc = local.GetComponent<PlayerHandController>();
            if (hc == null) hc = local.GetComponentInChildren<PlayerHandController>(true);
            if (hc == null)
            {
                Transform parent = local.transform.parent;
                if (parent != null) hc = parent.GetComponentInChildren<PlayerHandController>(true);
            }
            if (hc == null) { GnomeCheatMod.LogError("PlayerHandController not found anywhere!"); return; }

            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;

            if (!infiniteHandsDebugLogged)
            {
                infiniteHandsDebugLogged = true;
                var allFields = typeof(PlayerHandController).GetFields(flags);
                foreach (var f in allFields)
                {
                    if (f.Name.Contains("hand") || f.Name.Contains("Hand") || f.Name.Contains("reach") || f.Name.Contains("Reach") || f.Name.Contains("stretch") || f.Name.Contains("Stretch") || f.Name.Contains("target") || f.Name.Contains("break") || f.Name.Contains("Break"))
                        GnomeCheatMod.Log($"[HandField] {f.Name} = {f.GetValue(hc)} ({f.FieldType})");
                }
            }

            SetFieldSafe(hc, "maxHandStretchDistance", 999f, flags);
            SetFieldSafe(hc, "handReachDistance", 2f, flags);
            SetFieldSafe(hc, "handBreakDistance", 999999f, flags);
            // SetFieldSafe(hc, "targetReach", 999f, flags);
            SetFieldSafe(hc, "overStretchDuration", 0f, flags);
            SetFieldSafe(hc, "handStretchSpeed", 1f, flags);
        }

        private bool infiniteHandsDebugLogged;

        private void SetFieldSafe(object target, string fieldName, object value, System.Reflection.BindingFlags flags)
        {
            var field = target.GetType().GetField(fieldName, flags);
            if (field != null)
                field.SetValue(target, value);
            else
                GnomeCheatMod.LogError($"Field '{fieldName}' NOT FOUND on {target.GetType().Name}!");
        }

        private void UpdateStrongArms(PlayerNetworking local)
        {
            if (!StrongArmsEnabled) return;

            PlayerHandController hc = local.GetComponent<PlayerHandController>();
            if (hc == null) hc = local.GetComponentInChildren<PlayerHandController>(true);
            if (hc == null)
            {
                Transform parent = local.transform.parent;
                if (parent != null) hc = parent.GetComponentInChildren<PlayerHandController>(true);
            }
            if (hc == null) return;

            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            typeof(PlayerHandController).GetField("debugOverridePushJointForces", flags)?.SetValue(hc, true);

            // Max joint forces
            hc.spring = 500000f;
            hc.damping = 5000f;
            hc.rotSpring = 500000f;
            hc.rotDamp = 5000f;

            // Reduce mass of currently held object
            var activePush = hc.ActivePushObject;
            if (activePush != null && activePush.Rb != null)
            {
                activePush.Rb.mass = 0.001f;
                activePush.Rb.linearDamping = 0f;
                activePush.Rb.angularDamping = 0f;
            }
        }

        private void UpdateNoRagdoll(PlayerNetworking local)
        {
            if (!NoRagdollEnabled) return;
            local.ForceNotRagdoll();
        }

        private void UpdateInvisibility(PlayerNetworking local)
        {
            if (InvisibilityEnabled)
            {
                invisWasActive = true;

                SetRenderersEnabled(local, false);

                var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                typeof(PlayerNetworking).GetField("godMode", flags)?.SetValue(local, true);

                Transform parent = local.transform.parent;
                if (parent != null)
                {
                    var np = parent.GetComponent<RootMotion.Dynamics.NetworkPuppet>();
                    if (np != null)
                    {
                        var targetRootField = typeof(RootMotion.Dynamics.NetworkPuppet).GetField("targetRoot", flags);
                        if (targetRootField != null)
                        {
                            Transform targetRoot = (Transform)targetRootField.GetValue(np);
                            if (targetRoot != null)
                                targetRoot.localScale = Vector3.zero;
                        }
                    }
                }

                if (local.Puppet != null && local.Puppet.puppetMaster != null)
                {
                    foreach (var muscle in local.Puppet.puppetMaster.muscles)
                    {
                        if (muscle.rigidbody != null)
                            muscle.rigidbody.transform.localScale = Vector3.zero;
                    }
                }
            }
            else if (invisWasActive)
            {
                invisWasActive = false;

                SetRenderersEnabled(local, true);

                if (!GodModeEnabled)
                {
                    var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                    typeof(PlayerNetworking).GetField("godMode", flags)?.SetValue(local, false);
                }

                Transform parent = local.transform.parent;
                if (parent != null)
                {
                    var np = parent.GetComponent<RootMotion.Dynamics.NetworkPuppet>();
                    if (np != null)
                    {
                        var flags2 = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                        var targetRootField = typeof(RootMotion.Dynamics.NetworkPuppet).GetField("targetRoot", flags2);
                        if (targetRootField != null)
                        {
                            Transform targetRoot = (Transform)targetRootField.GetValue(np);
                            if (targetRoot != null)
                                targetRoot.localScale = Vector3.one;
                        }
                    }
                }

                if (local.Puppet != null && local.Puppet.puppetMaster != null)
                {
                    foreach (var muscle in local.Puppet.puppetMaster.muscles)
                    {
                        if (muscle.rigidbody != null)
                            muscle.rigidbody.transform.localScale = Vector3.one;
                    }
                }
            }
        }

        private void SetRenderersEnabled(PlayerNetworking player, bool enabled)
        {
            Renderer[] allRenderers = player.GetComponentsInChildren<Renderer>(true);
            if (allRenderers != null)
            {
                foreach (Renderer r in allRenderers)
                {
                    if (r != null)
                    {
                        r.enabled = enabled;
                        r.shadowCastingMode = enabled ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                }
            }

            if (player.Puppet != null && player.Puppet.puppetMaster != null)
            {
                Transform puppetRoot = player.Puppet.puppetMaster.transform;
                if (puppetRoot != null)
                {
                    Renderer[] puppetRenderers = puppetRoot.GetComponentsInChildren<Renderer>(true);
                    if (puppetRenderers != null)
                    {
                        foreach (Renderer r in puppetRenderers)
                        {
                            if (r != null)
                            {
                                r.enabled = enabled;
                                r.shadowCastingMode = enabled ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
                            }
                        }
                    }
                }
            }

            Transform parent = player.transform.parent;
            if (parent != null)
            {
                Renderer[] parentRenderers = parent.GetComponentsInChildren<Renderer>(true);
                if (parentRenderers != null)
                {
                    foreach (Renderer r in parentRenderers)
                    {
                        if (r != null)
                        {
                            r.enabled = enabled;
                            r.shadowCastingMode = enabled ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
                        }
                    }
                }
            }
        }

        private void UpdateAntiKidnap(PlayerNetworking local)
        {
            if (!AntiKidnapEnabled) return;

            local.ApplyEffect(GameEntityBase.EffectType.Untie, 0f);

            Spider spider = Object.FindFirstObjectByType<Spider>();
            if (spider != null && spider.CurrentlyHeld == local)
                spider.CurrentlyHeld = null;

            HumanAILink human = Object.FindFirstObjectByType<HumanAILink>();
            if (human != null && human.PlayerInHand == local)
                human.ReleasePlayer();

            VacuumAiLink vacuum = Object.FindFirstObjectByType<VacuumAiLink>();
            if (vacuum != null)
            {
                PlayerMultiKidnapper mk = vacuum.GetComponent<PlayerMultiKidnapper>();
                mk?.RemoveAllKidnapped();
            }
        }

        private void UpdateInfiniteStamina(PlayerNetworking local)
        {
            if (!InfiniteStaminaEnabled) return;

            var charActor = local.GetComponentInChildren<Lightbug.CharacterControllerPro.Core.CharacterActor>();
            if (charActor == null) return;

            var field = typeof(Lightbug.CharacterControllerPro.Core.CharacterActor)
                .GetField("currentStamina", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(charActor, charActor.maxStamina);
        }

        private void UpdateSpeed(PlayerNetworking local)
        {
            NormalMovement nm = local.GetComponentInChildren<NormalMovement>();
            if (nm == null) return;

            if (originalBaseSpeed < 0f)
            {
                originalBaseSpeed = nm.planarMovementParameters.baseSpeedLimit;
                originalBoostSpeed = nm.planarMovementParameters.boostSpeedLimit;
            }

            nm.planarMovementParameters.baseSpeedLimit = originalBaseSpeed * SpeedMultiplier;
            nm.planarMovementParameters.boostSpeedLimit = originalBoostSpeed * SpeedMultiplier;
        }

        private void UpdateStalkMode()
        {
            if (!StalkModeEnabled || StalkTargetIndex < 0) return;

            var allPlayers = PlayerHelper.GetAllPlayers();
            if (StalkTargetIndex >= allPlayers.Count) return;

            PlayerNetworking target = allPlayers[StalkTargetIndex];
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (target != null && local != null && !target.IsLocalPlayer)
                local.Teleport(target.Position + Vector3.up * 2.5f);
        }

        private void UpdateDisguise(PlayerNetworking local)
        {
            bool anyDisguise = BobDisguiseEnabled || MoleDisguiseEnabled;

            if (anyDisguise)
            {
                disguiseWasActive = true;

                SetRenderersEnabled(local, false);

                var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                typeof(PlayerNetworking).GetField("godMode", flags)?.SetValue(local, true);

                if (BobDisguiseEnabled)
                {
                    Bob bob = Object.FindFirstObjectByType<Bob>();
                    if (bob != null)
                    {
                        bob.transform.position = local.Position;
                        bob.transform.rotation = local.transform.rotation;
                    }
                }

                if (MoleDisguiseEnabled)
                {
                    MoleAiLink mole = Object.FindFirstObjectByType<MoleAiLink>();
                    if (mole != null)
                    {
                        mole.transform.position = local.Position;
                        mole.transform.rotation = local.transform.rotation;
                        Transform gfx = mole.GetComponentInChildren<Renderer>()?.transform?.parent;
                        if (gfx != null)
                        {
                            Vector3 lp = gfx.localPosition;
                            lp.y = 0f;
                            gfx.localPosition = lp;
                        }
                    }
                }
            }
            else if (disguiseWasActive)
            {
                disguiseWasActive = false;

                if (!InvisibilityEnabled)
                {
                    SetRenderersEnabled(local, true);

                    if (!GodModeEnabled)
                    {
                        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                        typeof(PlayerNetworking).GetField("godMode", flags)?.SetValue(local, false);
                    }
                }
            }
        }

        private Lightbug.CharacterControllerPro.Core.CharacterActor savedActor;
        private bool flyNoclipActive;
        private int noclipOriginalLayer = -1;
        private System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<GameObject, int>> noclipSavedLayers;

        public void ToggleFlyMode(bool enabled)
        {
            FlyModeEnabled = enabled;
            SetFlyNoclipState(enabled);
            GnomeCheatMod.Log($"Fly Mode: {enabled}");
        }

        public void ToggleNoclip(bool enabled)
        {
            NoclipEnabled = enabled;

            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            if (enabled)
            {
                noclipSavedLayers = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<GameObject, int>>();

                void SaveAndSetLayer(GameObject go)
                {
                    if (go == null) return;
                    noclipSavedLayers.Add(new System.Collections.Generic.KeyValuePair<GameObject, int>(go, go.layer));
                    go.layer = LayerMask.NameToLayer("Ignore Raycast");
                }

                SaveAndSetLayer(local.gameObject);
                foreach (Collider c in local.GetComponentsInChildren<Collider>(true))
                    if (c != null) SaveAndSetLayer(c.gameObject);

                Transform parent = local.transform.parent;
                if (parent != null)
                {
                    foreach (Collider c in parent.GetComponentsInChildren<Collider>(true))
                        if (c != null) SaveAndSetLayer(c.gameObject);
                }
            }
            else
            {
                if (noclipSavedLayers != null)
                {
                    foreach (var kvp in noclipSavedLayers)
                        if (kvp.Key != null) kvp.Key.layer = kvp.Value;
                    noclipSavedLayers = null;
                }
            }

            GnomeCheatMod.Log($"Noclip: {enabled}");
        }

        private void SetFlyNoclipState(bool enabled)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            flyNoclipActive = enabled || FlyModeEnabled || NoclipEnabled;

            if (enabled)
                local.ForceNotRagdoll();

            var actor = local.Actor;
            if (actor != null)
                actor.enabled = !flyNoclipActive;

            var stateController = local.StateController;
            if (stateController != null)
                stateController.enabled = !flyNoclipActive;

            var rb = local.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = !flyNoclipActive;
                if (flyNoclipActive)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            if (local.Puppet?.puppetMaster != null)
            {
                foreach (var muscle in local.Puppet.puppetMaster.muscles)
                {
                    if (muscle.rigidbody != null)
                    {
                        muscle.rigidbody.useGravity = !flyNoclipActive;
                        if (flyNoclipActive)
                        {
                            muscle.rigidbody.linearVelocity = Vector3.zero;
                            muscle.rigidbody.angularVelocity = Vector3.zero;
                        }
                    }
                }
            }
        }

        private void UpdateFlyNoclip(PlayerNetworking local)
        {
            if (!FlyModeEnabled) return;

            Transform cam = Camera.main?.transform;
            if (cam == null) return;

            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) dir += cam.forward;
            if (Input.GetKey(KeyCode.S)) dir -= cam.forward;
            if (Input.GetKey(KeyCode.A)) dir -= cam.right;
            if (Input.GetKey(KeyCode.D)) dir += cam.right;
            if (Input.GetKey(KeyCode.Space)) dir += Vector3.up;
            if (Input.GetKey(KeyCode.LeftControl)) dir += Vector3.down;

            float speed = FlySpeed * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f);

            local.ForceNotRagdoll();

            var rb = local.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (local.Puppet?.puppetMaster != null)
            {
                foreach (var muscle in local.Puppet.puppetMaster.muscles)
                {
                    if (muscle.rigidbody != null)
                    {
                        muscle.rigidbody.linearVelocity = Vector3.zero;
                        muscle.rigidbody.angularVelocity = Vector3.zero;
                    }
                }
            }

            if (dir != Vector3.zero)
            {
                Vector3 move = dir.normalized * speed * Time.deltaTime;
                local.transform.position += move;

                Transform parent = local.transform.parent;
                if (parent != null)
                    parent.position += move;
            }
        }

    }
}