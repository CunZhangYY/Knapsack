using UnityEngine;
using System.Collections;

public class Chest : Inventory {

    #region 单例模式
    private static Chest _instance;
    public static Chest Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("ChestPanel").GetComponent<Chest>();//因为在这里没法直接用获得组建的方法，只能先获取游戏对象，再获取该脚本实例
            }
            return _instance;
        }
    }
    #endregion
}
