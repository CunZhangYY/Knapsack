using UnityEngine;
using System.Collections;
/// <summary>
/// 武器类
/// </summary>
public class Weapon : Item {
    /// <summary>
    /// 攻击力
    /// </summary>
    public int damage { get; set; }
    /// <summary>
    /// 武器类型
    /// </summary>
    public WeaponType weaponType { get; set; }
    public enum WeaponType
    {
        none,//空
        offHand,//副武器
        mainHand//主武器
    }
    public Weapon(int id, string name, ItemType itemType, Quality quality, string description, int capacity,
        int buyPrice, int sellPrice,string sprite ,int damage,WeaponType weaponType):base(id, name, itemType, quality, description, capacity,
         buyPrice, sellPrice,sprite )
    {
        this.damage = damage;
        this.weaponType = weaponType;
    }

    public override string GetNameToolTip()
    {
        string weap = "";
        switch (weaponType)
        {
            case WeaponType.offHand:
                weap = "一把利剑";
                break;
            case WeaponType.mainHand:
                weap = "开天斧";
                break;          
        }

        string baseText = base.GetNameToolTip();
        string wea = string.Format("{0}\n\n<color=yellow>{1}\n</color><color=green>攻击力：{2}</color>", baseText, weap,damage);
        return wea;
    }
}
