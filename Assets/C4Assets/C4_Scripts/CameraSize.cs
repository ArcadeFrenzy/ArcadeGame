using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour
{
    public class CameraSize : MonoBehaviour
    {
        Camera mainCam;

        private void Awake()
        {
            mainCam = GetComponent<Camera>();
            mainCam.orthographic = true;
        }

        private void LateUpdate()
        {
            float maxY = (GameObject.Find("ConnectFourGame").GetComponent<ConnectFourGame1>().numRows + 2);

            mainCam.orthographicSize = maxY / 2f;
        }
    }
}
