using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    enum Texts { ItemName, Stats, Description, End}
    private ItemObject _itemObject;
    public ItemObject ItemObject { get { return _itemObject; } set { _itemObject = value; TextUpdate();} }
    private TextMeshProUGUI[] _texts = new TextMeshProUGUI[(int)Texts.End];

    private void Awake()
    {
        for (int i = 0; i < _texts.Length; i++)
        {
            _texts[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnGUI()
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        gameObject.GetComponent<RectTransform>().position = screenPos;
    }

    private void TextUpdate()
    {
        _texts[(int)Texts.ItemName].text = _itemObject.data.name;
        string stats = "";
        foreach(ItemBuff buff in _itemObject.data.buffs)
        {
            stats += buff.stat.ToString() + " : " + buff.value.ToString("n2") +"\n";
        }
        _texts[(int)Texts.Stats].text =stats;
        _texts[(int)Texts.Description].text = _itemObject.description;

    }
}
