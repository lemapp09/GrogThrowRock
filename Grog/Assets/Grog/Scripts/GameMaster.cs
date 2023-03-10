using System;
using System.Collections;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;


public enum Delay
{
    None,
    OneSecond,
    TwoSeconds,
    ThreeSeconds,
}

public class GameMaster : MonoBehaviour
{
    [SerializeField] 
    private Plane[] _planes;

    [SerializeField] 
    private AudioSource _audioSource;

    private AudioSource _grogAudioSource;

    //Music by
    //<a href="https://pixabay.com/users/audiocoffee-27005420/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Denys Kyshchuk</a> from <a href="https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=116705">Pixabay</a>

    public XROrigin XrOrigin;
    public GameObject Rocks;

    public AudioClip[] PlaneHitClips;
    public AudioClip[] PlaneExplodeClips;

    public AudioClip GrogClipGood;
    public AudioClip GrogClipDelicious;
    public AudioClip GrogClipThoughtful;
    public AudioClip GrogClipSuprised;
    public AudioClip GrogClipHuh;

    public AudioClip GameClipLevelComplete;
    public AudioClip GameClipAreaComplete;

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

    WaitForSeconds _wait1Sec = new WaitForSeconds(1.0f);
    WaitForSeconds _wait2Sec = new WaitForSeconds(2.0f);
    WaitForSeconds _wait3Sec = new WaitForSeconds(3.0f);


    void Awake()
    {
        Instance = this; 
        if (_planes.Length > 0 )
        {
            Instantiate(_planes[_currentPlaneId]);
            _planeCount = _level * _planeMultiplePerLevel;
        }
    }

    private void Start()
    {
        _grogAudioSource = XrOrigin.GetComponent<AudioSource>();

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

    void AllRocksGoHome()
    {
        for (int i = 0; i < Rocks.transform.childCount; i++)
        {
            Rock rock = Rocks.transform.GetChild(i).GetComponent<Rock>();
            if (rock)
                rock.GoHome();
        }
    }

    IEnumerator PlayGrogSoundWithDelay(AudioClip clip, Delay delay)
    {
        WaitForSeconds wait = null;

        switch (delay)
        {
            case Delay.OneSecond:
                wait = _wait1Sec;
                break;

            case Delay.TwoSeconds:
                wait = _wait2Sec;
                break;

            case Delay.ThreeSeconds:
                wait = _wait3Sec;
                break;
        }

        if(wait != null)
            yield return wait;

        _grogAudioSource.PlayOneShot(clip);
    }


    public void GrogSoundGood(Delay delay = Delay.None)
    {
        StartCoroutine(PlayGrogSoundWithDelay(GrogClipGood, delay));
    }

    public void GrogSoundDelicious(Delay delay = Delay.None)
    {
        StartCoroutine(PlayGrogSoundWithDelay(GrogClipDelicious, delay));
    }

    public void GrogSoundThoughtful(Delay delay = Delay.None)
    {
        StartCoroutine(PlayGrogSoundWithDelay(GrogClipThoughtful, delay));
    }

    public void GrogSoundSurprised(Delay delay = Delay.None)
    {
        StartCoroutine(PlayGrogSoundWithDelay(GrogClipSuprised, delay));
    }

    public void GrogSoundHuh(Delay delay = Delay.None)
    {
        StartCoroutine(PlayGrogSoundWithDelay(GrogClipHuh, delay));
    }

    public void GameSoundLevelComplete(Delay delay = Delay.None)
    {
        StartCoroutine(PlayGrogSoundWithDelay(GameClipLevelComplete, delay));
    }

    public void GameSoundAreaComplete(Delay delay = Delay.None)
    {
        StartCoroutine(PlayGrogSoundWithDelay(GameClipAreaComplete, delay));
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
        
        if (_numberOfPlanesHit > 0) {
            WonOrLose = "Won";
        }  else {
            WonOrLose = "Lost";
        } 

        _UISample.SetActive(true);
        _headerText.text = "Level " + level;
        _modalText.text = "Grog has " + WonOrLose + " Level " + level +
                               "\n It took " + TimeSpan.FromSeconds(_levelTimer).Minutes + 
                               ":" + TimeSpan.FromSeconds(_levelTimer).Seconds +
                               "\n Grog took down " + _numberOfPlanesHit + " planes."+
                               "\n Grog's score is " + _score;
        
        foreach (GameObject firework in _fireworks)
        {
            firework.SetActive(true);
        } 
        GameSoundLevelComplete(Delay.TwoSeconds);
        GrogSoundDelicious(Delay.ThreeSeconds);
        AllRocksGoHome();
        
        yield return new WaitForSeconds(5f);
        
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
