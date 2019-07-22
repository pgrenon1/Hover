using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float CurrentThrust = 0.2f;
    public MeshRenderer flameRenderer;
    public ThrustIndicator thrustIndicator;

    private Ship _ship;
    private Rigidbody _rigidbody;
    private Material _flameMaterial;
    private Color _color;

    // Start is called before the first frame update
    void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _rigidbody = _ship.GetComponent<Rigidbody>();
        _flameMaterial = GetComponentInChildren<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsReady)
            return;

        UpdateFlame();
    }

    private void UpdateFlame()
    {
        _flameMaterial.SetColor("_OuterColor", Color.HSVToRGB(CurrentThrust.Remap(0f, 1f, 0.1f, 1f), .8f, 1));
        _flameMaterial.SetColor("_OuterColorTop", Color.HSVToRGB(CurrentThrust.Remap(0f, 1f, 0.1f, 1f), .8f, 1));
        _flameMaterial.SetFloat("_FlameIntensity", _ship.flameIntensityCurve.Evaluate(CurrentThrust));
        _flameMaterial.SetFloat("_NoiseSpeed2", CurrentThrust.Remap(0f, 1f, -1f, -2f));
    }

    public void UpdateThrust(float modifier)
    {
        thrustIndicator.UpdateIndicator(CurrentThrust, modifier);

        if (GameManager.Instance.IsReady)
        {
            if (!_ship.IsReseting && _ship.Fuel > 0)
            {
                //Debug.Log(name + " = " + CurrentThrust * modifier);
                _rigidbody.AddForceAtPosition(transform.up * CurrentThrust * modifier, transform.position, _ship.forceMode);
                _ship.Fuel -= CurrentThrust * _ship.fuelPerThrust;
            }
        }

    }

    //private void AddForce()
    //{
    //if (Input.GetKey(KeyCode.Space))
    //{
    //    _rigidbody.AddForceAtPosition(transform.up * 4f * _ship.thrustModifier, transform.position, _ship.forceMode));
    //}
    //else if (!_ship.IsReseting && _ship.Fuel > 0)
    //{
    //    _rigidbody.AddForceAtPosition(transform.up * CurrentThrust * _ship.thrustModifier, transform.position, _ship.forceMode);
    //}
    //}
}
