using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using TMPro;
using System;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    [Header("Settings")]
    public ForceMode forceMode;
    public float timeBeforeTurnOver = 2;
    public float cheatSpeed = 0.1f;
    public float startingFuel = 4000;
    public int altitudeZero = 9865;
    public float fuelPerThrust = 0.1f;
    public bool detectUpsideDown = false;
    public float thrustModifier = 1.2f;
    public float fuelWarningBlinkTime = 0.7f;
    public float boostRecoverySpeed = 0.025f;
    public float boostDepletionSpeed = 0.2f;

    [Space]
    public float maxAngle = 30f;
    public float pushbackScalingRatio = 1f;
    public float pushbackTorque = 10f;

    [Header("Thrusters")]
    public Thruster frontRightThruster;
    public Thruster frontLeftThruster;
    public Thruster backRightThruster;
    public Thruster backLeftThruster;
    public AnimationCurve flameIntensityCurve;

    [Header("HUD")]
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI fuelWarning;
    public TextMeshProUGUI fuelPerSecText;
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI xSpeedText;
    public TextMeshProUGUI ySpeedText;
    public TextMeshProUGUI distanceToClosestText;
    public GameObject boostIndicatorLeft;
    public GameObject boostIndicatorRight;
    public Image boostIndicatorLeftFill;
    public Image boostIndicatorRightFill;
    public TextMeshProUGUI restartEngineLeft;
    public TextMeshProUGUI restartEngineRight;

    [Header("Passengers")]
    public int passengers;

    [Header("Boost")]
    public float boostModifierValue = 3f;

    private bool _keyLeft;
    public bool KeyLeft
    {
        get { return _keyLeft; }
        set
        {
            _keyLeft = value;
            _leftEngine = _keyLeft;
        }
    }
    private bool _keyRight;
    public bool KeyRight
    {
        get { return _keyRight; }
        set
        {
            _keyRight = value;
            _rightEngine = _keyRight;
        }
    }
    public bool PushLeft { get; set; }
    public bool PushRight { get; set; }
    public bool IsReseting { get; private set; } = false;
    public List<Thruster> Thrusters { get; set; } = new List<Thruster>();
    public int OnTargetCounter { get; set; }
    public Target TargetInContact { get; set; }
    public float Fuel { get; set; }
    public float LeftModifier { get; set; }
    public float RightModifier { get; set; }

    private bool _leftEngine;
    private bool _rightEngine;
    private float leftBoost = 1;
    private float rightBoost = 1;
    private Rigidbody _rigidbody;
    private Blink _fuelBlink;
    private float _upsideDownCounter;
    private Vector3 _startPosition;
    private Vector3 _startEulers;
    private Vector3 _startScale;
    private float _fuelPerSec;
    private float _altitude;
    private float _xSpeed;
    private float _ySpeed;
    private float _distanceToClosest;
    private float _fuelWarningBlinkTimer;
    private float _previousFuel;

    void Start()
    {
        Fuel = startingFuel;

        _startPosition = transform.position;
        _startEulers = transform.rotation.eulerAngles;
        _rigidbody = GetComponent<Rigidbody>();
        _fuelBlink = fuelWarning.GetComponent<Blink>();

        Thrusters.Add(frontRightThruster);
        Thrusters.Add(frontLeftThruster);
        Thrusters.Add(backRightThruster);
        Thrusters.Add(backLeftThruster);

        StartCoroutine(FuelPerSec());
    }

    private IEnumerator FuelPerSec()
    {
        while (Fuel > 0)
        {
            var fuelUsed = _previousFuel - Fuel;

            _fuelPerSec = fuelUsed / 2;

            _previousFuel = Fuel;

            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {
        UpdateCheatz();

        UpdateOnTarget();

        UpdateClampAngle();

        UpdateThrusters();

        UpdateStats();

        UpdateUI();

        UpdateBoost();

        UpdateRestartGame();
    }

    private void UpdateRestartGame()
    {
        if (transform.position.y < -800f)
        {
            GameManager.Instance.RestartGame();
        }
    }

    private void UpdateBoost()
    {
        if (leftBoost < 1)
        {
            leftBoost += Time.deltaTime * boostRecoverySpeed;
            boostIndicatorLeft.gameObject.SetActive(true);
            boostIndicatorLeftFill.fillAmount = leftBoost;
        }
        else
        {
            leftBoost = 1;
            boostIndicatorLeft.gameObject.SetActive(false);
        }

        if (rightBoost < 1)
        {
            rightBoost += Time.deltaTime * boostRecoverySpeed;
            boostIndicatorRight.gameObject.SetActive(true);
            boostIndicatorRightFill.fillAmount = rightBoost;
        }
        else
        {
            rightBoost = 1;
            boostIndicatorRight.gameObject.SetActive(false);
        }
    }

    private void UpdateThrusters()
    {
        if (_leftEngine)
        {
            if (PushLeft)
            {
                LeftModifier = thrustModifier * boostModifierValue;
                leftBoost -= boostDepletionSpeed * Time.deltaTime;
                if (leftBoost < 0)
                {
                    leftBoost = 0;
                    _leftEngine = false;
                }
            }
            else
                LeftModifier = thrustModifier;
        }
        else
        {
            LeftModifier = 0;
        }

        if (_rightEngine)
        {
            if (PushRight)
            {
                RightModifier = thrustModifier * boostModifierValue;
                rightBoost -= boostDepletionSpeed * Time.deltaTime;
                if (rightBoost < 0)
                {
                    rightBoost = 0;
                    _rightEngine = false;
                }
            }
            else
                RightModifier = thrustModifier;
        }
        else
        {
            RightModifier = 0;
        }

        frontRightThruster.UpdateThrust(RightModifier);
        backRightThruster.UpdateThrust(RightModifier);

        frontLeftThruster.UpdateThrust(LeftModifier);
        backLeftThruster.UpdateThrust(LeftModifier);
    }

    private void UpdateClampAngle()
    {
        var up = transform.up;
        float angle = Vector3.Angle(Vector3.up, up);
        if (angle > maxAngle)
        {
            var axis = Vector3.Cross(Vector3.up, up);
            float pushbackScaling = pushbackScalingRatio * (angle - maxAngle);
            float pushback = pushbackTorque * _rigidbody.mass * pushbackScaling;
            _rigidbody.AddTorque(-axis * pushback);
        }
    }

    private void UpdateOnTarget()
    {
        if (OnTargetCounter >= 4)
        {
            GameManager.Instance.PlayerIsOnTarget(TargetInContact);
        }
    }

    private void UpdateStats()
    {
        if (Fuel <= 0)
        {
            Fuel = 0;
        }
        _altitude = transform.position.y + altitudeZero; // lol
        _xSpeed = ((Vector2)_rigidbody.velocity).x;
        _ySpeed = Vector2.Dot(_rigidbody.velocity, Vector2.up);
        _distanceToClosest = Vector3.Distance(transform.position, GetClosestTarget().position);
    }

    private void UpdateUI()
    {
        fuelText.text = Math.Round(Fuel, 1).ToString();
        fuelPerSecText.text = Math.Round(_fuelPerSec, 1).ToString();
        altitudeText.text = Math.Round(_altitude, 1).ToString();
        xSpeedText.text = Math.Round(_xSpeed, 1).ToString();
        ySpeedText.text = Math.Round(_ySpeed, 1).ToString();
        distanceToClosestText.text = Math.Round(_distanceToClosest, 1).ToString();
        if (Fuel <= 0)
        {
            _fuelBlink.isNormallyOn = true;
            _fuelBlink.IsBlinking = false;
        }
        else
        {
            _fuelBlink.IsBlinking = Fuel < 1000;
        }

        restartEngineLeft.enabled = !_leftEngine;
        restartEngineRight.enabled = !_rightEngine;
    }

    private void UpdateCheatz()
    {
        // CHEATZ
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * cheatSpeed);
            transform.eulerAngles = _startEulers;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * cheatSpeed);
            transform.eulerAngles = _startEulers;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * cheatSpeed);
            transform.eulerAngles = _startEulers;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * cheatSpeed);
            transform.eulerAngles = _startEulers;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!detectUpsideDown)
            return;

        if (Vector3.Dot(transform.up, Vector3.down) > 0.3f)
        {
            //Ship is upsidedown and in contact with something
            _upsideDownCounter += Time.deltaTime;
            if (_upsideDownCounter >= timeBeforeTurnOver && !IsReseting)
            {
                _rigidbody.useGravity = false;
                IsReseting = true;
                transform.DOMoveY(transform.position.y + 10, 2f);
                transform.DORotateQuaternion(Quaternion.identity, 3f).OnComplete(WasTurnedOver);
            }
        }
    }

    private void WasTurnedOver()
    {
        _rigidbody.useGravity = true;
        IsReseting = false;
        _upsideDownCounter = 0;
    }

    public void ResetShip()
    {
        OnTargetCounter = 0;
        transform.position = _startPosition;
        transform.eulerAngles = _startEulers;
    }

    private Transform GetClosestTarget()
    {

        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in Map.Instance.islands)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = potentialTarget.transform;
            }
        }

        return closestTarget;
    }
}
