using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;

public class WitAutoReactivation : MonoBehaviour
{
    WitService _wit;
    [SerializeField] AudioSource OutputAudio;

    private void Start()
    {
        _wit = GetComponent<WitService>();
        if(_wit != null)
            _wit.Activate();
    }

    void Update()
    {
        if(_wit == null)
            _wit = GetComponent<WitService>();

        if (!_wit.Active)
        {
            if (!OutputAudio.isPlaying)
                _wit.ActivateImmediately();
        }
        else
        {
            if (OutputAudio.isPlaying)
                _wit.DeactivateAndAbortRequest();
        }
    }
}
