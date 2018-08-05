using UnityEngine;
using System.Collections;

public class Knapsack : Inventory {
    #region 单例模式
    private static Knapsack _instance;
    public static Knapsack Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("KnapsackPanel").GetComponent<Knapsack>();//因为在这里没法直接用获得组建的方法，只能先获取游戏对象，再获取该脚本实例
            }
            return _instance;
        }
    }
    #endregion
      
    //因为在他的父类中已经设置了一个update方法，如果此处再设置，那么就会覆盖父类的函数，导致隐藏方法不会调用 
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        int s = Random.Range(1, 18);
    //        StoreItem(s);
    //    }
    //}

}
