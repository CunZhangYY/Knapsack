using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private  int initCoin = 400;
    public int InitCoin
    {
        get
        {
            return initCoin;

        }
        set
        {
            initCoin = value;
            coinText.text = initCoin.ToString();

        }
    }
    private Text coinText;
	// Use this for initialization
	void Start () {
        coinText = GameObject.FindGameObjectWithTag("CoinText").GetComponent<Text>();
        coinText.text = initCoin.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        //控制背包的显示
        if (Input.GetKeyDown(KeyCode.T))
        {
            Knapsack.Instance.DisplaySwitch();
        }
        //控制箱子的显示
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Chest.Instance.DisplaySwitch();
            ForgePanel.Instance.DisplaySwitch();
        }
        //控制锻造面板的显示
        if (Input.GetKeyDown(KeyCode.O))
        {
            ForgePanel.Instance.DisplaySwitch();
            Chest.Instance.DisplaySwitch();
        }
        //控制角色面板的显示
        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterPanel.Instance.DisplaySwitch();
            VendorPanel.Instance.DisplaySwitch();
        }
        //控制小贩面板的显示
        if (Input.GetKeyDown(KeyCode.I))
        {
            VendorPanel.Instance.DisplaySwitch();
            CharacterPanel.Instance.DisplaySwitch();
        }
        
        //控制物品生成
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int s = Random.Range(1, 18);
            Knapsack.Instance.StoreItem(s);
        }

    }
    /// <summary>
    /// 消耗金币,若返回true，则说明消费成功，若返回false则说明余额不足未消费成功
    /// </summary>
    /// <param name="amount"></param>
    public bool ConsumeCoin(int amount = 0)
    {
        initCoin -= amount;
        if (initCoin <= 0)
        {
            initCoin += amount;
            return false;
        }
        coinText.text = initCoin.ToString();
        return true;
    }
    /// <summary>
    /// 赚取金币
    /// </summary>
    /// <param name="amount"></param>
    public void EarnCoin(int amount = 0)
    {
        initCoin += amount;
        coinText.text = initCoin.ToString();

    }
}

