using UnityEngine;
using System.Collections;
/// <summary>
/// 装备类
/// </summary>
public class Equipment : Item {
    /// <summary>
    /// 力量
    /// </summary>
    public int strength { get; set; }
    /// <summary>
    /// 智力
    /// </summary>
    public int intellect { get; set; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int agility { get; set; }
    /// <summary>
    /// 体力
    /// </summary>
    public int stamina { get; set; }
    /// <summary>
    ///装备类型 
    /// </summary>
    public EquipType equipType { get; set; }

    public enum EquipType
    {
        /// <summary>
        /// 空的类型，用来表示此处存储武器用的
        /// </summary>
        none,
        /// <summary>
        /// 头部
        /// </summary>
        head,
        /// <summary>
        /// 脖子
        /// </summary>
        neck,
        /// <summary>
        /// 胸部
        /// </summary>
        chest,
        /// <summary>
        /// 戒指
        /// </summary>
        ring,
        /// <summary>
        /// 腿
        /// </summary>
        leg,
        /// <summary>
        /// 护腕
        /// </summary>
        bracer,
        /// <summary>
        /// 靴子
        /// </summary>
        boots,
        /// <summary>
        /// 肩膀
        /// </summary>
        shoulder,
        /// <summary>
        /// 腰带
        /// </summary>
        belt,
        /// <summary>
        /// 副手
        /// </summary>
        offHand
    }
    public Equipment(int id, string name, ItemType itemType, Quality quality, string description, int capacity,
        int buyPrice, int sellPrice,string sprite ,int strength,int intellect,int agility,int stamina,EquipType equipType):base(id, name, itemType, quality, description, capacity,
         buyPrice, sellPrice,sprite)
    {
        this.strength = strength;
        this.intellect = intellect;
        this.agility = agility;
        this.stamina = stamina;
        this.equipType = equipType;
    }

    public override string GetNameToolTip()
    {
        string equipTp = "";
        switch (equipType)
        {
            case EquipType.head:
                equipTp = "头部";
                break;
            case EquipType.neck:
                equipTp = "脖子";
                break;
            case EquipType.chest:
                equipTp = "护胸";
                break;
            case EquipType.ring:
                equipTp = "戒指";
                break;
            case EquipType.leg:
                equipTp = "护膝";
                break;
            case EquipType.bracer:
                equipTp = "护腕";
                break;
            case EquipType.boots:
                equipTp = "靴子";
                break;
            case EquipType.shoulder:
                equipTp = "护肩";
                break;
            case EquipType.belt:
                equipTp = "腰带";
                break;
            case EquipType.offHand:
                equipTp = "副手";
                break;
            
        }

        string baseText = base.GetNameToolTip();
        string equip = string.Format("{0}\n\n<color=yellow>装备类型：{1}\n</color><color=green>力量：{2}\n智力：{3}\n敏捷：{4}\n体力：{5}\n</color>", baseText,equipTp, strength,intellect,agility,stamina);
        return equip;
    }
}
