using System.Collections;
using BezierSolution;
using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlaneCollision : MonoBehaviour
{
    [SerializeField] private BezierWalkerWithSpeed _splineWalker;

    [SerializeField] private int _planeID;

    [SerializeField] private AudioSource _audioSource;

    private AudioSource _hitAudioSource;

    [SerializeField] private GameObject _flames;

    [SerializeField] private GameObject _propeller;

    [SerializeField] private float _propellerSpeed = 20.0f;

    private float _propellerAngle = 0.0f;

    public bool PlaneHasBeenHit;

    // Sound Effect from
    // <a href="https://pixabay.com/sound-effects/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=14513">Pixabay</a>

    public void SoundPlaneHit()
    {
        AudioClip[] planeHitClips = GameMaster.Instance.PlaneHitClips;
        if (planeHitClips.Length > 0)
            _hitAudioSource.PlayOneShot(planeHitClips[UnityEngine.Random.Range(0, planeHitClips.Length - 1)], 0.4f);

    }

    public void SoundPlaneExplode()
    {
        AudioClip[] planeExplodeClips = GameMaster.Instance.PlaneExplodeClips;
        if (planeExplodeClips.Length > 0)
            _hitAudioSource.PlayOneShot(planeExplodeClips[UnityEngine.Random.Range(0, planeExplodeClips.Length - 1)], 0.4f);
    }

    public void Start()
    {
        _hitAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Update()
    {
        if (_propeller)
        {
            float divider = PlaneHasBeenHit ? 10.0f : 1.0f;
            
            _propellerAngle += (_propellerSpeed / divider) * Time.deltaTime;
            _propeller.transform.localRotation = Quaternion.Euler(0, 0, _propellerAngle);
        }
    }

    private void OnEnable()
    {
        _audioSource.Play();
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        StartCoroutine(GrowResizePlane());
        StartCoroutine(LifeSpanOfPlane());

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
            if (rock)
            {
                _splineWalker.enabled = false;
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
                Vector3 vector = (this.transform.position - GameMaster.Instance.XrOrigin.transform.position).normalized * 5f;

                rigidBody.AddForce(vector.x, vector.y, vector.z, ForceMode.Impulse);

                if (_flames)
                    _flames.SetActive(true);

                SoundPlaneHit();
                GameMaster.Instance.GrogSoundSurprised(Delay.ThreeSeconds);

                rock.GoHome();
            }
            else if(collision.gameObject.CompareTag("Ground"))
            {
                SoundPlaneExplode();
            }
        }

        if (!PlaneHasBeenHit) {
            GameMaster.Instance.PlaneCrash(_planeID, true);
            PlaneHasBeenHit = true;
        }

        StartCoroutine(RemoveDownPlane());
    }

    private IEnumerator RemoveDownPlane()
    {
        yield return new WaitForSeconds(15f);
        this.gameObject.SetActive(false);
    }
    
    private IEnumerator LifeSpanOfPlane()
    {
        yield return new WaitForSeconds(30f);
        if (!PlaneHasBeenHit)
        {
            StartCoroutine(ShrinkResizePlane());
        }
    }

    private IEnumerator GrowResizePlane()
    {
        float scale = 0.0005f;
        while (scale < 0.2) {
            this.transform.localScale = Vector3.one * scale;
            scale += 0.0005f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator ShrinkResizePlane()
    {
        float scale = 0.2f;
        while (scale > 0.0005) {
            this.transform.localScale = Vector3.one * scale;
            scale -= 0.0005f;
            yield return new WaitForSeconds(0.01f);
        }
        GameMaster.Instance.PlaneCrash(_planeID, false);
        _audioSource.Stop();
    }
}
