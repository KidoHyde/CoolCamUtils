using CrashUtils.WeaponManager.WeaponSetup;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CoolCamUtils
{
    public class CamGun : Gun
    {
        public static GameObject Asset;
        public static GameObject weaponPrefab;
        public static GameObject V1MDL;


        public GameObject CamObj;
        public GameObject PlayerObj;
        public Camera Cam;
        public Camera PlayerCam;
        public bool LookAt;
        public bool ShootAlt;

        public static void LoadAssets()
        {
            weaponPrefab = CamGunLoader.Assets.LoadAsset<GameObject>("CamGunPrefab");
            V1MDL = CamGunLoader.Assets.LoadAsset<GameObject>("v1_combined");

        }

        public override GameObject Create(Transform parent)
        {
            base.Create(parent);
            Asset = GameObject.Instantiate(weaponPrefab, parent);
            CamGunLoader.CamGn = Asset.AddComponent<CameraGun>();

            PlayerObj = GameObject.Instantiate(V1MDL, CameraController.Instance.GetComponent<Camera>().transform);
            PlayerObj.transform.localPosition = new Vector3(0, -3, -1);
            PlayerObj.transform.localScale = new Vector3(2, 2, 2);

            return Asset;
        }

        

        public override int Slot()
        {
            return 0;
        }
		
		public override int WheelOrder()
        {
            return 6;
        }

        public override string Pref()
        {
            return "camgun0";
        }

        
    }
    public class CameraGun : MonoBehaviour
    {
        public static AssetBundle Assets;
        public GameObject CamObj;
        public Camera Cam;
        public Camera PlayerCam;
        public bool LookAt;
        public bool ShootAlt;
        private float cooldown = 0.25f;

        private void Start()
        {

            PlayerCam = CameraController.Instance.GetComponent<Camera>();
            CamObj = new GameObject("Cool Cam");
            Cam = CamObj.AddComponent<Camera>();
            Cam.Render();
            Cam.cullingMask = 8192 | 198369047;
            Cam.gameObject.transform.SetParent(null);
            Cam.enabled = false;

        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            cooldown = 0;
        }

        private void OnDestroy()
        {

        }

        private void Update()
        {

            cooldown -= Time.deltaTime;

            if (!MonoSingleton<ColorBlindSettings>.Instance)
            {
                return;
            }




            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && !GameStateManager.Instance.PlayerInputLocked)
            {

                if (LookAt)
                {
                    LookAt = false;
                }
                else
                {
                    LookAt = true;
                }
            }





            else if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && MonoSingleton<GunControl>.Instance.activated && !GameStateManager.Instance.PlayerInputLocked && cooldown <= 0)
            {
                if (!ShootAlt)
                {
                    Cam.enabled = true;
                    this.Shoot();
                    cooldown = 0.25f;
                }
                else
                {
                    DisableCams();
                    cooldown = 0.25f;
                }
            }

        }

        public void Shoot()
        {
            Cam.transform.position = PlayerCam.transform.position;
            Cam.transform.rotation = PlayerCam.transform.rotation;
            Cam.enabled = true;
            Cam.depth = 10;
            Cam.pixelRect = new Rect(Screen.width * 0.625f, 100, Screen.width * 0.25f, Screen.height * 0.25f);
            ShootAlt = true;

        }
        public void DisableCams()
        {
            Cam.enabled = false;
            ShootAlt = false;
        }

    }


        
}
