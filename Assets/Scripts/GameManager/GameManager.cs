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
    
    private void OnDestroy()
    {
        EventBus.Unsubscribe<PlayerDefeatEvent>(OnPlayerDefeat);
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (_isGameActive)
        {
            PlayTime = Time.time - _startTime;
        }
    }

    public void StartGame()
    {
        _startTime = Time.time;
        PlayTime = 0f;
        _isGameActive = true;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnPlayerDefeat(PlayerDefeatEvent evt)
    {
        _isGameActive = false;
        Debug.Log($"Game Over! Player survived for {TimeSpan.FromSeconds(PlayTime):mm\\:ss\\.ff}");
    }
}