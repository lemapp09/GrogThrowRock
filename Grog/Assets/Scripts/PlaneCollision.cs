using BezierSolution;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneCollision : MonoBehaviour
{
    [SerializeField] 
    private BezierWalkerWithSpeed _splineWalker;

    public void OnCollisionEnter(Collision collision) {
        if ( this.GetComponent<Rigidbody>() != null) {
            Rigidbody rigidBody = this.GetComponent<Rigidbody>();
            Debug.Log("Plane Collided with " + collision.gameObject.name);
            Debug.Log("Rock's Velocity " + rigidBody.velocity);
            Debug.Log("Rock's Accumulated Force " + rigidBody.GetAccumulatedForce());

            if (collision.gameObject.CompareTag("Rock")) {
             _splineWalker.enabled = false;
             rigidBody.useGravity = true;
             rigidBody.isKinematic = false;
            }
        }
    }
    
}
