using UnityEngine;

[CreateAssetMenu(fileName = "HealthBarData", menuName = "ScriptableObjects/HealthBarData", order = 1)]
public class HealthBarData : ScriptableObject
{
    public GameObject HealthBarIcon;
    public Color FillBarColor;
    public float LoseHealthAnimationSpeed;
    public float ShowDamageAnimationSpeed;
}