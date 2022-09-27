using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour, IInteractable
{
    public float distance = 2f;
    public float Distance => distance;

    private GameObject _itemBox;
    private GameObject _itemBoxInven;
    public ItemBoxInventoryUI itemBoxInvenUI;

    public bool Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if (calcDistance - 0.2f > distance) //navmesh Radius값까지 고려
            return false;

        if(other.gameObject.GetComponent<PlayerController>()?.ConnectBox(this) ?? false)
        {
            _itemBox.SetActive(true); //일정거리 접근 시 활성화
            Vector3 screenPos = Camera.main.WorldToViewportPoint(gameObject.transform.position + Vector3.up * 2);
            screenPos.x *= _itemBox.GetComponent<RectTransform>().rect.width;
            screenPos.y *= _itemBox.GetComponent<RectTransform>().rect.height;
            _itemBoxInven.GetComponent<RectTransform>().position = screenPos;


            return true;
        }

        return false;
    }

    public void StopInteract(GameObject other)
    {
        itemBoxInvenUI.DisConnectInven();
        _itemBox.SetActive(false); //멀어질 시 비활성화
    }

    void Start()
    {
        if(gameObject.GetComponent<MeshRenderer>() != null)
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.layer = (int)Define.Layer.ItemBox;
        _itemBox = Managers.Resource.Instantiate("UI/Inventory/ItemBox");
        _itemBoxInven = _itemBox.transform.GetChild(0).gameObject;
        itemBoxInvenUI = _itemBoxInven.GetComponent<ItemBoxInventoryUI>();
        _itemBox.SetActive(false); //시작 시 해당 아이템박스 인벤토리 비활성화.
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
