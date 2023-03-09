using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GrabRight : MonoBehaviour
{
    [Range(0f, 1f)]
    public float GrabValue = 0.0f;
    public bool OverriteWithGrabValue = false;

    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Grab()
    {
        _animator.SetFloat("grab", 0.9f);
    }

    public void UnGrab()
    {
        _animator.SetFloat("grab", 0.0f);
    }

    public void Update()
    {
        if (OverriteWithGrabValue)
            _animator.SetFloat("grab", GrabValue);
    }
}
