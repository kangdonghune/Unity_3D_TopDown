using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    enum Texts { ItemName, Stats, Description, End}
    private ItemObject _itemObject;
    public ItemObject ItemObject { get { return _itemObject; } set { _itemObject = value; TextUpdate();} }
    public GameObject[] texts;

    private void LateUpdate()
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        gameObject.GetComponent<RectTransform>().position = screenPos;
    }

    private void TextUpdate()
    {
        texts[(int)Texts.ItemName].GetComponent<TextMeshProUGUI>().text = _itemObject.data.name;
        string stats = "";
        foreach(ItemBuff buff in _itemObject.data.buffs)
        {
            stats += buff.stat.ToString() + " : " + buff.value.ToString("n2") +"\n";
        }
        texts[(int)Texts.Stats].GetComponent<TextMeshProUGUI>().text =stats;
        texts[(int)Texts.Description].GetComponent<TextMeshProUGUI>().text = _itemObject.description;

    }
}
