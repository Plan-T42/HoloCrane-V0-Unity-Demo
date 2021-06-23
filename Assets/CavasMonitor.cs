using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CavasMonitor : MonoBehaviour
{
    GameObject connection;

    // Start is called before the first frame update
    void Awake()
    {
        connection = GameObject.Find("TestConnection");
        connection.GetComponent<testMultiThread>().startWatchdog = true;
        connection.GetComponent<testMultiThread>().startMonitor = true;
    }
    private void FixedUpdate()
    {
        GameObject Hoist = GameObject.Find("Hoist");
        Hoist.GetComponent<Text>().text = "Hoist: " + connection.GetComponent<testMultiThread>().hoistPos.ToString();

        GameObject ControlHoist = GameObject.Find("ControlHoist");
        ControlHoist.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().controlHoist.ToString();

        GameObject Bridge = GameObject.Find("Bridge");
        Bridge.GetComponent<Text>().text = "Bridge: " + connection.GetComponent<testMultiThread>().bridgePos.ToString();

        GameObject ControlBridge = GameObject.Find("ControlBridge");
        ControlBridge.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().controlBridge.ToString();

        GameObject Trolley = GameObject.Find("Trolley");
        Trolley.GetComponent<Text>().text = "Trolley: " + connection.GetComponent<testMultiThread>().trolleyPos.ToString();

        GameObject ControlTrolley = GameObject.Find("ControlTrolley");
        ControlTrolley.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().controlTrolley.ToString();

        GameObject Load = GameObject.Find("Load");
        Load.GetComponent<Text>().text = "Load: " + connection.GetComponent<testMultiThread>().load.ToString();

        GameObject Sway = GameObject.Find("Sway");
        Sway.GetComponent<Text>().text = "Sway: " + connection.GetComponent<testMultiThread>().SwayControl.ToString();

        GameObject Microspeed = GameObject.Find("Microspeed");
        Microspeed.GetComponent<Text>().text = "Microspeed: " + connection.GetComponent<testMultiThread>().MicroSpeed.ToString();

        GameObject Ropeangle = GameObject.Find("Ropeangle");
        Ropeangle.GetComponent<Text>().text = "Ropeangle: " + connection.GetComponent<testMultiThread>().RopeAngleFeaturesBypass.ToString();

        GameObject Inching = GameObject.Find("Inching");
        Inching.GetComponent<Text>().text = "Inching: " + connection.GetComponent<testMultiThread>().Inching.ToString();

        GameObject TargetHoist = GameObject.Find("TargetHoist");
        TargetHoist.GetComponent<Text>().text = "Hoist: " + connection.GetComponent<testMultiThread>().hoistTarget.ToString();

        GameObject TargetTrolley = GameObject.Find("TargetTrolley");
        TargetTrolley.GetComponent<Text>().text = "Trolley: " + connection.GetComponent<testMultiThread>().trolleyTarget.ToString();

        GameObject TargetBridge = GameObject.Find("TargetBridge");
        TargetBridge.GetComponent<Text>().text = "Bridge: " + connection.GetComponent<testMultiThread>().bridgeTarget.ToString();

        GameObject WatchDog = GameObject.Find("WatchDog");
        WatchDog.GetComponent<Text>().text = "WatchDog: " + connection.GetComponent<testMultiThread>().watchdog;

        GameObject WatchDogFault = GameObject.Find("WatchDogFault");
        if (connection.GetComponent<testMultiThread>().WatchdogFault == false)
        {
            WatchDogFault.GetComponent<Text>().text = "ok!";
            WatchDogFault.GetComponent<Text>().color = Color.green;
        }
        else 
        {
            WatchDogFault.GetComponent<Text>().text = "fail!";
            WatchDogFault.GetComponent<Text>().color = Color.red;
        }

        //GameObject TargetPositioningStart = GameObject.Find("TargetPositioningStart");
        //WatchDog.GetComponent<Text>().text = "TargetPositioning: " + connection.GetComponent<testMultiThread>().startTargetPositioning;

        GameObject WatchdogStart = GameObject.Find("WatchdogStart");
        WatchdogStart.GetComponent<Text>().text = "Watchdog: " + connection.GetComponent<testMultiThread>().startWatchdog;

        GameObject ControlStart = GameObject.Find("ControlStart");
        ControlStart.GetComponent<Text>().text = "Control: " + connection.GetComponent<testMultiThread>().startControl;

        GameObject MonitorStart = GameObject.Find("MonitorStart");
        MonitorStart.GetComponent<Text>().text = "Monitor: " + connection.GetComponent<testMultiThread>().startMonitor;


        // for debug connection (HttpRequest) in HoloLens:

        //GameObject WatchdogLog = GameObject.Find("WatchdogLog");
        //WatchdogLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().watchdogQueryLog;

        //GameObject MonitorLog = GameObject.Find("MonitorLog");
        //MonitorLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().monitorQueryLog;

        //GameObject ControlLog = GameObject.Find("ControlLog");
        //ControlLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().controlQueryLog;



    }

}
