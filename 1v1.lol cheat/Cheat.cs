using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace _1v1.lol_cheat
{
    /*
     * CODED BY OUTSPECT AND FXZH
     * I am not going to put comments everywhere.
     * I might update this in the future.
    */

    public class Cheat : MonoBehaviour
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey); //input handling

        //GUI Toggling (45 = insert)
        public int toggleKey = 45;
        public float toggleDelay = 0.5f;
        private bool toggled = true;
        private float lastToggleTime;

        //Variable to keep targets
        public static List<PlayerController> targets = new List<PlayerController>();

        //material for esp and fov circle
        public static Material mat = new Material(Shader.Find("GUI/Text Shader"));

        //watermark (now be happy this is open source lmao)
        private float colorChangeSpeed = 1f;
        private float timer = 0f;

        private void Start()
        {
            StartCoroutine(UpdateTargets());
        }

        private IEnumerator UpdateTargets()
        {
            while (true)
            {
                targets = Utils.GetTargets(); //get the list of targets from the util
                yield return new WaitForSeconds(1f);
            }
        }

        private void OnGUI()
        {
            float r = Mathf.PingPong(timer * colorChangeSpeed, 1f);
            float g = Mathf.PingPong(timer * colorChangeSpeed + 0.33f, 1f);
            float b = Mathf.PingPong(timer * colorChangeSpeed + 0.66f, 1f);

            UnityEngine.GUI.color = new Color(r, g, b);

            UnityEngine.GUI.Label(new Rect(10, 10, 400, 40), "https://github.com/outspect 1v1.lol cheat");

            UnityEngine.GUI.color = Color.white;

            timer += Time.deltaTime;

            if (GUI.fovcircle)
            {
                FOVCircle((int)GUI.fov);
            }

            if (toggled)
            {
                GUI.GUIRect = UnityEngine.GUI.Window(69, GUI.GUIRect, GUI.GUIMain, "outspect 1v1.lol cheat | FPS: " + 1.0f / Time.deltaTime + " | Toggle: INSERT");
            }
        }

        private void GUIToggleCheck()
        {
            if (GetAsyncKeyState(toggleKey) < 0)
            {
                if (Time.time - lastToggleTime >= toggleDelay)
                {
                    toggled = !toggled;
                    lastToggleTime = Time.time;
                }
            }
        }

        private void FOVCircle(int radius)
        {
            mat.SetPass(0);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
            GL.Begin(2);
            GL.Color(Color.yellow);

            for (int i = 0; i <= 360; i++)
            {
                float angle = i * (2f * (float)Math.PI / 360f);
                float x = Mathf.Cos(angle) * radius + Screen.width / 2;
                float y = Mathf.Sin(angle) * radius + Screen.height / 2;
                GL.Vertex(new Vector3(x, y, 0));
            }

            GL.End();
            GL.PopMatrix();
        }

        private void SilentAim()
        {
            short result = GetAsyncKeyState(0x01);
            bool leftmousebutton = (result & 0x8000) != 0;

            if (leftmousebutton)
            {
                Transform aimpos = null;

                float closest = float.PositiveInfinity;

                foreach (PlayerController ctrl in targets)
                {
                    //fxzh type shit
                    Vector3 w2s = CameraManager.NFLLAGMKOCA.MainCamera.WorldToScreenPoint(ctrl.gameObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).transform.position);
                    float abs = Vector2.Distance(new Vector2(w2s.x, Screen.height - w2s.y), new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f));
                    if (ctrl.IsMine() == false && !ctrl.JNLGBNPHIDF && w2s.z >= 0f && abs <= GUI.fov && abs < closest)
                    {
                        closest = abs;
                        aimpos = ctrl.gameObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).transform; //pretty straight forward
                    }
                }

                if (aimpos != null && closest != float.PositiveInfinity)
                {
                    CameraManager.NFLLAGMKOCA.MainCamera.gameObject.transform.LookAt(aimpos); //1v1lol camera works really weird but this is a silent aim
                }
            }
        }

        private static void BoxESP(bool enable)
        {
            if (enable)
            {
                foreach (PlayerController sss in targets)
                {
                    Vector3 first = sss.transform.position;

                    Vector3 start = new Vector3(first.x, first.y + 1.75f, first.z);
                    Vector3 end = new Vector3(first.x, first.y, first.z);

                    LineRenderer lineRenderer;
                    if (!sss.gameObject.GetComponent<LineRenderer>())
                    {
                        lineRenderer = sss.gameObject.AddComponent<LineRenderer>();
                    }
                    else
                    {
                        lineRenderer = sss.gameObject.GetComponent<LineRenderer>();
                    }

                    Material material = new Material(Shader.Find("GUI/Text Shader"));
                    material.color = new UnityEngine.Color(1f, 1f, 1f, 0.75f);

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, start);
                    lineRenderer.SetPosition(1, end);
                    lineRenderer.material = material;
                    lineRenderer.enabled = true;

                    float hue = Time.time * 0.5f;
                    UnityEngine.Color headColor = UnityEngine.Color.HSVToRGB(hue % 1f, 1f, 1f);
                    lineRenderer.startColor = new UnityEngine.Color(headColor.r, headColor.g, headColor.b, 0.75f);
                    lineRenderer.endColor = new UnityEngine.Color(headColor.r, headColor.g, headColor.b, 0.75f);

                    GUI.boxfix = true;
                }
            }
            else
            {
                if (GUI.boxfix)
                {
                    foreach (PlayerController sss in targets)
                    {
                        UnityEngine.Object.Destroy(sss.gameObject.GetComponent<LineRenderer>());
                    }
                    GUI.boxfix = false;
                }
            }
        }

        private static void Crasher()
        {
            int VK_MBUTTON = 4;
            short state = GetAsyncKeyState(VK_MBUTTON);
            if (GUI.crasher && (state & 0x8001) != 0)
            {
                float closest = float.PositiveInfinity;
                foreach (PlayerController ctrl in targets)
                {
                    Vector3 w2s = CameraManager.NFLLAGMKOCA.MainCamera.WorldToScreenPoint(ctrl.gameObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).transform.position);
                    float abs = Vector2.Distance(new Vector2(w2s.x, Screen.height - w2s.y), new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f));
                    if (ctrl.IsMine() == false && !ctrl.JNLGBNPHIDF && w2s.z >= 0f && abs <= GUI.fov && abs < closest)
                    {
                        closest = abs;
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer); //sets yourself as master client
                        PhotonNetwork.DestroyPlayerObjects(ctrl.photonView.Controller); //destroys player objects (will make them invisible and unable to do anything)
                    }
                }
            }
        }

        private void Update()
        {
            GUIToggleCheck();

            if (GUI.silentaim)
            {
                SilentAim();
            }

            if (GUI.boxesp)
            {
                BoxESP(true);
            } else { BoxESP(false); }

            if (GUI.crasher)
            {
                Crasher();
            }

            if (GUI.godmode)
            {
                PlayerController.LFNGIIPNIDN.ABDABPEKBFM.SetPlayerImmunity(true); //a joke
            }
            else
            {
                PlayerController.LFNGIIPNIDN.ABDABPEKBFM.SetPlayerImmunity(false);
            }

            if (GUI.infiniteammo)
            {
                PlayerController.LFNGIIPNIDN.AIACBMLLLFE.PFPIKMMEICB.SetCurrentAmmoAmount((int)999999);
                PlayerController.LFNGIIPNIDN.AIACBMLLLFE.PFPIKMMEICB.SetCurrentMagazineAmount((int)999999);
            }

            if (GUI.rapidfire)
            {
                GUI.rapidcooldown++;
                if (GUI.rapidcooldown > GUI.fireratecooldown)
                {
                    if (UnityEngine.Input.GetMouseButton(0))
                    {
                        PlayerController.LFNGIIPNIDN.AIACBMLLLFE.photonView.RPC("FireWeaponRemote", RpcTarget.All, new object[]
                        {
                        null,
                        true,
                        1
                        });
                    }
                    GUI.rapidcooldown = 0;
                }
            }

            if (GUI.speed)
            {
                PlayerController.LFNGIIPNIDN.NGABAFFHJBE = GUI.speedmultiply;
            }
            else
            {
                PlayerController.LFNGIIPNIDN.NGABAFFHJBE = 1; //should be the default value
            }
        }
    }
}
