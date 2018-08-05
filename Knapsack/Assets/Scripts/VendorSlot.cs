using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VendorSlot : Slot {

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right&&InventoryManager.Instance.IspickedItem==false)
        {//如果点击了鼠标右键，并且鼠标上没有信息 此时才可以执行购买操作
            if (transform.childCount > 0)
            {//假设槽不为空
                ItemUI item = transform.GetChild(0).GetComponent<ItemUI>();//获取槽上的物品
                transform.parent.parent.SendMessage("PurchaseItem", item.item);
                item.SetAmount(1);
            }
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Left && InventoryManager.Instance.IspickedItem == true)
            {//如果点击了鼠标左键，并且鼠标上有物品，不管点击的是哪个槽都执行销售操作，不管是不是空槽
                
                    ItemUI item = InventoryManager.Instance.PickedItem;//获取鼠标上待销售的物品                 
                    transform.parent.parent.SendMessage("SellItem", item);
                
            }
        }
        
    }
}
