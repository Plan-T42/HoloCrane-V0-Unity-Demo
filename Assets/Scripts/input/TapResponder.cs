// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace AaltoHoloCrane.Input
{
    /// <summary>
    /// This class implements IInputClickHandler to handle the tap gesture.
    /// It increases the scale of the object when tapped.
    /// </summary>
    /// 
    
    public class TapResponder : MonoBehaviour, IInputClickHandler
    {

        private float bridgeTarget;
        private float trolleyTarget;
        private float hoistTarget;

        private new Transform transform;

        private float TROLLEY_OFFSET = 1.239f; //1.189f; +0.05 //0.700f; //Z // 0.589f; //1.189f;
        private float BRIDGE_OFFSET = 27.929f; //27.979f; -0.05 //23.354f; //X //27.979f;       
        private float HOIST_OFFSET = 1.699f; //1.749f; -0.05 // 0.374f; //Y //1.749f;
        private float SCALE = -0.161f;

        public bool selected = false;
        GameObject connection;

        //private Material cachedMaterial;
        //private Color defaultColor;

        //private bool isHoistInTarget;
        //private bool isTrolleyInTarget;
        //private bool isHBridgeInTarget;

        private Color[] colors = new Color[] { Color.red, Color.cyan};
        private int i = 0;
        private Renderer rend;



        void Start()
        {
            connection = GameObject.Find("TestConnection");
            rend = GetComponent<Renderer>();
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {

            //here we change the color of the cube
            rend.material.color = colors[i];
            //index is calculated with mode operation so that¨it stays in the size of the array
            i = (i + 1) % colors.Length;

            //sanity test: (example target pos) 
            //if(name == "Target1")
            //{
            //    connection.GetComponent<testMultiThread>().target = 1;
            //}
            //else if (name == "Target2")
            //{
            //    connection.GetComponent<testMultiThread>().target = 2;
            //}
            //else if (name == "Target4")
            //{
            //    connection.GetComponent<testMultiThread>().target = 4;
            //}
            //else if (name == "Target5")
            //{
            //    connection.GetComponent<testMultiThread>().target = 5;
            //}

            //connection.GetComponent<testMultiThread>().startTargetPositioning = true;

            transform = GetComponent<Transform>();

            //trolleyTarget = SCALE * transform.position.x + TROLLEY_OFFSET;
            //bridgeTarget = SCALE * transform.position.y + BRIDGE_OFFSET;
            //hoistTarget = SCALE * transform.position.z + HOIST_OFFSET;
           
            trolleyTarget = SCALE * transform.localPosition.x + TROLLEY_OFFSET;
            bridgeTarget = SCALE * transform.localPosition.y + BRIDGE_OFFSET;
            hoistTarget = SCALE * transform.localPosition.z + HOIST_OFFSET;

            //trolleyTarget = -transform.position.z + TROLLEY_OFFSET;
            //hoistTarget = transform.position.y + HOIST_OFFSET;
            //bridgeTarget = transform.position.x + BRIDGE_OFFSET;

            connection.GetComponent<SingleThreadLessQuery>().hoistTarget = hoistTarget;
            connection.GetComponent<SingleThreadLessQuery>().trolleyTarget = trolleyTarget;
            connection.GetComponent<SingleThreadLessQuery>().bridgeTarget = bridgeTarget;

            connection.GetComponent<SingleThreadLessQuery>().startControl = true;
            selected = true;

            //connection.GetComponent<testMultiThread>().hoistTarget = hoistTarget;
            //connection.GetComponent<testMultiThread>().trolleyTarget = trolleyTarget;
            //connection.GetComponent<testMultiThread>().bridgeTarget = bridgeTarget;

            //connection.GetComponent<testMultiThread>().startControl = true;

            Debug.Log("Air Tap, going to fixed target");

            GameObject TargetName = GameObject.Find("TargetName");
            TargetName.GetComponent<TextMesh>().text = "Fixed " + name + " selected";


            // for dubug the calibration (transform) in HoloLens:
            //GameObject PosY = GameObject.Find("PosY");
            //PosY.GetComponent<Text>().text = "PosY" + transform.localPosition.y.ToString();
            //GameObject PosX = GameObject.Find("PosX");
            //PosX.GetComponent<Text>().text = "PosX" + transform.localPosition.x.ToString();
            //GameObject PosZ = GameObject.Find("PosZ");
            //PosZ.GetComponent<Text>().text = "PosZ" + transform.localPosition.z.ToString();


        }
    }
}