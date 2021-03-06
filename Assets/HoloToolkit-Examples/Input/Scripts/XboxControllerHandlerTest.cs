// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.UI;

namespace HoloToolkit.Unity.InputModule.Tests
{
    public class XboxControllerHandlerTest : XboxControllerHandlerBase
    {

        [SerializeField]
        private float movementSpeedMultiplier = 1f;

        [SerializeField]
        private float rotationSpeedMultiplier = 1f;

        [SerializeField]
        private XboxControllerMappingTypes resetButton = XboxControllerMappingTypes.XboxY;

        public Text DebugText;
        private Vector3 initialPosition;
        private Vector3 newPosition;
        private Vector3 newRotation;

        protected override void Start()
        {
            base.Start();
            initialPosition = transform.position;
        }

        public override void OnGamePadDetected(GamePadEventData eventData)
        {
            base.OnGamePadDetected(eventData);
            Debug.LogFormat("Joystick \"{0}\" Connected with id: {1}", eventData.GamePadName, eventData.SourceId);
        }

        public override void OnGamePadLost(GamePadEventData eventData)
        {
            base.OnGamePadLost(eventData);
            Debug.LogFormat("Joystick \"{0}\" Disconnected with id: {1}", eventData.GamePadName, eventData.SourceId);
            DebugText.text = "No Controller Connected";
        }

        public override void OnXboxAxisUpdate(XboxControllerEventData eventData)
        {
            base.OnXboxAxisUpdate(eventData);
            newPosition = Vector3.zero;
            newRotation = Vector3.zero;

            newPosition.x += eventData.XboxLeftStickHorizontalAxis * movementSpeedMultiplier;
            newPosition.z += eventData.XboxLeftStickVerticalAxis * movementSpeedMultiplier;
            newPosition.y += eventData.XboxSharedTriggerAxis * movementSpeedMultiplier;

            transform.position += newPosition;

            newRotation.y += eventData.XboxRightStickHorizontalAxis * rotationSpeedMultiplier;

            transform.rotation *= Quaternion.Euler(newRotation);

            if (OnButton_Up(resetButton, eventData))
            {
                transform.position = initialPosition;
            }

            DebugText.text =
                string.Format(
                    "{19}\n" +
                    "LS Horizontal: {0:0.000} Vertical: {1:0.000}\n" +
                    "RS Horizontal: {2:0.000} Vertical: {3:0.000}\n" +
                    "DP Horizontal: {4:0.000} Vertical: {5:0.000}\n" +
                    "Left Trigger:  {6:0.000} Right Trigger: {7:0.000} Shared Trigger: {8:0.00}\n" +
                    "A: {9} B: {10} X: {11} Y: {12}\n" +
                    "LB: {13} RB: {14} " +
                    "LS: {15} RS: {16}\n" +
                    "View: {17} Menu: {18}\n",
                    eventData.XboxLeftStickHorizontalAxis, eventData.XboxLeftStickVerticalAxis,
                    eventData.XboxRightStickHorizontalAxis, eventData.XboxRightStickVerticalAxis,
                    eventData.XboxDpadHorizontalAxis, eventData.XboxDpadVerticalAxis,
                    eventData.XboxLeftTriggerAxis, eventData.XboxRightTriggerAxis, eventData.XboxSharedTriggerAxis,
                    eventData.XboxA_Pressed, eventData.XboxB_Pressed, eventData.XboxX_Pressed, eventData.XboxY_Pressed,
                    eventData.XboxLeftBumper_Pressed, eventData.XboxRightBumper_Pressed,
                    eventData.XboxLeftStick_Pressed, eventData.XboxRightStick_Pressed,
                    eventData.XboxView_Pressed, eventData.XboxMenu_Pressed,
                    GamePadName);
        }
    }
}
