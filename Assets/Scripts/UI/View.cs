
using UnityEngine;

public class View<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 0.3f;
    [SerializeField] private float _fadeOutDuration = 0.3f;
    [SerializeField] private CanvasGroup group;

    protected GameManager GameManager;

    protected bool Visible;

    private float _startTime;
    private float _duration;
    private bool _inTransition;
    private bool _inOrOut;

    protected override void Awake()
    {
        // Cache
        GameManager = GameManager.Instance;

        // Init
        Visible = false;
        group.alpha = 0.0f;
        group.interactable = false;
        group.blocksRaycasts = false;

        GameManager.OnGamePhaseChange += OnGamePhaseChanged;
    }

    protected virtual void OnGamePhaseChanged(GamePhase gamePhase) { }

    public void Transition(bool inOrOut)
    {
        Visible = inOrOut;

        _startTime = Time.time;
        _inTransition = true;
        _inOrOut = inOrOut;
        _duration = inOrOut ? _fadeInDuration : _fadeOutDuration;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    protected virtual void Update()
    {
        if (!_inTransition) return;

        float time = Time.time - _startTime;
        float percent = time / _duration;

        if (percent < 1.0f)
        {
            group.alpha = _inOrOut ? percent : (1.0f - percent);
        }
        else
        {
            _inTransition = false;
            group.alpha = _inOrOut ? 1.0f : 0.0f;
            group.interactable = _inOrOut;
            group.blocksRaycasts = _inOrOut;
        }
    }
}