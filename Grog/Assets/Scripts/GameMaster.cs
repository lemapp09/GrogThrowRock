using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameMaster : MonoBehaviour
{
    [SerializeField] 
    private Plane[] _planes;

    [SerializeField] 
    private AudioSource _audioSource;
    //Music by
    //<a href="https://pixabay.com/users/audiocoffee-27005420/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Denys Kyshchuk</a> from <a href="https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Pixabay</a>

    public static GameMaster Instance;
    private int _currentPlaneId = 0;

    void Awake() {
        Instance = this; 
        if (_planes.Length > 0 ) {
            _planes[_currentPlaneId].gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        _audioSource.Play();
    }

    private void OnDisable() {
        _audioSource.Stop();
    }


    public void PlaneCrash(int planeID)
    {           
        Debug.Log("Plane #" + planeID + " has crashed!");
        _currentPlaneId++;
        if (_currentPlaneId < _planes.Length) {
            _planes[_currentPlaneId].gameObject.SetActive(true);
        } else {
            Debug.Log("Level I complete");
        }
    }

}
