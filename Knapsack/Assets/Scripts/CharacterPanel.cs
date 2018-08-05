using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPanel : Inventory {
    #region 单例模式
    private static CharacterPanel instance;
    public static CharacterPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("CharacterPanel").GetComponent<CharacterPanel>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    private Text propertyText;

    public override void Start()
    {
        slotList = GetComponentsInChildren<EquipmentSlot>();//获取所有物品槽信息
        canvasGroup = GetComponent<CanvasGroup>();
        propertyText = transform.FindChild("TextPanel/PropertyText").GetComponent<Text>();
        UpgradeText();
        DisplaySwitch();
    }

    #region 穿上或卸下装备
    public void PutOn(ItemUI item)
    {
        foreach (EquipmentSlot temp in slotList)
        {
            if (item.item is Equipment)
            {//当点击的物品是装备时，
             //目的槽上已经有物品了，此时执行交换
             //如果槽上没有物品时，此时直接将选中物品放到槽上
                if (temp.equipType == ((Equipment)(item.item)).equipType)
                {//判断物品的类型与槽的类型相同
                    if (temp.transform.childCount > 0)
                    {//此时执行交换操作，
                        if (item.amount == 1)
                        {
                            if(temp.transform.GetChild(0).GetComponent<ItemUI>().item is Weapon)
                            {//判断槽上的物品是属于武器还是装备 ，是武器才可以交换，是装备不可以交换
                                UpgradeText(item);//更新交换的物品的显示信息
                                item.ExchangeItem(temp.transform.GetChild(0).GetComponent<ItemUI>());
                            }
                        }
                        else
                        {
                            //此时就说明背包槽上的物品多于一个，此时要判断当角色面板槽上的物品是属于什么类型
                            //属于装备类型，就不进行交换操作
                            //属于武器类型，就进行交换操作，交换时只需要先将背包上的物品数量减一，再将从角色面板传过来的武器放到背包上
                            if (temp.transform.GetChild(0).GetComponent<ItemUI>().item is Weapon)
                            {
                                UpgradeText(item);//更新交换的物品的显示信息
                                item.RemoveAmount();//将背包上的数量减1
                                Knapsack.Instance.StoreItem(temp.transform.GetChild(0).GetComponent<ItemUI>().item);//调用背包上的创建新物品的方法，将角色面板上的物品放上去 
                                temp.transform.GetChild(0).GetComponent<ItemUI>().SetItem(item.item, 1);//将背包上的选中的物品放到角色面板上
                            }
                        }

                    }
                    else
                    {//说明角色面板的槽上空的
                     //此时直接将物品放到槽上
                        UpgradeText(item);//更新交换的物品的显示信息
                        temp.StoreItem(item.item);//直接调用该槽上的创建物品的方法
                        item.RemoveAmount();//减少自身的数量
                        if (item.amount <= 0)
                        {
                            Destroy(item.gameObject);
                            InventoryManager.Instance.HideToolTip();
                        }
                    }
                }


            }
            else
            {//当点击的物品是武器时，
                //目的槽上已经有物品了，此时执行交换
                //如果槽上没有物品时，此时直接将选中物品放到槽上
                if (temp.weaponType == ((Weapon)(item.item)).weaponType)
                {//判断物品的类型与槽的类型相同
                    if (temp.transform.childCount > 0)
                    {//此时执行交换操作，
                        if (item.amount == 1)
                        {
                            if (temp.transform.GetChild(0).GetComponent<ItemUI>().item is Equipment)
                            {//判断槽上的物品是属于武器还是装备 ，是武器才可以交换，是装备不可以交换
                                UpgradeText(item);//更新交换的物品的显示信息
                                item.ExchangeItem(temp.transform.GetChild(0).GetComponent<ItemUI>());
                            }

                        }
                        else
                        {
                            //此时就说明背包槽上的物品多于一个，此时要判断当角色面板槽上的物品是属于什么类型
                            //属于装备类型，就不进行交换操作
                            //属于武器类型，就进行交换操作，交换时只需要先将背包上的物品数量减一，再将从角色面板传过来的武器放到背包上
                            if (temp.transform.GetChild(0).GetComponent<ItemUI>().item is Equipment)
                            {
                                UpgradeText(item);//更新交换的物品的显示信息
                                item.RemoveAmount();//将背包上的数量减1
                                Knapsack.Instance.StoreItem(temp.transform.GetChild(0).GetComponent<ItemUI>().item);//调用背包上的创建新物品的方法，将角色面板上的物品放上去 
                                temp.transform.GetChild(0).GetComponent<ItemUI>().SetItem(item.item, 1);//将背包上的选中的物品放到角色面板上
                            }
                        }

                    }
                    else
                    {//说明角色面板的槽上空的
                     //此时直接将物品放到槽上
                        UpgradeText(item);//更新交换的物品的显示信息
                        temp.StoreItem(item.item);//直接调用该槽上的创建物品的方法
                        item.RemoveAmount();//减少自身的数量
                        if (item.amount <= 0)
                        {
                            Destroy(item.gameObject);
                            InventoryManager.Instance.HideToolTip();
                        }
                    }
                }
            }
        }
    }
    public void PutOff(ItemUI item)
    {
        Knapsack.Instance.StoreItem(item.item);
        UpgradeText();//更新交换的物品的显示信息
    }
    #endregion

    public void UpgradeText(ItemUI item = null)
    {
        int strength = 0, intellect = 0, agility = 0, stamina = 0, damage = 0;
        if (item != null)
        {
            if (item.item is Equipment)
            {
                Equipment e = ((Equipment)(item.item));

                strength = e.strength;
                intellect = e.intellect;
                agility = e.agility;
                stamina = e.stamina;

            }
            if (item.item is Weapon)
            {
                Weapon w = ((Weapon)(item.item));
                damage = w.damage;
            }
        }
        
        string s = string.Format("<color=red>力量：{0}\n智力：{1}\n敏捷：{2}\n体力：{3}\n攻击力：{4}\n</color>", strength, intellect, agility, stamina, damage);
        propertyText.text = s;
    }


}
