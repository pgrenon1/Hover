using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class PostProcessController : MonoBehaviour
{
    private PostProcessVolume _volume;
    private LensDistortion _ld;

    void Start()
    {
        _volume = GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _ld);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
