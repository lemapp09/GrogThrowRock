using System;
using BezierSolution;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneCollision : MonoBehaviour
{
    [SerializeField] 
    private BezierWalkerWithSpeed _splineWalker;
    
    [SerializeField] 
    private int _planeID;
    
    [SerializeField]
    private AudioSource _audioSource;
    // Sound Effect from
    // <a href="https://pixabay.com/sound-effects/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=14513">Pixabay</a>
    private Vector3 _originalPos;
    
    private void Awake() {
        _originalPos = this.transform.position;
        _audioSource.Play();
    }

    public void OnCollisionEnter(Collision collision) {
        if ( this.GetComponent<Rigidbody>() != null) {
            Rigidbody rigidBody = this.GetComponent<Rigidbody>();

            if (collision.gameObject.CompareTag("Rock")) {
             _splineWalker.enabled = false;
             rigidBody.useGravity = true;
             rigidBody.isKinematic = false;
            }
        }
        GameMaster.Instance.PlaneCrash(_planeID);
    }

    private void OnDisable()
    {
        if (this.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rigidBody = this.GetComponent<Rigidbody>();
            _splineWalker.enabled = true;
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            this.transform.position = _originalPos;
        }
    }
}
