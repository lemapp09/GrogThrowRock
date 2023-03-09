using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
class Rock : MonoBehaviour
{
    public float homingStrength = 2.0f;

    public Transform grabPointLeft;
    public Transform grabPointRight;
    public GameObject leftHandTop;

    XRGrabInteractable _grabInteractable;
    SphereCollider _homingCollider;
    Rigidbody _rigidbody;
    AudioSource _audioSource;

    Vector3 _spawnPosition;
    Vector3 _lastHere;
    Vector3 _lastThere;
    Vector3 _lastNormal;
    
    public Material GetMat()
    {
        return GetComponent<Renderer>().material;
    }

    public void SetMat(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }

    public void GrabbedOnLeft()
    {
        _grabInteractable.attachTransform = grabPointLeft;
    }

    public void GrabbedOnRight()
    {
        _grabInteractable.attachTransform = grabPointRight;
    }

    public void StartThrow()
    {
        _homingCollider.enabled = true;
    }

    public void StopThrow()
    {
        _homingCollider.enabled = false;
    }

    public void GoHome()
    {
        _homingCollider.enabled = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.position = _spawnPosition;
    }

    void RockWasGrabbed(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.parent.gameObject == leftHandTop)
            GrabbedOnLeft();
        else
            GrabbedOnRight();
    }

    void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();

        _homingCollider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        if (!_homingCollider.isTrigger)
        {
            Debug.LogError("Homing sphere collider must be a trigger not a physics collision");
            return;
        }

        _grabInteractable.selectEntered.AddListener(RockWasGrabbed) ;

        _spawnPosition = transform.position;

        _homingCollider.enabled = false;
    }

    void Update()
    {
        if (this.transform.position.y < -2)
        {
            GoHome();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StopThrow();
            Debug.Log("Collision object touched ground!");
            if (collision.contacts[0].thisCollider is not SphereCollider)
                StopThrow();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlaneCollision planeCollision = other.gameObject.GetComponent<PlaneCollision>();
        if (planeCollision) // TODO: Check for if plane is downed, then do not do this.
        {
            _lastHere = transform.position;
            _lastThere = other.transform.position;
            _lastNormal = (_lastThere - _lastHere).normalized;

            _rigidbody.AddForce(homingStrength * _lastNormal, ForceMode.Impulse); // Use impulse if adding force not inside FixedUpdate.

            _audioSource.Play();

            Debug.Log("Homing...");

            //plane.OnHoming();
  
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_lastHere, _lastThere);
        Gizmos.DrawSphere(_lastThere, 0.05f);
    }


}
