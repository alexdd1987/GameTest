using UnityEngine;

// This script only exists to move the colliders away and trigger the OnTriggerExit when the entity dies
public class MoveColliders : MonoBehaviour
{
    private const float  Units = 1000f;

    void Awake()
    {
        GetComponentInParent<Entity>().OnDeath += MoveColliderDownImmediately;
    }

    private void MoveColliderDownImmediately()
    {
        gameObject.transform.position = Vector3.down * Units;
    }
}
