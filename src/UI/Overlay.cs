using UnityEngine;

namespace GnomeCheat.UI
{
    public class Overlay
    {
        private GUIStyle titleStyle;
        private GUIStyle infoStyle;
        private bool initialized;
        private float deltaTime;

        public void Draw()
        {
            Init();

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1f / deltaTime;

            float t = (Mathf.Sin(Time.unscaledTime * 2f) + 1f) / 2f;
            Color col1 = new Color(0.6f, 0.1f, 0.9f);
            Color col2 = new Color(1f, 0.2f, 0.6f);
            Color gradient = Color.Lerp(col1, col2, t);

            titleStyle.normal.textColor = Color.black;
            GUI.Label(new Rect(16, 12, 300, 28), "BURGLIN'WARE V1.6", titleStyle);
            titleStyle.normal.textColor = gradient;
            GUI.Label(new Rect(15, 11, 300, 28), "BURGLIN'WARE V1.6", titleStyle);

            infoStyle.normal.textColor = Color.black;
            GUI.Label(new Rect(16, 34, 300, 20), "Made By LDAP & Gideon", infoStyle);
            GUI.Label(new Rect(16, 50, 300, 20), $"FPS: {fps:F0}", infoStyle);

            infoStyle.normal.textColor = new Color(0.95f, 0.95f, 1f, 0.9f);
            GUI.Label(new Rect(15, 33, 300, 20), "Made By LDAP & Gideon", infoStyle);
            GUI.Label(new Rect(15, 49, 300, 20), $"FPS: {fps:F0}", infoStyle);
        }

        private void Init()
        {
            if (initialized) return;

            titleStyle = new GUIStyle
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };

            infoStyle = new GUIStyle
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };

            initialized = true;
        }
    }
}