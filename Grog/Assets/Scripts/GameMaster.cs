using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] 
    private Plane[] _planes;

    [SerializeField] 
    private AudioSource _audioSource;
    //Music by
    //<a href="https://pixabay.com/users/audiocoffee-27005420/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Denys Kyshchuk</a> from <a href="https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Pixabay</a>
    
    [SerializeField] 
    private float _timeBetweenPlanes = 5f;
    public static GameMaster Instance;
    private int _currentPlaneId = 0;
    private bool _isGameActive = true;
    private bool _isPlaneCrash = false;
    private bool _isTimeUp;

    void Awake() {
        Instance = this;
        StartSendingInPlane();
    }

    private void OnEnable()
    {
        _audioSource.Play();
    }

    private void OnDisable() {
        _isGameActive = false;
        _audioSource.Stop();
    }

    private void StartSendingInPlane() {
        if (_planes.Length > 0) {
            StartCoroutine(ActivatePlane());
        }
    }

    private IEnumerator ActivatePlane() {
        while (_isGameActive == true)
        {
            // Debug.Log("GameMaster is Sending out plane #: " + _currentPlaneId);
            _planes[_currentPlaneId].gameObject.SetActive(true);
            
            for( float timer = _timeBetweenPlanes ; timer >= 0 ; timer -= Time.deltaTime ) {
                if( _isPlaneCrash ) {
                    timer = 0f;
                }
                yield return null ;
            }

            if (!_isPlaneCrash) {
                _planes[_currentPlaneId].gameObject.SetActive(false);
            }

            _currentPlaneId++;
            if (_currentPlaneId > (_planes.Length - 1)) {
                _currentPlaneId = 0;
            }
        }
    }

    public void PlaneCrash(int planeID) {
        _isPlaneCrash = true;
        StartCoroutine(RemoveDownPlane(planeID));
    }

    private IEnumerator RemoveDownPlane(int planeId) {
        yield return new WaitForSeconds(10f);
        _planes[planeId].gameObject.SetActive(false);
        _isPlaneCrash = false;
    }
}
