using UnityEngine;
using System;
using System.Reflection;

/**
 * Derives basic logic from
 * https://github.com/DWilches/SerialCommUnity
 */

public class ArduinoSerialComm : MonoBehaviour
{
    #region
    private static ArduinoSerialComm _instance;
    public static ArduinoSerialComm Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    //private CombatManager _combatManager;
    //public CombatManager combatManager { get { if (_combatManager == null) { _combatManager = FindObjectOfType<CombatManager>(); } return _combatManager; } }

    //private GridSelector _gridSelector;
    //public GridSelector gridSelector { get { if (_gridSelector == null) { _gridSelector = FindObjectOfType<GridSelector>(); } return _gridSelector; } }

    //private ResourceManager _resourceManager;
    //public ResourceManager resourceManager { get { if (_resourceManager == null) { _resourceManager = FindObjectOfType<ResourceManager>(); } return _resourceManager; } }

    //private IntroManager _introManager;
    //public IntroManager introManager { get { if (_introManager == null) { _introManager = FindObjectOfType<IntroManager>(); } return _introManager; } }

    public bool Connected { get; set; }
    public GameObject controllerParent;
    public SerialController serialController;

    public Thruster ThrusterFrontRight { get; set; }
    public Thruster ThrusterBackRight { get; set; }
    public Thruster ThrusterFrontLeft { get; set; }
    public Thruster ThrusterBackLeft { get; set; }

    private Ship _ship;

    //public int currentScreen = 0;
    //public bool firstChar = true;
    //public string[] lcdTopTexts;
    //public string[] lcdBottomTexts;
    //public AudioClip sSelectorClip;
    //public AudioClip[] keysClip;
    //public AudioClip powerOnClip;

    void Start()
    {
        // default to serial controller object
        if (controllerParent == null)
        {
            serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        }
        else
        {
            serialController = controllerParent.GetComponent<SerialController>();
        }
        _ship = FindObjectOfType<Ship>();
        ThrusterFrontRight = _ship.frontRightThruster;
        ThrusterBackRight = _ship.backRightThruster;
        ThrusterFrontLeft = _ship.frontLeftThruster;
        ThrusterBackLeft = _ship.backLeftThruster;
        //lcdTopTexts = new string[4];
        //lcdBottomTexts = new string[4];
    }

    //internal void UpdateTotalAmount(int tractorBeamID, Source harvestedSource)
    //{
    //    string serialMsg;
    //    serialMsg = "TA " + tractorBeamID + " " + harvestedSource.amount;
    //    Debug.Log("Sending Serial Message: " + serialMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //internal void UpdateCurrentAmount(int tractorBeamID, Source harvestedSource)
    //{
    //    string serialMsg;
    //    serialMsg = "CA " + tractorBeamID + " " + harvestedSource.amount;
    //    Debug.Log("Sending Serial Message: " + serialMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //internal void Ready()
    //{
    //    string serialMsg;
    //    serialMsg = "CA ";
    //    Debug.Log("Sending Serial Message: " + serialMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //internal void ReadAll()
    //{
    //    string serialMsg;
    //    serialMsg = "RA ";
    //    Debug.Log("Sending Serial Message: " + serialMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //internal void BeamOff(int tractorBeamID)
    //{
    //    string serialMsg;
    //    serialMsg = "OFF " + tractorBeamID;
    //    Debug.Log("Sending Serial Message: " + serialMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //internal void Depleated(int tractorBeamID)
    //{
    //    string serialMsg;
    //    serialMsg = "DEP " + tractorBeamID;
    //    Debug.Log("Sending Serial Message: " + serialMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //void Update()
    //{

    //}

    void OnMessageArrived(string msg)
    {
        if (msg[0] == 'K')
        {
            if (msg[1] == 'L')
                _ship.KeyLeft = msg[2] == '1';
            else if (msg[1] == 'R')
                _ship.KeyRight = msg[2] == '1';
        }
        else if (msg[0] == 'P')
        {
            if (msg[1] == 'L')
                _ship.PushLeft = msg[2] == '1';
            else if (msg[1] == 'R')
                _ship.PushRight = msg[2] == '1';
        }
        else
        {
            GameManager.Instance.FirstMessageRecieved = true;
            var split = msg.Split(',');
            ThrusterFrontLeft.CurrentThrust = float.Parse(split[0]).Remap(0, 1023, 0, 1);
            ThrusterBackLeft.CurrentThrust = float.Parse(split[1]).Remap(0, 1023, 0, 1);
            ThrusterBackRight.CurrentThrust = float.Parse(split[2]).Remap(0, 1023, 0, 1);
            ThrusterFrontRight.CurrentThrust = float.Parse(split[3]).Remap(0, 1023, 0, 1);
        }

        //var valA = msg.Substring(indexofA + 1, msg.IndexOf("B") - indexofA + 1);
        //Debug.Log(valA);
        //var valB = msg.Substring(msg.IndexOf("B") + 1, msg.IndexOf("C") - msg.IndexOf("B"));
        //Debug.Log(valB);
        //var valC = msg.Substring(msg.IndexOf("C") + 1, msg.IndexOf("D") - msg.IndexOf("C"));
        //Debug.Log(valC);
        //var valD = msg.Substring(msg.IndexOf("D") + 1);
        //Debug.Log(valD);
        //var indexofB = ;
        //var indexofC;
        //var indexofD;
        //switch (msg.Substring(0, 1))
        //{
        //    case "A":
        //        thrusterA.Thrust = float.Parse(msg.Substring(1));
        //        break;
        //    case "B":
        //        thrusterB.Thrust = float.Parse(msg.Substring(1));
        //        break;
        //    case "C":
        //        thrusterC.Thrust = float.Parse(msg.Substring(1));
        //        break;
        //    case "D":
        //        thrusterD.Thrust = float.Parse(msg.Substring(1));
        //        break;
        //    default:
        //        Debug.Log("Unhandled message arrived: " + msg);
        //        break;
        //}

        //Debug.Log("Unhandled message arrived: " + msg);
    }

    public void GetSwitchStatus()
    {
        SendSerialMessage("state");
    }

    public void SendSerialMessage(string msg)
    {
        serialController.SendSerialMessage(msg);
    }

    //// Invoked when a line of data is received from the serial device.
    //void OnMessageArrived(string msg)
    //{
    //    if (!GameManager.instance.intro)
    //    {
    //        if (msg[0] == 's')
    //        {
    //            S(msg);
    //        }
    //        else if (msg[0] == 'r')
    //        {
    //            R(msg);
    //        }
    //        else if (msg[0] == 'k')
    //        {
    //            K(msg);
    //        }
    //        else if (msg[0] == 'a' || msg[0] == 'b' || msg[0] == 'c' || msg[0] == 'd')
    //        {
    //            Abcd(msg);
    //        }
    //        else if (msg[0] == 'e')
    //        {
    //            E(msg);
    //        }
    //        else if (msg[0] == 'l')
    //        {
    //            L(msg);
    //        }
    //        else
    //        {
    //            Debug.Log("Unhandled message arrived: " + msg);
    //        }
    //    }
    //    else
    //    {
    //        if (msg[0] == 'l')
    //        {
    //            if (msg[1] == '1')
    //            {
    //                AudioManager.instance.Play(powerOnClip, false);
    //                introManager.BeginGame();
    //            }
    //        }
    //    }
    //}

    //// Key handler
    //void E(string data)
    //{
    //    if (data[1] == '0')
    //    {
    //        combatManager.EnginesOff();
    //    }
    //    else if (data[1] == '1')
    //    {
    //        combatManager.EnginesOn();
    //    }
    //}

    //// Red illuminated switch handler
    //void L(string data)
    //{

    //    if (data[1] == '0')
    //    {
    //        combatManager.LaunchOff();
    //    }
    //    else if (data[1] == '1')
    //    {
    //        if (!combatManager.enginesOn)
    //        {
    //            //StartCoroutine(combatManager.Warn("To create ships, turn off 'Launch' (red switch/L)! To launch a wave, use 'Engines' (key switch/E) and 'Launch'!"));
    //            combatManager.LaunchOn();
    //            return;
    //        }

    //        combatManager.LaunchOn();
    //    }
    //}

    //// Switches handler
    //public void Abcd(string data)
    //{
    //    Debug.Log("Switch Pressed: " + data);
    //    int beamIndex = 0;
    //    switch (data[0])
    //    {
    //        case 'a':
    //            beamIndex = 0;
    //            break;
    //        case 'b':
    //            beamIndex = 1;
    //            break;
    //        case 'c':
    //            beamIndex = 2;
    //            break;
    //        case 'd':
    //            beamIndex = 3;
    //            break;
    //    }

    //    if (data[1] == '1')
    //    {
    //        resourceManager.tractorBeams[beamIndex].BeamOn(lcdTopTexts[beamIndex]);
    //    }
    //    else if (data[1] == '0')
    //    {
    //        resourceManager.tractorBeams[beamIndex].BeamOff();
    //        ClearRow(beamIndex, 0);
    //    }
    //}

    //// Arrow Selector handler
    //public void S(string data)
    //{
    //    Debug.Log("Arrow Selector Handler function received: " + data);

    //    //if (data[1] > 3) return;

    //    switch (data[1])
    //    {
    //        case '0':
    //            currentScreen = 0;
    //            NoBlinkAll();
    //            if (resourceManager.tractorBeams[0].inUse)
    //                return;

    //            StartBlink(0);
    //            AudioManager.instance.Play(sSelectorClip, false);
    //            ClearRow(0, 0);
    //            break;
    //        case '1':
    //            currentScreen = 1;
    //            NoBlinkAll();
    //            if (resourceManager.tractorBeams[1].inUse)
    //                return;

    //            StartBlink(1);
    //            AudioManager.instance.Play(sSelectorClip, false);
    //            ClearRow(1, 0);
    //            break;
    //        case '2':
    //            currentScreen = 2;
    //            NoBlinkAll();
    //            if (resourceManager.tractorBeams[2].inUse)
    //                return;

    //            StartBlink(2);
    //            AudioManager.instance.Play(sSelectorClip, false);
    //            ClearRow(2, 0);
    //            break;
    //        case '3':
    //            currentScreen = 3;
    //            NoBlinkAll();
    //            if (resourceManager.tractorBeams[3].inUse)
    //                return;

    //            StartBlink(3);
    //            AudioManager.instance.Play(sSelectorClip, false);
    //            ClearRow(3, 0);
    //            break;
    //    }

    //    firstChar = true;
    //}

    //void NoBlinkAll()
    //{
    //    string debugMsg;
    //    string serialMsg;

    //    serialMsg = "NBA";
    //    debugMsg = "Sending serial message: " + serialMsg;
    //    Debug.Log(debugMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //void StartBlink(int currentScreen)
    //{
    //    string debugMsg;
    //    string serialMsg;

    //    serialMsg = "SB " + currentScreen.ToString();
    //    debugMsg = "Sending serial message: " + serialMsg;
    //    Debug.Log(debugMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    //// Round Selector handler
    //public void R(string data)
    //{
    //    Debug.Log("Round Selector Handler function received: " + data);
    //    string indexString = data.Remove(0, 1);
    //    int shipIndex;
    //    Int32.TryParse(indexString, out shipIndex);

    //    if (shipIndex > GameManager.instance.hangarManager.ships.Count - 1)
    //        return;

    //    gridSelector.selectedShipIndex = shipIndex;
    //    gridSelector.UpdateSelectedShip();
    //}

    //// Keyboard handler
    //public void K(string data)
    //{
    //    if (resourceManager.tractorBeams[currentScreen].inUse) return;

    //    switch (data[1])
    //    {
    //        case '0':
    //            AudioManager.instance.Play(keysClip[0], false);
    //            break;
    //        case '1':
    //            AudioManager.instance.Play(keysClip[1], false);
    //            break;
    //        case '2':
    //            AudioManager.instance.Play(keysClip[2], false);
    //            break;
    //        case '3':
    //            AudioManager.instance.Play(keysClip[3], false);
    //            break;
    //        case '4':
    //            AudioManager.instance.Play(keysClip[4], false);
    //            break;
    //        case '5':
    //            AudioManager.instance.Play(keysClip[5], false);
    //            break;
    //        case '6':
    //            AudioManager.instance.Play(keysClip[6], false);
    //            break;
    //        case '7':
    //            AudioManager.instance.Play(keysClip[7], false);
    //            break;
    //        case '8':
    //            AudioManager.instance.Play(keysClip[8], false);
    //            break;
    //        case '9':
    //            AudioManager.instance.Play(keysClip[9], false);
    //            break;
    //        case 'A':
    //            AudioManager.instance.Play(keysClip[10], false);
    //            break;
    //        case 'B':
    //            AudioManager.instance.Play(keysClip[11], false);
    //            break;
    //        case 'C':
    //            AudioManager.instance.Play(keysClip[12], false);
    //            break;
    //        case 'D':
    //            AudioManager.instance.Play(keysClip[13], false);
    //            break;
    //        case '#':
    //            AudioManager.instance.Play(keysClip[14], false);
    //            break;
    //        case '*':
    //            AudioManager.instance.Play(keysClip[15], false);
    //            break;
    //    }

    //    if (firstChar)
    //    {
    //        ClearRow(currentScreen, 0);
    //        lcdTopTexts[currentScreen] = null;
    //    }

    //    WriteCodeChar(currentScreen, data[1]);

    //    lcdTopTexts[currentScreen] = lcdTopTexts[currentScreen] + data[1];
    //    firstChar = false;
    //}

    //private void WriteCodeChar(int lcdIndex, char c)
    //{
    //    string debugMsg;
    //    string serialMsg;

    //    serialMsg = "WCC " + lcdIndex.ToString() + " " + c;
    //    debugMsg = "Sending serial message: " + serialMsg;
    //    Debug.Log(debugMsg);

    //    serialController.SendSerialMessage(serialMsg);
    //}

    //public void ClearRow(int lcdIndex, int rowIndex)
    //{
    //    if (rowIndex == 0)
    //        lcdTopTexts[lcdIndex] = null;
    //    else if (rowIndex == 1)
    //        lcdBottomTexts[lcdIndex] = null;

    //    string debugMsg;
    //    string serialMsg;

    //    serialMsg = "CR " + lcdIndex.ToString() + " " + rowIndex.ToString();
    //    debugMsg = "Sending serial message: " + serialMsg;
    //    Debug.Log(debugMsg);
    //    serialController.SendSerialMessage(serialMsg);
    //}

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        Connected = success;
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
}
public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
