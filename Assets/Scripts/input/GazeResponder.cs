// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

namespace AaltoHoloCrane.Input
{
    /// <summary>
    /// This class implements IFocusable to respond to gaze changes.
    /// It highlights the object being gazed at.
    /// </summary>
    public class GazeResponder : MonoBehaviour, IFocusable
    {
        private Material[] defaultMaterials;
        private Transform transform;
        //GameObject goX;
        //GameObject goY;
        //GameObject goZ;

        private void Start()
        {
            defaultMaterials = GetComponent<Renderer>().materials;
            transform = GetComponent<Transform>();

            //goX = GameObject.Find("TargetX");
            //goY = GameObject.Find("TargetY");
            //goZ = GameObject.Find("TargetZ");

        }

        public void OnFocusEnter()
        {
            for (int i = 0; i < defaultMaterials.Length; i++)
            {
                // Highlight the material when gaze enters using the shader property.
                defaultMaterials[i].SetFloat("_Highlight", .25f);
                // defaultMaterials[i].EnableKeyword("_ENVIRONMENT_COLORING");

                Debug.Log("In gaze");
                Debug.Log(transform.position);

                //goX.GetComponent<Text>().text = "T: " + (transform.position.x+3.35).ToString("0.00");           
                //goY.GetComponent<Text>().text = "H: " + (transform.position.y+1.55).ToString("0.00");
                //goZ.GetComponent<Text>().text = "B: " + (transform.position.z+15.96).ToString("0.00");

            }
        }

        public void OnFocusExit()
        {
            for (int i = 0; i < defaultMaterials.Length; i++)
            {
                // Remove highlight on material when gaze exits.
                defaultMaterials[i].SetFloat("_Highlight", 0f);
                //defaultMaterials[i].DisableKeyword("_ENVIRONMENT_COLORING");

                Debug.Log("Gaze Exit");

                //goX.GetComponent<Text>().text = "X: ";
                //goY.GetComponent<Text>().text = "Y: ";
                //goZ.GetComponent<Text>().text = "Z: ";
            }
        }
    }
}