using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthMode : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }


}
