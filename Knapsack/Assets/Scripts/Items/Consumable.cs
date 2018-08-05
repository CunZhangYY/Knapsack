using UnityEngine;
using System.Collections;
/// <summary>
/// 消耗品类
/// </summary>
public class Consumable : Item {
    
    public int hp { get; set; }
    public int mp { get; set; }
    public Consumable(int id, string name, ItemType itemType, Quality quality, string description, int capacity,
        int buyPrice, int sellPrice,string sprite,int hp,int mp) :base( id,  name, itemType,  quality,  description,  capacity,
         buyPrice,  sellPrice,sprite)
    {
        this.hp = hp;
        this.mp = mp;
    }
    public override string ToString()
    {
        string s = "";
        s += id.ToString();
        s += name;
        s += itemType;
        s += quality;
        s += description;
        s += capacity.ToString();
        s += buyPrice;
        s += sellPrice;
        s += sprite;
        s += hp;
        s += mp;
        return s;
    }
    public override string GetNameToolTip()
    {
        string baseText = base.GetNameToolTip();
        string consum = string.Format("{0}\n\n<color=green>血量：{1}\n技能蓝：{2}</color>", baseText, hp, mp);
        return consum;
    }
}
