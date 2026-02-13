using UnityEngine;

namespace GnomeCheat.UI
{
    public static class Styles
    {
        public static bool Initialized { get; private set; }

        public static GUIStyle Background;
        public static GUIStyle Title;
        public static GUIStyle Tab;
        public static GUIStyle TabActive;
        public static GUIStyle Button;
        public static GUIStyle Toggle;
        public static GUIStyle Box;
        public static GUIStyle Label;
        public static GUIStyle Slider;
        public static GUIStyle SliderThumb;

        private static Texture2D texDark, texMid, texLight, texAccent, texAccentHover, texToggleOn, texToggleOff;
        public static Texture2D TexDark => texDark;

        public static void Init()
        {
            if (Initialized) return;

            texDark = MakeTex(new Color(0.08f, 0.08f, 0.12f, 0.95f));
            texMid = MakeTex(new Color(0.14f, 0.14f, 0.2f, 1f));
            texLight = MakeTex(new Color(0.2f, 0.2f, 0.28f, 1f));
            texAccent = MakeTex(new Color(0.4f, 0.2f, 0.8f, 1f));
            texAccentHover = MakeTex(new Color(0.5f, 0.3f, 0.9f, 1f));
            texToggleOn = MakeTex(new Color(0.3f, 0.75f, 0.4f, 1f));
            texToggleOff = MakeTex(new Color(0.25f, 0.25f, 0.3f, 1f));

            Background = new GUIStyle { padding = new RectOffset(12, 12, 12, 12) };
            Background.normal.background = texDark;

            Title = new GUIStyle
            {
                fontSize = 18, fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 4, 8)
            };
            Title.normal.textColor = new Color(0.7f, 0.5f, 1f);

            Tab = new GUIStyle
            {
                fontSize = 12, fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(8, 8, 6, 6),
                margin = new RectOffset(2, 2, 0, 0)
            };
            Tab.normal.background = texMid;
            Tab.normal.textColor = new Color(0.7f, 0.7f, 0.75f);
            Tab.hover.background = texLight;
            Tab.hover.textColor = Color.white;

            TabActive = new GUIStyle(Tab);
            TabActive.normal.background = texAccent;
            TabActive.normal.textColor = Color.white;
            TabActive.hover.background = texAccentHover;

            Button = new GUIStyle
            {
                fontSize = 12, alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(8, 8, 6, 6),
                margin = new RectOffset(2, 2, 2, 2)
            };
            Button.normal.background = texMid;
            Button.normal.textColor = new Color(0.85f, 0.85f, 0.9f);
            Button.hover.background = texLight;
            Button.hover.textColor = Color.white;
            Button.active.background = texAccent;
            Button.active.textColor = Color.white;

            Toggle = new GUIStyle
            {
                fontSize = 12, alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(8, 8, 6, 6),
                margin = new RectOffset(2, 2, 2, 2)
            };
            Toggle.normal.background = texToggleOff;
            Toggle.normal.textColor = new Color(0.7f, 0.7f, 0.75f);
            Toggle.onNormal.background = texToggleOn;
            Toggle.onNormal.textColor = Color.white;
            Toggle.hover.background = texLight;
            Toggle.hover.textColor = Color.white;
            Toggle.onHover.background = texToggleOn;
            Toggle.onHover.textColor = Color.white;

            Box = new GUIStyle
            {
                fontSize = 13, fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(6, 6, 4, 4),
                margin = new RectOffset(0, 0, 4, 4)
            };
            Box.normal.background = texMid;
            Box.normal.textColor = new Color(0.7f, 0.5f, 1f);

            Label = new GUIStyle
            {
                fontSize = 12,
                padding = new RectOffset(4, 4, 2, 2)
            };
            Label.normal.textColor = new Color(0.8f, 0.8f, 0.85f);

            Slider = new GUIStyle(GUI.skin.horizontalSlider);
            Slider.normal.background = texMid;
            Slider.fixedHeight = 8;

            SliderThumb = new GUIStyle(GUI.skin.horizontalSliderThumb);
            SliderThumb.normal.background = texAccent;
            SliderThumb.fixedWidth = 14;
            SliderThumb.fixedHeight = 14;

            Initialized = true;
        }

        private static Texture2D MakeTex(Color col)
        {
            Texture2D t = new Texture2D(1, 1);
            t.SetPixel(0, 0, col);
            t.Apply();
            return t;
        }
    }
}
