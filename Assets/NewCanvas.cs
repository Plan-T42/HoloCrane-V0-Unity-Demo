using UnityEngine;
using UnityEngine.UI;

public class NewCanvas : MonoBehaviour
{
    GameObject connection;

    // Start is called before the first frame update
    void Awake()
    {
        connection = GameObject.Find("TestConnection");
        connection.GetComponent<NewMultiThread>().startWatchdog = true;
        connection.GetComponent<NewMultiThread>().startMonitor = true;
    }
    private void FixedUpdate()
    {
        GameObject Hoist = GameObject.Find("Hoist");
        Hoist.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().hoistPos.ToString();

        GameObject ControlHoist = GameObject.Find("ControlHoist");
        ControlHoist.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().controlHoist.ToString();

        GameObject Bridge = GameObject.Find("Bridge");
        Bridge.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().bridgePos.ToString();

        GameObject ControlBridge = GameObject.Find("ControlBridge");
        ControlBridge.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().controlBridge.ToString();

        GameObject Trolley = GameObject.Find("Trolley");
        Trolley.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().trolleyPos.ToString();

        GameObject ControlTrolley = GameObject.Find("ControlTrolley");
        ControlTrolley.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().controlTrolley.ToString();

        GameObject Load = GameObject.Find("Load");
        Load.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().load.ToString();

        GameObject TargetHoist = GameObject.Find("TargetHoist");
        TargetHoist.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().hoistTarget.ToString();

        GameObject TargetTrolley = GameObject.Find("TargetTrolley");
        TargetTrolley.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().trolleyTarget.ToString();

        GameObject TargetBridge = GameObject.Find("TargetBridge");
        TargetBridge.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().bridgeTarget.ToString();

        GameObject WatchDog = GameObject.Find("WatchDog");
        WatchDog.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().watchdog.ToString();

        GameObject WatchDogFault = GameObject.Find("WatchDogFault");
        if (connection.GetComponent<NewMultiThread>().WatchdogFault == false)
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
        WatchdogStart.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().startWatchdog.ToString();

        GameObject ControlStart = GameObject.Find("ControlStart");
        ControlStart.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().startControl.ToString();

        GameObject MonitorStart = GameObject.Find("MonitorStart");
        MonitorStart.GetComponent<TextMesh>().text = connection.GetComponent<NewMultiThread>().startMonitor.ToString();


        // for debug connection (HttpRequest) in HoloLens:

        //GameObject WatchdogLog = GameObject.Find("WatchdogLog");
        //WatchdogLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().watchdogQueryLog;

        //GameObject MonitorLog = GameObject.Find("MonitorLog");
        //MonitorLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().monitorQueryLog;

        //GameObject ControlLog = GameObject.Find("ControlLog");
        //ControlLog.GetComponent<Text>().text = connection.GetComponent<testMultiThread>().controlQueryLog;



    }

}
