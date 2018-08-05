using UnityEngine;
using System.Collections;

public class VendorPanel : Inventory {
    #region 单例模式
    private static VendorPanel instance;
    public static VendorPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("VendorPanel").GetComponent<VendorPanel>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion
    public int[] itemId;//规定产生的物品ID
    
    private Player player;
    public override void Start()
    {
        base.Start();
        InitVendor();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    /// <summary>
    /// 初始化小贩中的物品
    /// </summary>
    public void InitVendor()
    {
        foreach (int  id in itemId)
        {
            StoreItem(id);
        }
    }
    public void PurchaseItem(Item item)
    {
        if (player.ConsumeCoin(item.buyPrice))
        {
            Knapsack.Instance.StoreItem(item);
        }
    }
     public void SellItem(ItemUI item)
    {
        int sellCoin = item.item.sellPrice * item.amount;
        player.EarnCoin(sellCoin);
        foreach(Slot temp in slotList)
        {
            if (temp.transform.childCount > 0)
            {//大于0 表示槽上有物品了
                if (temp.transform.GetChild(0).GetComponent<ItemUI>().item.id == item.item.id)
                {//查看槽上的物品是否与鼠标上的物品相同，若相同则让该物品闪烁一下 表示销售成功
                    temp.transform.GetChild(0).GetComponent<ItemUI>().SetAmount(1);//目的是让物品闪烁以下，并不修改他的数量
                    InventoryManager.Instance.IspickedItem = false;
                    InventoryManager.Instance.HidePicked();
                    return;
                }
                
            } else
            {
                
                //即此时遇见了空槽，说明在有物品的槽区域内没有发现跟这个物品一样物品
                 //此时执行插入操作，将鼠标上的物品放上去
                temp.StoreItem(item.item);
                InventoryManager.Instance.IspickedItem = false;
                InventoryManager.Instance.HidePicked();
                return;
            }

        }
    }
}
