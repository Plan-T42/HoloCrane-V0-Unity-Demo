// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;


namespace HoloToolkit.Unity.InputModule
{
    /// <summary>
    /// The TapToPlace class is a basic way to enable users to move objects 
    /// and place them on real world surfaces.
    /// Put this script on the object you want to be able to move. 
    /// Users will be able to tap objects, gaze elsewhere, and perform the tap gesture again to place.
    /// This script is used in conjunction with GazeManager, WorldAnchorManager, and SpatialMappingManager.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Interpolator))]
    public class TapToPlace : MonoBehaviour, IInputClickHandler
    {

        #region edited: new variables 

        private float bridgeTarget;
        private float trolleyTarget;
        private float hoistTarget;

        private new Transform transform;

        private float TROLLEY_OFFSET = 1.239f; //1.189f; +0.05 //0.700f; //Z // 0.589f; //1.189f;
        private float BRIDGE_OFFSET = 27.929f; //27.979f; -0.05 //23.354f; //X //27.979f;       
        private float HOIST_OFFSET = 1.699f; //1.749f; -0.05 // 0.374f; //Y //1.749f;
        private float SCALE = -0.161f;

        GameObject connection;

        #endregion


        [Tooltip("Distance from camera to keep the object while placing it.")]
        public float DefaultGazeDistance = 2.0f;

        [Tooltip("Place parent on tap instead of current game object.")]
        public bool PlaceParentOnTap;

        [Tooltip("Specify the parent game object to be moved on tap, if the immediate parent is not desired.")]
        public GameObject ParentGameObjectToPlace;

        /// <summary>
        /// Keeps track of if the user is moving the object or not.
        /// Setting this to true will enable the user to move and place the object in the scene.
        /// Useful when you want to place an object immediately.
        /// </summary>
        [Tooltip("Setting this to true will enable the user to move and place the object in the scene without needing to tap on the object. Useful when you want to place an object immediately.")]
        public bool IsBeingPlaced;

        [Tooltip("Setting this to true will allow this behavior to control the DrawMesh property on the spatial mapping.")]
        public bool AllowMeshVisualizationControl = true;

        [Tooltip("Should the center of the Collider be used instead of the gameObjects world transform.")]
        public bool UseColliderCenter;

        private Interpolator interpolator;

        /// <summary>
        /// The default ignore raycast layer built into unity.
        /// </summary>
        private const int IgnoreRaycastLayer = 2;

        private Dictionary<GameObject, int> layerCache = new Dictionary<GameObject, int>();
        private Vector3 PlacementPosOffset;

        private Color[] colors = new Color[] { Color.red, Color.cyan };
        private int i = 0;
        private Renderer rend;

        protected virtual void Start()
        {
            connection = GameObject.Find("TestConnection");

            rend = GetComponent<Renderer>();
            rend.material.color = Color.green;

            if (PlaceParentOnTap)
            {
                ParentGameObjectToPlace = GetParentToPlace();
                PlaceParentOnTap = ParentGameObjectToPlace != null;
            }

            interpolator = EnsureInterpolator();

            if (IsBeingPlaced)
            {
                StartPlacing();
            }
            else // If we are not starting out with actively placing the object, give it a World Anchor
            {
                //AttachWorldAnchor();
            }
        }

        private void OnEnable()
        {
            Bounds bounds = transform.GetColliderBounds();
            PlacementPosOffset = transform.position - bounds.center;
        }

        /// <summary>
        /// Returns the predefined GameObject or the immediate parent when it exists
        /// </summary>
        /// <returns></returns>
        private GameObject GetParentToPlace()
        {
            if (ParentGameObjectToPlace)
            {
                return ParentGameObjectToPlace;
            }

            return gameObject.transform.parent ? gameObject.transform.parent.gameObject : null;
        }

        /// <summary>
        /// Ensures an interpolator on either the parent or on the gameobject itself and returns it.
        /// </summary>
        private Interpolator EnsureInterpolator()
        {
            var interpolatorHolder = PlaceParentOnTap ? ParentGameObjectToPlace : gameObject;
            return interpolatorHolder.EnsureComponent<Interpolator>();
        }

        protected virtual void Update()
        {
            if (!IsBeingPlaced) { return; }
            Transform cameraTransform = CameraCache.Main.transform;

            Vector3 placementPosition = GetPlacementPosition(cameraTransform.position, cameraTransform.forward, DefaultGazeDistance);

            if (UseColliderCenter)
            {
                placementPosition += PlacementPosOffset;
            }

            // Here is where you might consider adding intelligence
            // to how the object is placed.  For example, consider
            // placing based on the bottom of the object's
            // collider so it sits properly on surfaces.

            if (PlaceParentOnTap)
            {
                placementPosition = ParentGameObjectToPlace.transform.position + (placementPosition - gameObject.transform.position);
            }

            // update the placement to match the user's gaze.
            interpolator.SetTargetPosition(placementPosition);

            // Rotate this object to face the user.
            interpolator.SetTargetRotation(Quaternion.Euler(0, cameraTransform.localEulerAngles.y, 0));
        }

        public virtual void OnInputClicked(InputClickedEventData eventData)
        {
            // On each tap gesture, toggle whether the user is in placing mode.
            IsBeingPlaced = !IsBeingPlaced;
            HandlePlacement();

            //here we change the color of the cube
            rend.material.color = colors[i];
            //index is calculated with mode operation so that
            //it stays in the size of the array
            i = (i + 1) % colors.Length;
        }

        private void HandlePlacement()
        {
            if (IsBeingPlaced)
            {
                StartPlacing();
            }
            else
            {
                StopPlacing();

            }
        }
        private void StartPlacing()
        {
            var layerCacheTarget = PlaceParentOnTap ? ParentGameObjectToPlace : gameObject;
            layerCacheTarget.SetLayerRecursively(IgnoreRaycastLayer, out layerCache);
            InputManager.Instance.PushModalInputHandler(gameObject);

            ToggleSpatialMesh();
            //RemoveWorldAnchor();
        }

        private void StopPlacing()
        {
            var layerCacheTarget = PlaceParentOnTap ? ParentGameObjectToPlace : gameObject;
            layerCacheTarget.ApplyLayerCacheRecursively(layerCache);
            InputManager.Instance.PopModalInputHandler();

            ToggleSpatialMesh();
            //AttachWorldAnchor();

            #region edited: new assignments

            transform = GetComponent<Transform>();

            trolleyTarget = SCALE * transform.localPosition.x + TROLLEY_OFFSET;
            bridgeTarget = SCALE * transform.localPosition.y + BRIDGE_OFFSET;
            hoistTarget = SCALE * transform.localPosition.z + HOIST_OFFSET;


            //trolleyTarget = -transform.position.z + TROLLEY_OFFSET;
            //hoistTarget = transform.position.y + HOIST_OFFSET;
            //bridgeTarget = transform.position.x + BRIDGE_OFFSET;

            //trolleyTarget = transform.position.x + TROLLEY_OFFSET;
            //hoistTarget = transform.position.y + HOIST_OFFSET;
            //bridgeTarget = transform.position.z + BRIDGE_OFFSET;

            connection.GetComponent<SingleThreadLessQuery>().hoistTarget = hoistTarget;
            connection.GetComponent<SingleThreadLessQuery>().trolleyTarget = trolleyTarget;
            connection.GetComponent<SingleThreadLessQuery>().bridgeTarget = bridgeTarget;

            connection.GetComponent<SingleThreadLessQuery>().startControl = true;

            //connection.GetComponent<testMultiThread>().hoistTarget = hoistTarget;
            //connection.GetComponent<testMultiThread>().trolleyTarget = trolleyTarget;
            //connection.GetComponent<testMultiThread>().bridgeTarget = bridgeTarget;

            //connection.GetComponent<testMultiThread>().startControl = true;

            Debug.Log("Tap to Place, going to Movable target");

            GameObject TargetName = GameObject.Find("TargetName");
            TargetName.GetComponent<TextMesh>().text = "Moveable Target selected";

            // for dubug the calibration (transform) in HoloLens:
            //GameObject PosY = GameObject.Find("PosY");
            //PosY.GetComponent<Text>().text = "PosY" + transform.localPosition.y.ToString();
            //GameObject PosX = GameObject.Find("PosX");
            //PosX.GetComponent<Text>().text = "PosX" + transform.localPosition.x.ToString();
            //GameObject PosZ = GameObject.Find("PosZ");
            //PosZ.GetComponent<Text>().text = "PosZ" + transform.localPosition.z.ToString();

            #endregion
        }

        private void AttachWorldAnchor()
        {
            if (WorldAnchorManager.Instance != null)
            {
                // Add world anchor when object placement is done.
                WorldAnchorManager.Instance.AttachAnchor(PlaceParentOnTap ? ParentGameObjectToPlace : gameObject);
            }
        }

        private void RemoveWorldAnchor()
        {
            if (WorldAnchorManager.Instance != null)
            {
                //Removes existing world anchor if any exist.
                WorldAnchorManager.Instance.RemoveAnchor(PlaceParentOnTap ? ParentGameObjectToPlace : gameObject);
            }
        }

        /// <summary>
        /// If the user is in placing mode, display the spatial mapping mesh.
        /// </summary>
        private void ToggleSpatialMesh()
        {
            if (SpatialMappingManager.Instance != null && AllowMeshVisualizationControl)
            {
                SpatialMappingManager.Instance.DrawVisualMeshes = IsBeingPlaced;
            }
        }

        /// <summary>
        /// If we're using the spatial mapping, check to see if we got a hit, else use the gaze position.
        /// </summary>
        /// <returns>Placement position infront of the user</returns>
        private static Vector3 GetPlacementPosition(Vector3 headPosition, Vector3 gazeDirection, float defaultGazeDistance)
        {
            RaycastHit hitInfo;
            if (SpatialMappingRaycast(headPosition, gazeDirection, out hitInfo))
            {
                return hitInfo.point;
            }
            return GetGazePlacementPosition(headPosition, gazeDirection, defaultGazeDistance);
        }

        /// <summary>
        /// Does a raycast on the spatial mapping layer to try to find a hit.
        /// </summary>
        /// <param name="origin">Origin of the raycast</param>
        /// <param name="direction">Direction of the raycast</param>
        /// <param name="spatialMapHit">Result of the raycast when a hit occured</param>
        /// <returns>Wheter it found a hit or not</returns>
        private static bool SpatialMappingRaycast(Vector3 origin, Vector3 direction, out RaycastHit spatialMapHit)
        {
            if (SpatialMappingManager.Instance != null)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(origin, direction, out hitInfo, 30.0f, SpatialMappingManager.Instance.LayerMask))
                {
                    spatialMapHit = hitInfo;
                    return true;
                }
            }
            spatialMapHit = new RaycastHit();
            return false;
        }

        /// <summary>
        /// Get placement position either from GazeManager hit or infront of the user as backup
        /// </summary>
        /// <param name="headPosition">Position of the users head</param>
        /// <param name="gazeDirection">Gaze direction of the user</param>
        /// <param name="defaultGazeDistance">Default placement distance infront of the user</param>
        /// <returns>Placement position infront of the user</returns>
        private static Vector3 GetGazePlacementPosition(Vector3 headPosition, Vector3 gazeDirection, float defaultGazeDistance)
        {
            if (GazeManager.Instance.HitObject != null)
            {
                return GazeManager.Instance.HitPosition;
            }
            return headPosition + gazeDirection * defaultGazeDistance;
        }
    }
}
