using System;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] 
    private GameObject _spline;

    [SerializeField] 
    private GameObject _plane;

    private void OnEnable() {
        _spline.SetActive(true);
        _plane.SetActive(true);
    }

    private void OnDisable() {
        _spline.SetActive(false);
        _plane.SetActive(false);
    }
}
