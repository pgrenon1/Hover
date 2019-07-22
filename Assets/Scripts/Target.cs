using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    public Transform passengerParent;

    public float Multiplier { get; set; }
    public int Passengers { get; set; }
    public GameObject passengerPrefab { get; set; }

    private TextMeshPro _multiplierText;

    void Start()
    {
        _multiplierText = GetComponentInChildren<TextMeshPro>();
        //StartCoroutine(SpawnPassengers());
    }

    void Update()
    {
        _multiplierText.text = "x" + Multiplier.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var ship = collision.collider.GetComponentInParent<Ship>();
        if (ship)
        {
            ship.OnTargetCounter += 1;
            ship.TargetInContact = this;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var ship = collision.collider.GetComponentInParent<Ship>();
        if (ship)
        {
            ship.OnTargetCounter -= 1;
            ship.TargetInContact = this;
        }
    }

    private IEnumerator SpawnPassengers()
    {
        for (int i = 0; i < Passengers; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(passengerPrefab, passengerParent);
        }
    }

}
