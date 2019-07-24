using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class ThrustIndicator : MonoBehaviour
{
    public Thruster thruster;
    [Header("Shake")]
    public float shakeThreshold;
    public float shakeFrequency;
    public Vector3 maximumAngularShake;
    public float seed;
    public float recoverySpeed = 1.5f;

    private float _trauma = 1;
    private Image _image;
    private TextMeshProUGUI _text;
    private Blink _blink;

    private void Awake()
    {
        seed = UnityEngine.Random.value;
    }

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _blink = GetComponent<Blink>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * shakeFrequency) * 2 - 1)) * _trauma);

        _trauma = Mathf.Clamp01(_trauma - recoverySpeed * Time.deltaTime);
    }

    public void UpdateIndicator(float currentThrust, float modifier)
    {
        float roundedCurrentThrust = modifier == 0 ? 0 : (float)Math.Round(currentThrust, 1);

        if (thruster.CurrentThrust > shakeThreshold)
        {
            _trauma = roundedCurrentThrust;
        }

        _image.fillAmount = roundedCurrentThrust;
        _image.color = Color.HSVToRGB(roundedCurrentThrust.Remap(0f, 1f, 0.1f, 1f), 1, 1);

        _text.text = Mathf.RoundToInt(modifier == 0 ? 0 : currentThrust * 100).ToString();
        _blink.IsBlinking = modifier <= 0;
    }
}
