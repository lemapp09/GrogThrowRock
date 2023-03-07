using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] 
    private Plane[] _planes;

    [SerializeField] 
    private float _timeBetweenPlanes = 5f;
    public static GameMaster Instance;
    private int _currentPlaneId = 0;
    private bool _isGameActive = true;
    
    void Awake() {
        StartSendingInPlane();
    }

    private void OnDisable() {
        _isGameActive = false;
    }

    private void StartSendingInPlane() {
        if (_planes.Length > 0) {
            StartCoroutine(ActivatePlane());
        }
    }

    private IEnumerator ActivatePlane() {
        while (_isGameActive) {
            _planes[_currentPlaneId].gameObject.SetActive(true);
            yield return new WaitForSeconds(_timeBetweenPlanes);
            _planes[_currentPlaneId].gameObject.SetActive(false);
            _currentPlaneId++;
            if (_currentPlaneId > _planes.Length - 1) _currentPlaneId = 0;
        }
    }

}
