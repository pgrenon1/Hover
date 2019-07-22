using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGen : MonoBehaviour
{
    public GameObject cloud;
    public int amount = 100;
    public float maxDistance = 200;

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            var c = Instantiate(cloud, transform);
            c.transform.position += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * Random.Range(0, maxDistance);
        }
    }
}
