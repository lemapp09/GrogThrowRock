using System;
using System.Collections;
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

    [SerializeField]
    private AudioClip[] _crashClips;

    [SerializeField]
    private GameObject _flames;

    // Sound Effect from
    // <a href="https://pixabay.com/sound-effects/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=14513">Pixabay</a>

<<<<<<< HEAD
    [SerializeField]
    private AudioClip[] _crashClips;

    [SerializeField]
    private GameObject _flames;

    private bool _planeHasBeenHit = false;
   
=======
>>>>>>> c946356079012235524b2300b7ef50fb22c54535
    private void OnEnable() {
        _audioSource.Play();
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
<<<<<<< HEAD
        StartCoroutine(GrowPlane());
=======

>>>>>>> c946356079012235524b2300b7ef50fb22c54535
        if (_flames)
            _flames.SetActive(false);
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
        if (_flames)
            _flames.SetActive(false);
    }
    
    public void OnCollisionEnter(Collision collision) {
        if ( this.GetComponent<Rigidbody>() != null) {
            Rigidbody rigidBody = this.GetComponent<Rigidbody>();

            Rock rock = collision.gameObject.GetComponent<Rock>();
            //if (collision.gameObject.CompareTag("Rock")) {
            if (rock) {
                _splineWalker.enabled = false;
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
                Vector3 vector = (this.transform.position - _xrOrigin.transform.position).normalized * 75f;
                rigidBody.AddForce(vector.x, vector.y, vector.z, ForceMode.Impulse );
<<<<<<< HEAD
                _audioSource.Stop();
=======

>>>>>>> c946356079012235524b2300b7ef50fb22c54535
                if(_crashClips.Length > 0)
                    _audioSource.PlayOneShot(_crashClips[UnityEngine.Random.Range(0, _crashClips.Length - 1)]);

                if (_flames)
                    _flames.SetActive(true);

                rock.GoHome();
            }
        }

        if (!_planeHasBeenHit) {
            GameMaster.Instance.PlaneCrash(_planeID);
            _planeHasBeenHit = true;
        }
    }

    private IEnumerator GrowPlane()
    {
        float scale = 0.001f;
        this.transform.localScale = Vector3.one * scale;
        while (scale < 1)
        {
            scale += 0.01f;
            this.transform.localScale = Vector3.one * scale;
            yield return new WaitForSeconds(1f);
        }
    }
    
    
    private IEnumerator RemoveDownPlane() {
        Debug.Log("Plane #" + _planeID + " has started crash countdown");
        yield return new WaitForSeconds(1f);
        Debug.Log("Plane #" + _planeID + " has finished crash countdown");
        this.gameObject.SetActive(false);
    }
}
