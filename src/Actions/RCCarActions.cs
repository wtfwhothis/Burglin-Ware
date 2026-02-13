using UnityEngine;
using Unity.Netcode;
using GnomeCheat.Core;
using GnomeCheat.Utils;

namespace GnomeCheat.Actions
{
    public static class RCCarActions
    {
        public static void TeleportToMe(RCCar car)
        {
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (car != null && player != null)
            {
                car.transform.position = player.Position + player.transform.forward * 3f;
                GnomeCheatMod.Log("Teleported RC Car to you");
            }
        }

        public static void TeleportMeTo(RCCar car)
        {
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (car != null && player != null)
            {
                player.Teleport(car.transform.position + Vector3.up * 2f);
                GnomeCheatMod.Log("Teleported you to RC Car");
            }
        }

        public static void LaunchUp(RCCar car)
        {
            if (car == null) return;
            SetRigidbodyProperty(car, "linearVelocity", Vector3.up * 30f);
            GnomeCheatMod.Log("Launched RC Car up");
        }

        public static void Flip(RCCar car)
        {
            if (car == null) return;
            car.transform.rotation = Quaternion.Euler(0, car.transform.eulerAngles.y, 0);
            GnomeCheatMod.Log("Flipped RC Car upright");
        }

        public static void Spin(RCCar car)
        {
            if (car == null) return;
            SetRigidbodyProperty(car, "angularVelocity", new Vector3(
                Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
            GnomeCheatMod.Log("Spinning RC Car");
        }

        public static void Freeze(RCCar car)
        {
            if (car == null) return;
            SetRigidbodyProperty(car, "linearVelocity", Vector3.zero);
            SetRigidbodyProperty(car, "angularVelocity", Vector3.zero);
            GnomeCheatMod.Log("Froze RC Car");
        }

        public static void SuperSpeed(RCCar car)
        {
            if (car == null) return;
            SetRigidbodyProperty(car, "linearVelocity", car.transform.forward * 50f);
            GnomeCheatMod.Log("Super speed RC Car!");
        }

        public static void Duplicate(RCCar car)
        {
            if (car == null) return;
            PlayerNetworking player = PlayerHelper.GetLocalPlayer();
            if (player == null) return;

            Vector3 pos = player.Position + player.transform.forward * 3f + Vector3.up * 0.5f;
            try
            {
                GameObject dup = Object.Instantiate(car.gameObject, pos, car.transform.rotation);
                NetworkObject netObj = dup.GetComponent<NetworkObject>();
                if (netObj != null) { netObj.Spawn(); GnomeCheatMod.Log("Duplicated RC Car!"); }
                else { Object.Destroy(dup); GnomeCheatMod.LogError("No NetworkObject!"); }
            }
            catch (System.Exception e) { GnomeCheatMod.LogError($"Duplicate failed: {e.Message}"); }
        }

        public static void EjectDriver(RCCar car)
        {
            if (car?.CurrentDriver != null)
            {
                car.CurrentDriver = null;
                GnomeCheatMod.Log("Ejected driver");
            }
        }

        public static void Delete(RCCar car)
        {
            if (car != null) { Object.Destroy(car.gameObject); GnomeCheatMod.Log("Deleted RC Car"); }
        }

        public static void LaunchAll()
        {
            var cars = PlayerHelper.GetAllRCCars();
            foreach (var car in cars) LaunchUp(car);
            GnomeCheatMod.Log($"Launched {cars.Count} RC Cars");
        }

        public static void SuperSpeedAll()
        {
            var cars = PlayerHelper.GetAllRCCars();
            foreach (var car in cars) SuperSpeed(car);
            GnomeCheatMod.Log($"Super speed {cars.Count} RC Cars");
        }

        public static void DeleteAll()
        {
            var cars = PlayerHelper.GetAllRCCars();
            int count = cars.Count;
            foreach (var car in cars) Object.Destroy(car.gameObject);
            GnomeCheatMod.Log($"Deleted {count} RC Cars");
        }

        // === RC CAR KAMIKAZE ===
        private static PlayerNetworking kamikazeTarget;
        private static RCCar kamikazeCar;
        private static bool kamikazeActive;

        public static bool IsKamikazeActive => kamikazeActive;

        public static void StartKamikaze(PlayerNetworking target)
        {
            if (target == null) return;

            RCCar[] allCars = Object.FindObjectsByType<RCCar>(FindObjectsSortMode.None);
            RCCar closest = null;
            float closestDist = float.MaxValue;

            foreach (var car in allCars)
            {
                if (car == null) continue;
                float dist = Vector3.Distance(car.transform.position, target.Position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = car;
                }
            }

            if (closest == null)
            {
                try
                {
                    PlayerNetworking local = PlayerHelper.GetLocalPlayer();
                    if (local == null) return;

                    GameObject prefab = null;
                    foreach (var obj in Resources.FindObjectsOfTypeAll<RCCar>())
                    {
                        if (obj != null) { prefab = obj.gameObject; break; }
                    }
                    if (prefab == null) { GnomeCheatMod.LogError("No RC Car prefab found"); return; }

                    Vector3 spawnPos = target.Position + Vector3.up * 0.5f - (target.Position - local.Position).normalized * 10f;
                    GameObject dup = Object.Instantiate(prefab, spawnPos, Quaternion.identity);
                    NetworkObject netObj = dup.GetComponent<NetworkObject>();
                    if (netObj != null) netObj.Spawn();
                    closest = dup.GetComponent<RCCar>();
                }
                catch (System.Exception e)
                {
                    GnomeCheatMod.LogError($"Kamikaze spawn failed: {e.Message}");
                    return;
                }
            }

            kamikazeTarget = target;
            kamikazeCar = closest;
            kamikazeActive = true;
            GnomeCheatMod.Log("RC Car Kamikaze launched!");
        }

        public static void StopKamikaze()
        {
            kamikazeActive = false;
            kamikazeTarget = null;
            kamikazeCar = null;
            GnomeCheatMod.Log("Kamikaze stopped");
        }

        public static void UpdateKamikaze()
        {
            if (!kamikazeActive || kamikazeTarget == null || kamikazeCar == null)
            {
                kamikazeActive = false;
                return;
            }

            Vector3 targetPos = kamikazeTarget.Position;
            Vector3 carPos = kamikazeCar.transform.position;
            Vector3 dir = (targetPos - carPos).normalized;
            float dist = Vector3.Distance(carPos, targetPos);

            kamikazeCar.transform.rotation = Quaternion.LookRotation(dir);

            Component rb = kamikazeCar.GetComponent(typeof(Rigidbody));
            if (rb != null)
            {
                var velProp = rb.GetType().GetProperty("linearVelocity");
                velProp?.SetValue(rb, dir * 40f);
            }

            if (dist < 2f)
            {
                kamikazeTarget.ForceRagdoll();
                kamikazeTarget.AddForceToRagdoll(Vector3.up * 30f + dir * 20f, ForceMode.VelocityChange);
                kamikazeTarget.AddExplosionForceToRagdollRpc(targetPos, 5f, 2000f);

                if (rb != null)
                {
                    var velProp = rb.GetType().GetProperty("linearVelocity");
                    velProp?.SetValue(rb, Vector3.up * 20f + Random.insideUnitSphere * 10f);
                    var angProp = rb.GetType().GetProperty("angularVelocity");
                    angProp?.SetValue(rb, Random.insideUnitSphere * 30f);
                }

                kamikazeActive = false;
                GnomeCheatMod.Log("KAMIKAZE HIT!");
            }
        }

        private static void SetRigidbodyProperty(RCCar car, string propName, Vector3 value)
        {
            Component rb = car.GetComponent(typeof(Rigidbody));
            if (rb == null) return;
            var prop = rb.GetType().GetProperty(propName);
            prop?.SetValue(rb, value);
        }
    }
}