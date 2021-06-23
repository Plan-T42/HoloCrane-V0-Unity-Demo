using UnityEngine;
using UnityEngine.UI;

public class CavasSingleThreadLessQuery : MonoBehaviour
{
    GameObject connection;

    // Start is called before the first frame update
    void Awake()
    {
        connection = GameObject.Find("TestConnection");
        connection.GetComponent<SingleThreadLessQuery>().startWatchdog = true;
        connection.GetComponent<SingleThreadLessQuery>().startMonitor = true;
    }
    private void FixedUpdate()
    {
        GameObject Hoist = GameObject.Find("Hoist");
        Hoist.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().hoistPos.ToString();

        GameObject ControlHoist = GameObject.Find("ControlHoist");
        ControlHoist.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().controlHoist.ToString();

        GameObject Bridge = GameObject.Find("Bridge");
        Bridge.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().bridgePos.ToString();

        GameObject ControlBridge = GameObject.Find("ControlBridge");
        ControlBridge.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().controlBridge.ToString();

        GameObject Trolley = GameObject.Find("Trolley");
        Trolley.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().trolleyPos.ToString();

        GameObject ControlTrolley = GameObject.Find("ControlTrolley");
        ControlTrolley.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().controlTrolley.ToString();

        GameObject Load = GameObject.Find("Load");
        Load.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().load.ToString();

        //GameObject Sway = GameObject.Find("Sway");
        //Sway.GetComponent<Text>().text = "Sway: " + connection.GetComponent<SingleThread>().SwayControl.ToString();

        //GameObject Microspeed = GameObject.Find("Microspeed");
        //Microspeed.GetComponent<Text>().text = "Microspeed: " + connection.GetComponent<SingleThread>().MicroSpeed.ToString();

        //GameObject Ropeangle = GameObject.Find("Ropeangle");
        //Ropeangle.GetComponent<Text>().text = "Ropeangle: " + connection.GetComponent<SingleThread>().RopeAngleFeaturesBypass.ToString();

        //GameObject Inching = GameObject.Find("Inching");
        //Inching.GetComponent<Text>().text = "Inching: " + connection.GetComponent<SingleThread>().Inching.ToString();

        GameObject TargetHoist = GameObject.Find("TargetHoist");
        TargetHoist.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().hoistTarget.ToString();

        GameObject TargetTrolley = GameObject.Find("TargetTrolley");
        TargetTrolley.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().trolleyTarget.ToString();

        GameObject TargetBridge = GameObject.Find("TargetBridge");
        TargetBridge.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().bridgeTarget.ToString();

        GameObject WatchDog = GameObject.Find("WatchDog");
        WatchDog.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().watchdog.ToString();

        GameObject WatchDogFault = GameObject.Find("WatchDogFault");
        if (connection.GetComponent<SingleThreadLessQuery>().WatchdogFault == false)
        {
            WatchDogFault.GetComponent<TextMesh>().text = "ok!";
            WatchDogFault.GetComponent<TextMesh>().color = Color.green;
        }
        else
        {
            WatchDogFault.GetComponent<TextMesh>().text = "fail!";
            WatchDogFault.GetComponent<TextMesh>().color = Color.red;
        }

        //GameObject TargetPositioningStart = GameObject.Find("TargetPositioningStart");
        //WatchDog.GetComponent<Text>().text = "TargetPositioning: " + connection.GetComponent<testMultiThread>().startTargetPositioning;

        GameObject WatchdogStart = GameObject.Find("WatchdogStart");
        WatchdogStart.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().startWatchdog.ToString();

        GameObject ControlStart = GameObject.Find("ControlStart");
        ControlStart.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().startControl.ToString();

        GameObject MonitorStart = GameObject.Find("MonitorStart");
        MonitorStart.GetComponent<TextMesh>().text = connection.GetComponent<SingleThreadLessQuery>().startMonitor.ToString();


        // for debug connection (HttpRequest) in HoloLens:

        //GameObject WatchdogLog = GameObject.Find("WatchdogLog");
        //WatchdogLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().watchdogQueryLog;

        //GameObject MonitorLog = GameObject.Find("MonitorLog");
        //MonitorLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().monitorQueryLog;

        //GameObject ControlLog = GameObject.Find("ControlLog");
        //ControlLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().controlQueryLog;



    }

}
