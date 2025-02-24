using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Hp,
        Power,
        Defense,
        Crit
    }
    public ItemType type;
    public string item_name;
    public string description;
    public Sprite icon;

    public Image showIcon;
    public Text showName;
    public Text showDes;

    private void Start()
    {
        showName.text = item_name;
        showDes.text = description;
        showIcon.sprite= icon;
    }

    public void SelectCard()
    {
        GameObject temp=GameObject.Find("Player");
        switch (type)
        {
            case ItemType.Hp:
                temp.GetComponent<Player>().HpCard++;
                break;
            case ItemType.Power:
                temp.GetComponent<Player>().PowerCard++;
                break;
            case ItemType.Defense:
                temp.GetComponent<Player>().DefanseCard++;
                break;
            case ItemType.Crit:
                temp.GetComponent<Player>().CritCard++;
                break;
        }
        temp.GetComponent<Player>().Close_LevelUp_UI();
    }
}
