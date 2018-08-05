using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ForgePanel : Inventory {
    #region 单例模式
    private static ForgePanel instance;
    public static ForgePanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("ForgePanel").GetComponent<ForgePanel>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion
    public override void Start()
    {
        base.Start();
        DisplaySwitch();
    }
    /// <summary>
    /// 锻造系统,由按钮点击事件进行调用
    /// </summary>
    public void ForgeItem()
    {
        List<int> itemMaterialID = new List<int>();
          
        foreach(Slot temp in slotList)
        {//将锻造面板上的两个物品的属性信息按照预先设计好的方法  按照物品数量添加id编号 两个物品都添加到 列表上
            //当槽上的物品数量大于1时  才有必要去整合材料信息，若数量小于1 则锻造肯定是失败的，直接打印错误信息并return即可
            if (temp.transform.childCount > 0)
            {
                ItemUI current = temp.transform.GetChild(0).GetComponent<ItemUI>();
                for (int i = 0; i < current.amount; i++)
                {
                    itemMaterialID.Add(current.item.id);//将物品槽上的物品的id添加到列表上的
                }
            }else
            {
                Debug.Log("拥有的材料尚不能构成锻造条件");
                return;
            }

        }
        //此时itemMaterialID上已经总和了物品属性 此时就需要进行与配方进行匹配了
        foreach (Formula item in InventoryManager.Instance.GetForgeList())
        {
            if (item.Match(itemMaterialID))
            {//如果返回true，则说明匹配成功了，开始进行锻造，若不经这一步，走到了遍历的外面 那么说明匹配失败，打印一个失败信息
                //锻造的过程 是将锻造所得物品放回背包 
                //按照配方上的锻造所得物品进行创建
                Knapsack.Instance.StoreItem(item.GetItemID);
                //根据配方的id材料信息 销毁材料槽上的物品信息
                foreach (int id in item.GetFormulaList())
                {
                    foreach (Slot temp in slotList)
                    {
                        ItemUI t = temp.transform.GetChild(0).GetComponent<ItemUI>();
                        if (t.item.id == id)
                        {//如果检测到当前物品 就将数量减一
                            t.RemoveAmount();//是数量减1
                        }
                        if (t.amount <= 0)
                        {//如果数量小于0 那么就销毁当前物品
                            Destroy(temp.transform.GetChild(0).gameObject);
                        }
                    }
                }
                
                return;
            }
        }
        Debug.Log("拥有的材料尚不能构成锻造条件");
    }
}
