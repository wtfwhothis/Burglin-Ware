using UnityEngine;
using System.Collections.Generic;
using GnomeCheat.Actions;

namespace GnomeCheat.UI.Tabs
{
    public class ObjectsTab
    {
        private List<string> objectList = new List<string>
        {
            "Knife", "Gun", "Taser", "Grenade", "PepperSpray", "Mousetrap",
            "BearToy", "CatToy", "BallToy", "RobotToy", "Egg", "Bread",
            "Toaster", "Radio", "RecordPlayer", "GameConsole", "GameConsoleJoystick",
            "Hairdryer", "Fork", "Pan", "Teapot", "CatBowl", "Globe",
            "ClockTable", "Slipper", "Underpants", "TrashBucket", "Plunger", "ToiletPaper"
        };
        private int selectedIndex;
        private Vector2 scrollPos;

        public void Draw()
        {
            GUILayout.Label("=== OBJECT SPAWNER ===", Styles.Box);
            GUILayout.Space(10);

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));
            for (int i = 0; i < objectList.Count; i++)
            {
                if (GUILayout.Toggle(selectedIndex == i, objectList[i], Styles.Toggle))
                    selectedIndex = i;
            }
            GUILayout.EndScrollView();
            GUILayout.Space(10);

            if (GUILayout.Button($"Spawn: {objectList[selectedIndex]}", Styles.Button, GUILayout.Height(40)))
                SpawnerActions.SpawnObject(objectList[selectedIndex]);

            // GUILayout.Space(5);
            //if (GUILayout.Button("Find All Objects in Scene", Styles.Button))
                //SpawnerActions.FindAllObjects();
        }
    }
}
