using System.Collections.Generic;
using UnityEngine;
using Utility;
using Utility.Hierarchy;

namespace Game.GameType.Roman.ClientSide
{
    public class DiscoveryMode : MonoBehaviour
    {
        private Camera _mainCamera;
        private Transform _cameraTransform;

        private bool isDiscoveryMode = false;

        private List<GameObject> _cards = new List<GameObject>();
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            _cameraTransform = _mainCamera.transform;
            Input.gyro.enabled = true;
        }

        private void Update()
        {
            if (!isDiscoveryMode) return;
            
            Vector3 gravity = Input.gyro.gravity;
            Vector3 cameraRot = new Vector3(gravity.z * -90f, gravity.x * 90f, 0);
            
            _cameraTransform.rotation = Quaternion.Euler(cameraRot);
        }

        public void EnterDiscoveryMode()
        {
            Camera.allCameras[(int)Define.CameraIndex.Main].orthographic = false;
            
            GameObject deviceObjectPrefab = Resources.Load<GameObject>("Prefabs/DeviceObject");

            int n = 3;
            int r = 2;
            
            for (int i = 0; i < n; i++)
            {
                Vector3 targetPos = new Vector3(r * Mathf.Cos(((360f / n) * i - 90f) * Mathf.Deg2Rad), 3, r * Mathf.Sin(((360f / n) * i - 90f) * Mathf.Deg2Rad));
                Quaternion targetRot = Quaternion.Euler(-90, 0, -90 + ((180 * (n-2) / n) * i));
            
                var newObj = Instantiate(deviceObjectPrefab, targetPos, targetRot, GameObjectRoot.Transform);
                _cards.Add(newObj);
                
                var rigidBody = newObj.GetComponent<Rigidbody>();
                rigidBody.useGravity = false;
                
                var col = newObj.GetComponent<Collider>();
                col.isTrigger = true;
                
                var newDeviceObject = newObj.GetComponent<DeviceObject>();
                newDeviceObject.ownColor = (ColorPalette.ColorName)i;
            }
            
            isDiscoveryMode = true;
        }

        public void ExitDiscoveryMode()
        {
            Camera.allCameras[(int)Define.CameraIndex.Main].orthographic = false;

            foreach (var card in _cards)
            {
                Destroy(card);
            }
            
            isDiscoveryMode = false;
        }
    }
}