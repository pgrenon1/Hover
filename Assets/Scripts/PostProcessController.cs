using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class PostProcessController : MonoBehaviour
{
    private PostProcessVolume _volume;
    private ChromaticAberration _cA;

    private float _aberration;

    void Start()
    {
        _volume = GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _cA);
    }

    // Update is called once per frame
    void Update()
    {
        _aberration -= 0.1f * Time.deltaTime;

        if (GameManager.Instance.ship.PushLeft)
        {
            _aberration += .05f * Time.deltaTime;
        }

        if (GameManager.Instance.ship.PushRight)
        {
            _aberration += .05f * Time.deltaTime;
        }

        if (_aberration > 1)
            _aberration = 1;
        else if (_aberration < 0)
            _aberration = 0;

        _aberration += Random.Range(-0.1f, 0.1f) * _aberration;

        _cA.intensity.value = _aberration;


    }
}
