using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GnomeCheat.Utils;

namespace GnomeCheat.ESP
{
    public class ESPRenderer
    {
        public ESPSettings Settings = new ESPSettings();

        private float lastUpdate;
        private List<PlayerNetworking> cachedPlayers = new List<PlayerNetworking>();
        private List<StealableObject> cachedItems = new List<StealableObject>();
        private Bob cachedBob;
        private HumanAILink cachedHuman;
        private Spider cachedSpider;

        private Texture2D espTex;
        private GUIStyle labelStyle;
        private bool stylesInit;

        public void Draw()
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            if (Time.time - lastUpdate > Settings.UpdateInterval)
            {
                UpdateCache();
                lastUpdate = Time.time;
            }

            InitStyles();

            if (Settings.PlayerEnabled) DrawPlayers(cam);
            if (Settings.NpcEnabled) DrawNpcs(cam);
            if (Settings.ItemEnabled) DrawItems(cam);
        }

        private void InitStyles()
        {
            if (stylesInit) return;
            espTex = new Texture2D(1, 1);
            espTex.SetPixel(0, 0, Color.white);
            espTex.Apply();

            labelStyle = new GUIStyle
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            stylesInit = true;
        }

        private void UpdateCache()
        {
            if (Settings.PlayerEnabled)
                cachedPlayers = PlayerHelper.GetAllPlayers();

            if (Settings.NpcEnabled)
            {
                if (Settings.NpcBob) cachedBob = Object.FindFirstObjectByType<Bob>();
                if (Settings.NpcHuman) cachedHuman = Object.FindFirstObjectByType<HumanAILink>();
                if (Settings.NpcSpider) cachedSpider = Object.FindFirstObjectByType<Spider>();
            }

            if (Settings.ItemEnabled)
                cachedItems = Object.FindObjectsByType<StealableObject>(FindObjectsSortMode.None).ToList();
        }

        private void DrawPlayers(Camera cam)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            foreach (PlayerNetworking player in cachedPlayers)
            {
                if (player == null) continue;

                if (!player.IsLocalPlayer)
                {
                    float dist = Vector3.Distance(local.Position, player.Position);
                    if (dist > Settings.MaxDistance) continue;

                    Vector3 headScreen = WorldToScreen(cam, player.Position + Vector3.up * 2f);
                    if (headScreen.z <= 0) continue;

                    Color col = Settings.ColorPlayerOther;

                    if (Settings.PlayerBoxes)
                    {
                        Vector3 feetScreen = WorldToScreen(cam, player.Position - Vector3.up * 0.1f);
                        float h = Mathf.Abs(feetScreen.y - headScreen.y);
                        float w = h * 0.4f;
                        DrawBox(new Rect(headScreen.x - w / 2f, headScreen.y, w, h), col);
                    }

                    if (Settings.PlayerNames || Settings.PlayerHealth || Settings.PlayerDistance)
                    {
                        string label = "";
                        if (Settings.PlayerNames) label = PlayerHelper.GetPlayerName(player);
                        if (Settings.PlayerHealth && player.Health != null) label += $"\n{player.Health.Health:F0} HP";
                        if (Settings.PlayerDistance) label += $"\n{dist:F0}m";
                        DrawLabel(headScreen, label, col);
                    }

                    if (Settings.PlayerTracers)
                        DrawLine(new Vector2(Screen.width / 2f, Screen.height), new Vector2(headScreen.x, headScreen.y), col, 1f);
                }
                else if (Settings.PlayerNames)
                {
                    Vector3 s = WorldToScreen(cam, player.Position + Vector3.up * 2f);
                    if (s.z > 0) DrawLabel(s, "[YOU]", Settings.ColorPlayerLocal);
                }
            }
        }

        private void DrawNpcs(Camera cam)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            if (Settings.NpcBob && cachedBob != null)
                DrawNpcMarker(cam, local, cachedBob.transform.position, "Bob", Settings.ColorBob, 1.5f);

            if (Settings.NpcHuman && cachedHuman != null)
            {
                string label = "Human";
                if (cachedHuman.HasGun) label += " [GUN]";
                if (cachedHuman.PlayerInHand != null) label += " [HOLD]";
                DrawNpcMarker(cam, local, cachedHuman.transform.position, label, Settings.ColorHuman, 2f);
            }

            if (Settings.NpcSpider && cachedSpider != null)
            {
                string label = "Spider";
                if (cachedSpider.CurrentlyHeld != null) label += " [HOLD]";
                DrawNpcMarker(cam, local, cachedSpider.transform.position, label, Settings.ColorSpider, 1f);
            }
        }

        private void DrawNpcMarker(Camera cam, PlayerNetworking local, Vector3 worldPos, string name, Color col, float height)
        {
            float dist = Vector3.Distance(local.Position, worldPos);
            if (dist > Settings.MaxDistance) return;

            Vector3 topScreen = WorldToScreen(cam, worldPos + Vector3.up * (height + 0.5f));
            if (topScreen.z <= 0) return;

            if (Settings.NpcBoxes)
            {
                Vector3 botScreen = WorldToScreen(cam, worldPos);
                float h = Mathf.Abs(botScreen.y - topScreen.y);
                float w = h * 0.5f;
                DrawBox(new Rect(topScreen.x - w / 2f, topScreen.y, w, h), col);
            }

            if (Settings.NpcNames || Settings.NpcDistance)
            {
                string label = "";
                if (Settings.NpcNames) label = name;
                if (Settings.NpcDistance) label += $"\n{dist:F0}m";
                DrawLabel(topScreen, label, col);
            }

            if (Settings.NpcTracers)
                DrawLine(new Vector2(Screen.width / 2f, Screen.height), new Vector2(topScreen.x, topScreen.y), col, 1f);
        }

        private void DrawItems(Camera cam)
        {
            PlayerNetworking local = PlayerHelper.GetLocalPlayer();
            if (local == null) return;

            int count = 0;
            foreach (StealableObject item in cachedItems)
            {
                if (item == null || count >= 50) break;

                float dist = Vector3.Distance(local.Position, item.transform.position);
                if (dist > Settings.MaxDistance) continue;

                Vector3 s = WorldToScreen(cam, item.transform.position + Vector3.up * 0.3f);
                if (s.z <= 0) continue;

                if (Settings.ItemNames || Settings.ItemDistance)
                {
                    string label = "";
                    if (Settings.ItemNames) label = item.gameObject.name;
                    if (Settings.ItemDistance) label += $"\n{dist:F0}m";
                    DrawLabel(s, label, Settings.ColorItem);
                }

                if (Settings.ItemTracers)
                    DrawLine(new Vector2(Screen.width / 2f, Screen.height), new Vector2(s.x, s.y), Settings.ColorItem, 1f);

                count++;
            }
        }

        // === Drawing Helpers ===
        private Vector3 WorldToScreen(Camera cam, Vector3 worldPos)
        {
            Vector3 s = cam.WorldToScreenPoint(worldPos);
            s.y = Screen.height - s.y;
            return s;
        }

        private void DrawLabel(Vector3 screenPos, string text, Color col)
        {
            Vector2 size = labelStyle.CalcSize(new GUIContent(text));
            Rect rect = new Rect(screenPos.x - size.x / 2f, screenPos.y - size.y - 4f, size.x, size.y);

            labelStyle.normal.textColor = Color.black;
            GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width, rect.height), text, labelStyle);
            labelStyle.normal.textColor = col;
            GUI.Label(rect, text, labelStyle);
        }

        private void DrawBox(Rect rect, Color color)
        {
            Color prev = GUI.color;
            GUI.color = color;
            float t = 2f;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, t), espTex);
            GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - t, rect.width, t), espTex);
            GUI.DrawTexture(new Rect(rect.x, rect.y, t, rect.height), espTex);
            GUI.DrawTexture(new Rect(rect.x + rect.width - t, rect.y, t, rect.height), espTex);
            GUI.color = prev;
        }

        private void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            Color prev = GUI.color;
            GUI.color = color;
            Vector2 delta = end - start;
            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            Matrix4x4 prevMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, start);
            GUI.DrawTexture(new Rect(start.x, start.y - width / 2f, delta.magnitude, width), espTex);
            GUI.matrix = prevMatrix;
            GUI.color = prev;
        }
    }
}
