﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public float blinkTime = 0.7f;
    public bool isNormallyOn = true;

    public bool IsBlinking { get; set; } = false;

    private Image _image;
    private TextMeshProUGUI _text;
    private float _timer;
    private TextMeshProUGUI _childText;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _image = GetComponent<Image>();
        _childText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsBlinking)
        {
            if (_image)
                _image.enabled = isNormallyOn;

            if (_text)
                _text.enabled = isNormallyOn;

            if (_childText)
                _childText.enabled = isNormallyOn;
        }
        else
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                if (_image)
                    _image.enabled = !_image.enabled;

                if (_text)
                    _text.enabled = !_text.enabled;

                if (_childText)
                    _childText.enabled = !_childText.enabled;

                _timer = blinkTime;
            }
        }
    }
}
