using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _iconSlotTransform;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private Transform _damageTextTransform;
    [SerializeField] private TextMeshProUGUI _bumpDamageText;
    [SerializeField] private Transform _bumpDamageTextTransform;
    [SerializeField] private TextMeshProUGUI _healthIndicator;
    [SerializeField] private HealthBarData _healthBarData;

    private Slider _fillSlider;

    private const int DestroyAfterFrames = 60;
    private const float DamageNumberOffset = 0.1f;

    private int _entityInitialHealth;
    private int _entityCurrentHealth;

    void Awake()
    {
        _fillSlider = GetComponentInChildren<Slider>();

        var entity = GetComponentInParent<Entity>();
        entity.OnEntityParamsSet += SetEntityInitialHealth;
        entity.OnHealthUpdated += SetEntityCurrentHealth;
        entity.OnDeath += DestroyBarAfterSomeFrames;
        transform.parent.GetComponentInChildren<HitCollisions>().OnHitTriggerEvent += ShowWeaponDamage;

        // last minute addition to add bumpDamageText on health bar, separated from normal damage
        // as they can happen at the same time
        var bumpCollisions = transform.parent.GetComponentInChildren<BumpCollisions>();
        if (!bumpCollisions) return;

        bumpCollisions.OnBumpCollisionEvent += ShowBumpDamage;
    }

    void Start()
    {
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        Instantiate(_healthBarData.HealthBarIcon, _iconSlotTransform);
        
        _healthBarFill.color = _healthBarData.FillBarColor;

        SetTextColorAndVisibility(_damageText, _damageTextTransform);

        if (_bumpDamageText && _bumpDamageTextTransform)
        {
            SetTextColorAndVisibility(_bumpDamageText, _bumpDamageTextTransform);
        }

        _healthIndicator.text = _entityInitialHealth + "/" + _entityInitialHealth;
    }

    private void SetTextColorAndVisibility(TextMeshProUGUI text, Transform textTransform)
    {
        text.color = _healthBarData.FillBarColor;
        textTransform.gameObject.SetActive(false);
    }

    private void SetEntityInitialHealth(Entity.EntityParams entityParams)
    {
        _entityInitialHealth = entityParams.Health;
    }
    private void SetEntityCurrentHealth(int health)
    {
        _entityCurrentHealth = health;
    }

    private void DestroyBarAfterSomeFrames()
    {
        StartCoroutine(WaitFramesAndDestroyObject(DestroyAfterFrames));
    }

    private IEnumerator WaitFramesAndDestroyObject(int frames)
    {
        int count = 0;

        while (count < frames)
        {
            ++count;

            yield return 0;
        }

        Destroy(gameObject);
    }
    private void ShowWeaponDamage(int damage)
    {
        StartCoroutine(LoseHealthAnimation(damage));
        StartCoroutine(ShowDamageNumber(_damageText, _damageTextTransform, damage));
    }

    private void ShowBumpDamage(int damage)
    {
        StartCoroutine(LoseHealthAnimation(damage));
        StartCoroutine(ShowDamageNumber(_bumpDamageText, _bumpDamageTextTransform, damage));
    }

    private IEnumerator LoseHealthAnimation(float damage)
    {
        _healthIndicator.text = _entityCurrentHealth + "/" + _entityInitialHealth;

        var damagePercent = damage / _entityInitialHealth;
        var targetValue = _fillSlider.value - damagePercent;

        while (_fillSlider.value > targetValue)
        {
            _fillSlider.value -= damagePercent * _healthBarData.LoseHealthAnimationSpeed * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator ShowDamageNumber(TextMeshProUGUI damageText, Transform damageTextTransform,  int damage)
    {
        damageText.text = damage.ToString();
        damageText.gameObject.SetActive(true);

        Vector3 offset = Vector3.up * DamageNumberOffset;

        var startPosition = damageTextTransform.localPosition;
        var endPosition = startPosition + offset;

        while (damageTextTransform.localPosition.y < endPosition.y)
        {
            damageTextTransform.localPosition += offset * Time.deltaTime * _healthBarData.ShowDamageAnimationSpeed;

            yield return null;
        }

        damageTextTransform.gameObject.SetActive(false);
        damageTextTransform.localPosition -= offset;
    }
}