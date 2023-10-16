using UnityEngine;

namespace _1v1.lol_cheat
{
    public class GUI : MonoBehaviour
    {
        //GUI Variables
        public static Rect GUIRect = new Rect(30, 30, 700, 600);
        public static int selected_tab = 0;
        public static string[] tabnames = { "Aim", "Visual", "Other" };

        //aim
        public static bool silentaim = false;
        public static float fov = 180;

        //fovcircle
        public static bool fovcircle = false;

        //godmode
        public static bool godmode = false;

        //infiniteammo
        public static bool infiniteammo = false;

        //rapidfire
        public static bool rapidfire = false;
        public static int rapidcooldown = 0;
        public static float fireratecooldown = 3;

        //crasher
        public static bool crasher = false;

        //boxesp
        public static bool boxesp = false;
        public static bool boxfix = false;

        //speed
        public static bool speed = false;
        public static float speedmultiply = 5;

        public static void GUIMain(int o) //required for the gui.window thing
        {
            GUILayout.BeginArea(new Rect(10, 10, GUIRect.width - 20, GUIRect.height - 20));
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            for (int i = 0; i < tabnames.Length; i++)
            {
                if (GUILayout.Toggle(selected_tab == i, tabnames[i], "Button"))
                {
                    selected_tab = i; //set the tab
                }
            }
            GUILayout.EndHorizontal();


            switch (selected_tab)
            {
                case 0:
                    silentaim = DrawToggleButton(silentaim, "Silent Aim");
                    crasher = DrawToggleButton(crasher, "Crash Players in FOV Circle (Middle mouse click)");
                    break;
                case 1:
                    boxesp = DrawToggleButton(boxesp, "Box ESP");
                    fovcircle = DrawToggleButton(fovcircle, "FOV Circle");
                    GUILayout.Label("FOV: " + fov);
                    fov = GUILayout.HorizontalSlider(fov, 1.0f, 1440.0f);

                    break;
                case 2:
                    godmode = DrawToggleButton(godmode, "God Mode");
                    infiniteammo = DrawToggleButton(infiniteammo, "Infinite Ammo");
                    rapidfire = DrawToggleButton(rapidfire, "Fire Rate");
                    GUILayout.Label("Fire rate cooldown (in frames): " + fireratecooldown);
                    fireratecooldown = GUILayout.HorizontalSlider(fireratecooldown, 1.1f, 100.0f);
                    speed = DrawToggleButton(speed, "Speed");
                    GUILayout.Label("Speed: " + speedmultiply);
                    speedmultiply = GUILayout.HorizontalSlider(speedmultiply, 1.0f, 15.0f);
                    break;

            }

            GUILayout.EndArea();
            UnityEngine.GUI.DragWindow(new Rect(0, 0, GUIRect.width, 20));
        }
        public static bool DrawToggleButton(bool currentState, string label)
        {
            Color buttonColor;

            if (true)
            {
                if (currentState)
                {
                    buttonColor = new Color(0.15f, 0.7f, 0.15f);
                }
                else
                {
                    buttonColor = new Color(0.3f, 0.3f, 0.3f);
                }
            }
            else { buttonColor = new Color(0f, 0f, 0f); }

            UnityEngine.GUI.backgroundColor = buttonColor;

            if (GUILayout.Button(label))
            {
                currentState = !currentState;
            }

            return currentState;
        }
    }
}
