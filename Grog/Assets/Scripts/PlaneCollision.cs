using System;
using BezierSolution;
using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneCollision : MonoBehaviour
{
    [SerializeField] 
    private BezierWalkerWithSpeed _splineWalker;
    
    [SerializeField] 
    private int _planeID;

    [SerializeField] 
    private XROrigin _xrOrigin;
    
    [SerializeField]
    private AudioSource _audioSource;
    // Sound Effect from
    // <a href="https://pixabay.com/sound-effects/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=14513">Pixabay</a>
    
    private void OnEnable() {
        _audioSource.Play();
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }

    private void OnDisable() {
        _audioSource.Stop();
        if (this.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rigidBody = this.GetComponent<Rigidbody>();
            _splineWalker.enabled = true;
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
        }
    }
    
    public void OnCollisionEnter(Collision collision) {
        if ( this.GetComponent<Rigidbody>() != null) {
            Rigidbody rigidBody = this.GetComponent<Rigidbody>();

            if (collision.gameObject.CompareTag("Rock")) {
             _splineWalker.enabled = false;
             rigidBody.useGravity = true;
             rigidBody.isKinematic = false;
             Vector3 vector = (this.transform.position - _xrOrigin.transform.position).normalized * 75f;
             rigidBody.AddForce(vector.x, vector.y, vector.z, ForceMode.Impulse );
            }
        }
        GameMaster.Instance.PlaneCrash(_planeID);
    }

}
