using System;
using System.Collections;
using BezierSolution;
using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneCollision : MonoBehaviour
{
    [SerializeField] private BezierWalkerWithSpeed _splineWalker;

    [SerializeField] private int _planeID;

    [SerializeField] private XROrigin _xrOrigin;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _crashClips;

    [SerializeField] private GameObject _flames;

    private bool _planeHasBeenHit;

    // Sound Effect from
    // <a href="https://pixabay.com/sound-effects/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=14513">Pixabay</a>

    private void OnEnable()
    {
        _audioSource.Play();
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        StartCoroutine(ResizePlane());
        if (_flames)
            _flames.SetActive(false);
    }

    private void OnDisable()
    {
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

    public void OnCollisionEnter(Collision collision)
    {
        if (this.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rigidBody = this.GetComponent<Rigidbody>();

            Rock rock = collision.gameObject.GetComponent<Rock>();
            //if (collision.gameObject.CompareTag("Rock")) {
            if (rock)
            {
                _splineWalker.enabled = false;
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
                Vector3 vector = (this.transform.position - _xrOrigin.transform.position).normalized * 75f;
                rigidBody.AddForce(vector.x, vector.y, vector.z, ForceMode.Impulse);

                if (_crashClips.Length > 0)
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

        StartCoroutine(RemoveDownPlane());
    }

    private IEnumerator RemoveDownPlane()
    {
        Debug.Log("Plane #" + _planeID + " has started countdown");
        yield return new WaitForSeconds(5f);
        Debug.Log("Plane #" + _planeID + " has ended countdown");
        this.gameObject.SetActive(false);
    }

    private IEnumerator ResizePlane()
    {
        float scale = 0.001f;
        while (scale < 0.2) {
            this.transform.localScale = Vector3.one * scale;
            scale += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
