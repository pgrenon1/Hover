using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public float radius = 40;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var inRange = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in inRange)
        {
            //find nearest
            //push it to targetgroup
            //if empty => remove
            //potentiometer on the sides
            //
        }
    }
}
