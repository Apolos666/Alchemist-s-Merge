using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    private float _startTime;
    private bool _isGameActive;

    public float PlayTime { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        EventBus.Subscribe<PlayerDefeatEvent>(OnPlayerDefeat);
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    private void OnDestroy()
    {
        EventBus.Unsubscribe<PlayerDefeatEvent>(OnPlayerDefeat);
    }

    private void OnApplicationQuit()
    {
        SaveSystem.Instance.SaveGame();
    }

    private void Update()
    {
        if (_isGameActive)
        {
            PlayTime = Time.time - _startTime;
        }
    }

    private void InitializeGame()
    {
        var isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;
        
        if (isNewGame)
        {
            SaveSystem.Instance.ClearSaveData();
            PlayerPrefs.DeleteKey("IsNewGame");
        }
        else
        {
            SaveSystem.Instance.LoadGame();
        }
        
        StartGame();
    }

    private void StartGame()
    {
        _startTime = Time.time;
        PlayTime = 0f;
        _isGameActive = true;
    }

    public void ResetGame()
    {
        SaveSystem.Instance.ClearSaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("IsNewGame", 1);
    }

    private void OnPlayerDefeat(PlayerDefeatEvent evt)
    {
        _isGameActive = false;
        Debug.Log($@"Game Over! Player survived for {TimeSpan.FromSeconds(PlayTime):mm\:ss\.ff}");
    }
}