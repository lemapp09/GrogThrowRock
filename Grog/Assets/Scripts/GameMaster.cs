using System;
using System.Collections;
using TMPro;
using UnityEngine;

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
    [SerializeField] private GameObject _UISample;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _modalText;
    private int _level = 1;
    private float _levelTimer;


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

    private void Update() {
        _levelTimer += Time.deltaTime;
    }

    public void PlaneCrash(int planeID)
    {
        _currentPlaneId++;
        if (_currentPlaneId < _planes.Length) {
            _planes[_currentPlaneId].gameObject.SetActive(true);
        } else {
            StartCoroutine(DisplayLevelWin(_level));
        }
    }
    private IEnumerator DisplayLevelWin(int level)
    {
        _UISample.SetActive(true);
        _headerText.text = "Level " + level;
        _modalText.text = "Grog has won Level " + level +
                          "\n It took " + TimeSpan.FromSeconds(_levelTimer).Minutes + 
                          ":" + TimeSpan.FromSeconds(_levelTimer).Seconds;
        yield return new WaitForSeconds(10f);
        _UISample.SetActive(false);
        _level++;
        _currentPlaneId = 0;
        _planes[_currentPlaneId].gameObject.SetActive(true);
        _levelTimer = 0f;
    }
}
