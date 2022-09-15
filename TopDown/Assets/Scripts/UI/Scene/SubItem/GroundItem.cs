using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour, IInteractable
{
    public ItemObject ItemObject;

    public float distance = 3f;
    public float Distance => distance;

    private void Start()
    {
        gameObject.GetOrAddComponent<CameraFacing>();
    }

    private void OnValidate()
    {
        gameObject.GetOrAddComponent<SpriteRenderer>().sprite = ItemObject?.icon;

    }

    public bool Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if (calcDistance > distance)
            return false;

        return false;
      //  return other.GetComponent<PlayerController>().PickUpItem(this) ?? false;
    }

    public void StopInteract(GameObject other)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
