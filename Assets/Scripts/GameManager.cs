using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
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

    public bool isFlat;
    public Ship ship;
    public float winCountdownTime = 1;
    public int startingPoints;

    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timeText;
    public Image warningPanel;
    public TextMeshProUGUI thrustModifierText;
    public TextMeshProUGUI maxAngleText;
    [Space]
    public TextMeshProUGUI fuelWarning;

    public ScorePopup scorePopupPrefab;

    public Coroutine OnTargetCoroutine { get; set; }
    public bool IsCheating { get; set; }
    public bool IsThrustersToZero { get; set; }
    private bool _isReady;
    public bool IsReady
    {
        get { return _isReady; }
        set
        {
            _isReady = value;
            if (value)
                ArduinoSerialComm.Instance.GetSwitchStatus();
        }
    }
    public bool FirstMessageRecieved { get; set; }

    private float _time;
    private float _points;
    private int _score;
    private float _winCountdown;

    void Start()
    {
        //StartCoroutine(Intro());
        //ResetCountdown();
        _points = startingPoints;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                ship.ResetShip();
            }
            else
            {
                RestartGame();

            }
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ship.maxAngle = 30;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ship.maxAngle = 40;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ship.maxAngle = 50;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ship.maxAngle = 60;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ship.maxAngle = 70;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ship.maxAngle = 80;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ship.maxAngle = 90;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ship.maxAngle = 360;
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            ship.thrustModifier -= 0.05f;
        }
        else if (Input.GetKeyDown(KeyCode.Equals))
        {
            ship.thrustModifier += 0.05f;
            if (ship.thrustModifier < 1)
                ship.thrustModifier = 1;
        }

        if (!IsReady && ship.Thrusters.All(thruster => thruster.CurrentThrust < 0.1f))
            IsReady = true;

        if (!IsReady)
            return;

        warningPanel.gameObject.SetActive(!IsReady);

        _time += Time.deltaTime;
        _points -= Time.deltaTime * 5f;
        if (_points < 0)
            _points = 0;

        scoreText.text = _score.ToString();
        pointsText.text = Mathf.RoundToInt(_points).ToString();
        string minutes = Mathf.Floor(_time / 60).ToString("00");
        string seconds = Mathf.Floor(_time % 60).ToString("00");
        timeText.text = string.Format("{0}:{1}", minutes, seconds);
        maxAngleText.text = Mathf.RoundToInt(ship.maxAngle).ToString();
        thrustModifierText.text = Math.Round(ship.thrustModifier, 2).ToString();
    }

    public void PlayerIsOnTarget(Target targetInContact)
    {
        if (OnTargetCoroutine == null)
            OnTargetCoroutine = StartCoroutine(DoOnTarget(targetInContact));
    }

    private IEnumerator DoOnTarget(Target targetInContact)
    {
        yield return new WaitForSeconds(winCountdownTime);

        // If the ship is still on target at the end
        if (ship.OnTargetCounter >= 4)
        {
            yield return new WaitForSeconds(0.5f);

            var popup = Instantiate(scorePopupPrefab, Vector3.zero, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            popup.StartCoroutine(popup.DoPop(_points, targetInContact.Multiplier));

            _score += Mathf.RoundToInt(_points * targetInContact.Multiplier);
            StartOver();
        }

        OnTargetCoroutine = null;
    }

    //public void ResetCountdown()
    //{
    //    HideCountdown();
    //    _winCountdown = winCountdownTime;
    //}


    //public void DecrementCountdown()
    //{
    //    if (!countdownText.enabled)
    //        ShowCountdown();
    //    _winCountdown -= Time.deltaTime;
    //    countdownText.text = string.Format("{0:F2}", _winCountdown);
    //    if (_winCountdown <= 0)
    //    {
    //        Win();
    //    }
    //}

    //private void HideCountdown()
    //{
    //    countdownText.enabled = false;
    //}

    //private void ShowCountdown()
    //{
    //    countdownText.enabled = true;
    //}

    private void StartOver()
    {
        Debug.Log("STARTING OVER!");
        _points = startingPoints;
        Map.Instance.Generate();
        IsThrustersToZero = false;
        ship.ResetShip();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //private IEnumerator Intro()
    //{
    //    IsInTransition = true;

    //    connectionPanel.DOFade(0, 3f);

    //    yield return new WaitForSeconds(3f);

    //    IsInTransition = false;
    //}
}
