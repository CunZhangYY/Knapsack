using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 物品基类
/// </summary>
public class Item  {

    public enum ItemType
    {
        Consumble,
        Equipment,
        Weapon,
        Material
    }
    public enum Quality
    {
        Common,
        Uncommon,
        Rare,
        Eqic,
        Legendary,
        Artifact
    }
    /// <summary>
    /// 物品的ID编号
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 物品的名字
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 物品的种类
    /// </summary>
    public ItemType itemType { get; set; }
    /// <summary>
    /// 物品的品质
    /// </summary>
    public Quality quality { get; set; }
    /// <summary>
    /// 物品的描述信息
    /// </summary>
    public string description { get; set; }
    /// <summary>
    /// 每个物品槽里限定可以放几个这类物品
    /// </summary>
    public int capacity { get; set; }
    /// <summary>
    /// 购买价格
    /// </summary>
    public int buyPrice { get; set; }
    /// <summary>
    /// 销售价格
    /// </summary>
    public int sellPrice { get; set; }
    /// <summary>
    /// 每个物品都需要有一个ui图标，sprite就是存放每个ui图标的存储路径
    /// </summary>
    public string sprite { get; set; }


    public Item(int id,string name ,ItemType itemType,Quality quality,string description,int capacity,
        int buyPrice,int sellPrice,string sprite)
    {
        this.id = id;
        this.name = name;
        this.itemType = itemType;
        this.quality = quality;
        this.description = description;
        this.capacity = capacity;
        this.buyPrice = buyPrice;
        this.sellPrice = sellPrice;
        this.sprite = sprite;
    }

    public virtual string GetNameToolTip()
    {
        string qualityText = "";
        switch (quality)
        {
            case Quality.Common:
                qualityText="white";
                break;
            case Quality.Uncommon:
                qualityText = "lime";
                break;
            case Quality.Rare:
                qualityText = "navy";
                break;
            case Quality.Eqic:
                qualityText = "magenta";
                break;
            case Quality.Legendary:
                qualityText = "orange";
                break;
            case Quality.Artifact:
                qualityText = "red";
                break;
            
        }

        string text = string.Format("<color={4}>名字：{0}</color>\n<color=red>购买价格：{1}\n销售价格：{2}</color>\n<color=blue>功能描述：{3}</color>", name, buyPrice, sellPrice, description,qualityText);
        return text;
    }
   
}
