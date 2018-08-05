using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Formula {
    /// <summary>
    /// 锻造材料1的ID   
    /// </summary>
	public int Item1ID { get; set; }
    /// <summary>
    /// 锻造材料1的数量
    /// </summary>
    public int Item1Amount { get; set; }
    /// <summary>
    /// 锻造材料2的ID
    /// </summary>
    public int Item2ID { get; set; }
    /// <summary>
    /// 锻造材料2的数量
    /// </summary>  
    public int Item2Amount { get; set; }
    /// <summary>
    /// 锻造所得的物品
    /// </summary>
    public int GetItemID { get; set; }
    /// <summary>
    /// 配方也做成两个属性总和到一起的方式
    /// </summary>
    private List<int> formulaList = new List<int>();

    public Formula(int Item1ID,int Item1Amount,int Item2ID,int Item2Amount,int GetItemID)
    {
        this.Item1ID = Item1ID;
        this.Item1Amount = Item1Amount;
        this.Item2ID = Item2ID;
        this.Item2Amount = Item2Amount;
        this.GetItemID = GetItemID;
        for(int i = 0; i < this.Item1Amount; i++)
        {
            formulaList.Add(this.Item1ID);
        }
        for(int i = 0; i < this.Item2Amount; i++)
        {
            formulaList.Add(this.Item2ID);
        }
    }

    public override string ToString()
    {
        string s = "";
        s += this.Item1ID;
        s += this.Item1Amount;
        s += this.Item2ID;
        s += this.Item2Amount;
        s += this.GetItemID;
        return s;
    }
    /// <summary>
    /// 配方与材料进行匹配
    /// </summary>
    /// <param name="itemMaterialID"></param>
    /// <returns></returns>
    public bool Match(List<int> itemMaterialID)
    {
        string s = "";
        Debug.Log("输出槽上的物品信息");
        foreach (int item in itemMaterialID)
        {
            s += item.ToString();
        }
        Debug.Log(s);
        string e = "";
        Debug.Log("输出配方的物品信息");
        foreach (int item in formulaList)
        {
            e += item.ToString();
        }
        Debug.Log(e);
        //List<int> temp = itemMaterialID;//不能这样临时保存一个引用类型的变量，因为这样相当于是又创建了一个引用而已，并不是一个变量副本
        List<int> temp = new List<int>();
        temp = itemMaterialID;//这样才算是创建了一个itemMaterialID的一个副本
        foreach (int item in formulaList)
        {
            if (temp.Remove(item) == false)
            {//如果移除操作失败了，说明itemMaterialID中有不匹配的项 ，即说明匹配失败
                return false;
            }
        }
        return true;
    }
    public List<int > GetFormulaList()
    {
        return this.formulaList;
    }
}
