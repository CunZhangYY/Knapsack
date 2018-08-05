using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot {
    public Equipment.EquipType  equipType;//设置一个槽上可存储的物品类型
    public Weapon.WeaponType weaponType;//设置一个槽上可存储的武器类型
                                        // Use this for initialization

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {//如果点击了鼠标右键 则执行脱下操作
            if (transform.childCount > 0 && InventoryManager.Instance.IspickedItem == false)
            {//大于0 说明物品槽上有物品
                ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
                if (currentItem.item is Equipment || currentItem.item is Weapon)
                {
                    transform.parent.parent.SendMessage("PutOff", currentItem);//给父类发送一个信息
                    InventoryManager.Instance.HideToolTip();
                    Destroy(currentItem.gameObject);
                    CharacterPanel.Instance.UpgradeText();//更新交换的物品的显示信息
                }
            }
        }


        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (InventoryManager.Instance.IspickedItem == true)
        {//鼠标上有物品的情况
            ItemUI currentPickedItem = InventoryManager.Instance.PickedItem;
            if (transform.childCount > 0)
            {//槽上有物品时
                if(currentPickedItem.item is Equipment)
                {//判断鼠标上的物品是否是属于装备类型的
                    if (((Equipment)(currentPickedItem.item)).equipType == equipType)
                    {//鼠标上的物品若属于装备类型，那么将其强制类型转换为装备类型，并判断，其装备的物品类型是否属于跟该物品槽相同
                        if (currentPickedItem.amount == 1)
                        {//我要求他只能在鼠标上的物品只有一个时才允许被交换，若果多于一个那么就不允许交换
                            CharacterPanel.Instance.UpgradeText(currentPickedItem);//更新交换的物品的显示信息
                            currentPickedItem.ExchangeItem(transform.GetChild(0).GetComponent<ItemUI>());
                        }
                    }
                }
                else
                {
                    if(currentPickedItem.item is Weapon)
                    {//判断鼠标上的物品是否是武器类型
                        if (((Weapon)(currentPickedItem.item)).weaponType == weaponType)
                        {//鼠标上的物品若属于装备类型，那么将其强制类型转换为装备类型，并判断，其装备的物品类型是否属于跟该物品槽相同
                            if (currentPickedItem.amount == 1)
                            {
                                CharacterPanel.Instance.UpgradeText(currentPickedItem);//更新交换的物品的显示信息
                                currentPickedItem.ExchangeItem(transform.GetChild(0).GetComponent<ItemUI>());
                            }
                        }
                    }
                }
            }else
            {//选中槽上没有物品时，直接把鼠标上的物品放到槽上即可
                if (currentPickedItem.item is Equipment)
                {//判断鼠标上的物品是否是属于装备类型的
                    if (((Equipment)(currentPickedItem.item)).equipType == equipType)
                    {//鼠标上的物品若属于装备类型，那么将其强制类型转换为装备类型，并判断，其装备的物品类型是否属于跟该物品槽相同
                        this.StoreItem(currentPickedItem.item);
                        currentPickedItem.RemoveAmount();//减少一个物品
                        CharacterPanel.Instance.UpgradeText(currentPickedItem);//更新交换的物品的显示信息
                        if (currentPickedItem.amount <= 0)
                        {
                            InventoryManager.Instance.HidePicked();
                            InventoryManager.Instance.IspickedItem = false;
                        }
                    }
                }
                else
                {
                    if (currentPickedItem.item is Weapon)
                    {//判断鼠标上的物品是否是武器类型
                        if (((Weapon)(currentPickedItem.item)).weaponType == weaponType)
                        {//鼠标上的物品若属于装备类型，那么将其强制类型转换为装备类型，并判断，其装备的物品类型是否属于跟该物品槽相同
                            this.StoreItem(currentPickedItem.item);
                            currentPickedItem.RemoveAmount();//减少一个物品
                            CharacterPanel.Instance.UpgradeText(currentPickedItem);//更新交换的物品的显示信息
                            if (currentPickedItem.amount <= 0)
                            {
                                InventoryManager.Instance.HidePicked();
                                InventoryManager.Instance.IspickedItem = false;
                            }
                        }
                    }
                }
                
            }
        }else
        {
            if (transform.childCount > 0)
            {
                InventoryManager.Instance.SetPickedItem(transform.GetChild(0).GetComponent<ItemUI>().item, transform.GetChild(0).GetComponent<ItemUI>().amount);
                CharacterPanel.Instance.UpgradeText();//更新交换的物品的显示信息
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}
