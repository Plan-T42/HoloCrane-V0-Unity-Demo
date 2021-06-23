using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//literally for global variables

public class Args : MonoBehaviour {
    public static double HOIST_OFFSET=1.55;
    public static double BRIDGE_OFFSET = 15.96;
    public static double TROLLEY_OFFSET = 3.38;

    /*public static string AALTOHOLOCRANE_DIGITALTWIN_IP = "192.168.1.121";
     public static string AALTOHOLOCRANE_WS_ADDRESS = "ws://192.168.1.121:3000/socket.io/?EIO=3&transport=websocket";*/

    public static string AALTOHOLOCRANE_DIGITALTWIN_IP = "192.168.201.10";
    public static string AALTOHOLOCRANE_WS_ADDRESS = "ws://192.168.201.10:3000/socket.io/?EIO=3&transport=websocket";

    /*public static string AALTOHOLOCRANE_DIGITALTWIN_IP = "10.100.53.167";
    public static string AALTOHOLOCRANE_WS_ADDRESS = "ws://10.100.53.167:3000/socket.io/?EIO=3&transport=websocket";*/
}
