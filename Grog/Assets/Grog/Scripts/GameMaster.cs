using System;
using System.Collections;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] 
    private Plane[] _planes;

    [SerializeField] 
    private AudioSource _audioSource;
    //Music by
    //<a href="https://pixabay.com/users/audiocoffee-27005420/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Denys Kyshchuk</a> from <a href="https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Pixabay</a>

    public XROrigin XrOrigin;

    public AudioClip[] PlaneCrashClips;

    public static GameMaster Instance;
    private int _currentPlaneId = 0;
    [SerializeField] private GameObject _UISample;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _modalText;
    [SerializeField] private GameObject[] _fireworks;
    [SerializeField] private int _planeMultiplePerLevel = 4;
    private int _planeCount;
    private int _score;
    private int _numberOfPlanesHit;
    private int _level = 1;
    private float _levelTimer;

    void Awake()
    {
        _modalText.fontSize = 18;
        Instance = this; 
        if (_planes.Length > 0 )
        {
            Instantiate(_planes[_currentPlaneId]);
            _planeCount = _level * _planeMultiplePerLevel;
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

    public void PlaneCrash(int planeID, bool planeHit)
    {
        _currentPlaneId++;
        if (planeHit) {
            _score += 100;
            _numberOfPlanesHit++;
        }
        if (_currentPlaneId < _planeCount) {
            Instantiate(_planes[_currentPlaneId % _planes.Length]);
        } else {
            StartCoroutine(DisplayLevelWin(_level));
        }
    }
    
    private IEnumerator DisplayLevelWin(int level)
    {
        string WonOrLose = "";
        _UISample.SetActive(true);
        _headerText.text = "Level " + level;
        if (_numberOfPlanesHit > 0) {
            WonOrLose = "Won";
        }  else {
            WonOrLose = "Lose";
        } 
        _modalText.text = "Grog has " + WonOrLose + " Level " + level +
                          "\n It took " + TimeSpan.FromSeconds(_levelTimer).Minutes + 
                          ":" + TimeSpan.FromSeconds(_levelTimer).Seconds +
                          "\n Grog took down " + _numberOfPlanesHit + " planes."+
                          "\n Grog's score is " + _score;
        foreach (GameObject firework in _fireworks)
        {
            firework.SetActive(true);
        }
        yield return new WaitForSeconds(10f);
        foreach (GameObject firework in _fireworks)
        {
            firework.SetActive(false);
        }
        _UISample.SetActive(false);
        _level++;
        _planeCount = _level * _planeMultiplePerLevel;
        _currentPlaneId = 0;
        Instantiate(_planes[_currentPlaneId]);
        _levelTimer = 0f;
        _numberOfPlanesHit = 0;
    }
}
