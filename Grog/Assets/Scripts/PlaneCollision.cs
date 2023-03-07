using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    [SerializeField] 
    private BezierWalkerWithSpeed _splineWalker;

    [SerializeField] 
    private GameObject _redBox;
    private void OnCollisionEnter(Collision collision) {
        if ( collision.gameObject.CompareTag("Rock") ) {
            _splineWalker.enabled = false;
            _redBox.SetActive(true);
        }
    }
}
