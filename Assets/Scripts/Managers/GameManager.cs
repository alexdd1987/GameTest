using System;
using System.Collections;
using UnityEngine;

public enum GamePhase
{
    MainMenu,
    Game,
    End
}

public class GameManager : Singleton<GameManager>
{
    public Action<GamePhase> OnGamePhaseChange;
    public GamePhase CurrentPhase { get; private set; }
    [SerializeField] private GameObject _gamePrefab;
    [SerializeField] private GameObject _uiPrefab;

    public bool IsDebug;

    [HideInInspector] public Spawner Spawner;
    [HideInInspector] public bool HasGameStarted;

    private const string HudCanvasTag = "HudCanvas";


    private AudioSource _battleThemeAudioSource;
    private GameObject _game;
    private GameObject _ui;

    private WaitForSeconds _waitForHalfASecond;

    private int _playerScore;

    protected override void Awake()
    {
        _waitForHalfASecond = new WaitForSeconds(0.5f);
        _battleThemeAudioSource = GetComponent<AudioSource>();

        InitGame();
    }

    private void InitGame()
    {
        _playerScore = 0;

        _game = Instantiate(_gamePrefab);

        Spawner = _game.GetComponentInChildren<Spawner>();

        Spawner.OnEnemyKilled += IncreaseScore;
        Spawner.OnHeroSpawned += SetEndGameOnPlayerDeathEvent;

        var hudCanvas = GameObject.FindWithTag(HudCanvasTag);

        _ui = Instantiate(_uiPrefab, hudCanvas.transform);

        _game.SetActive(false);
        _ui.SetActive(false);
    }

    private void SetEndGameOnPlayerDeathEvent(Transform hero)
    {
        hero.GetComponentInChildren<AnimationEvents>().OnHeroDieAnimationEnded += EndGame;
    }

    private void IncreaseScore()
    {
        ++_playerScore;
    }

    void Start()
    {
        ChangePhase(GamePhase.MainMenu);
    }

    public void ChangePhase(GamePhase gamePhase)
    {
        switch (gamePhase)
        {
            case GamePhase.MainMenu:
                break;

            case GamePhase.Game:

                _game.SetActive(true);
                _ui.SetActive(true);

                StartCoroutine(StartGame());
                break;

            case GamePhase.End:
                break;
        }

        CurrentPhase = gamePhase;

        OnGamePhaseChange?.Invoke(gamePhase);
    }

    private IEnumerator StartGame()
    {
        yield return _waitForHalfASecond;

        _battleThemeAudioSource.Play();
        HasGameStarted = true;
    }

    public void PrintLog(string message)
    {
        if (!IsDebug) return;
        Debug.Log(message);
    }

    public int GetPlayerScore()
    {
        return _playerScore;
    }

    public void RestartGame()
    {
        InitGame();
        ChangePhase(GamePhase.MainMenu);
    }

    private void EndGame()
    {
        Destroy(_game);
        Destroy(_ui);

        HasGameStarted = false;
        _battleThemeAudioSource.Stop();

        ChangePhase(GamePhase.End);
    }
}