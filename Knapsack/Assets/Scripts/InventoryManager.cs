using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    #region 单例模式
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            }
            return _instance;
        }
    }
    #endregion
    private ToolTip toolTip;//创建工具条游戏对象的引用
    private bool isToolTip = false;
    private Canvas canvas;
    private Vector2 toolTipOffset = new Vector2(10, -25);

    #region 场景中 跟随鼠标移动的被选中物品的引用信息

    private ItemUI pickedItem;//创建场景中跟随鼠标移动的物品实例引用
    public ItemUI PickedItem
    {
        get
        {
            return pickedItem;
        }
        set
        {
            pickedItem = value;
        }
    }
    private bool isPickedItem = false;//标志跟随鼠标移动的物品上是否已有物品
    public bool IspickedItem
    {
        get
        {
            return isPickedItem;
        }set
        {
            isPickedItem = value;
        }
    }
    /// <summary>
    /// 拾取物品时保存物品锁在槽的信息
    /// </summary>
    private Transform currentSlot = null;
    public Transform CurrentSlot
    {
        get
        {
            return currentSlot;
        }
        set
        {
            currentSlot = value;
        }
    }
    /// <summary>
    /// 拾取物品之后的物品槽是否为空的标志位
    /// </summary>
    private bool isItemSlot = false;//默认不为空
    public bool IsItemSlot
    {
        get
        {
            return isItemSlot;
        }
        set
        {
            isItemSlot = value;
        }
    }
    #endregion

    //解析JSON文件后 所得的对象 存放所有的物品信息对象列表，其中的每个对象都代表着一个物品的信息
    private  List<Item> itemList;//这是物品的信息
    private List<Formula> forgeList;//这是锻造系统秘方的信息

    void Awake()
    {
        ParseItemJson();
        ParseForgeJSon();
    }
    void Start()
    {
        toolTip = GameObject.FindObjectOfType<ToolTip>();//通过类型获取工具条游戏对象，因为工具条没有被销毁与重建所以可以在start（）方法中创建
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        PickedItem = GameObject.Find("PickedItemImage").GetComponent<ItemUI>();//获取游戏场景中跟随鼠标移动的物品实例对象
        HidePicked();
        foreach(Formula item in forgeList)
        {
            print(item);
        }
    }

    #region 解析锻造系统的配方信息
    public void ParseForgeJSon()
    {
        forgeList = new List<Formula>();
        TextAsset formulaText = Resources.Load<TextAsset>("Forluma");
        string formula = formulaText.text;
        JSONObject f = new JSONObject(formula);
        foreach ( JSONObject temp in f.list)
        {
            Formula item = null;
            int Item1ID = (int)temp["Item1ID"].n;
            int Item1Amount = (int)temp["Item1Amount"].n;
            int Item2ID = (int)temp["Item2ID"].n;
            int Item2Amount = (int)temp["Item2Amount"].n;
            int GetItemID = (int)temp["GetItemID"].n;
            item = new Formula(Item1ID, Item1Amount, Item2ID, Item2Amount, GetItemID);
            forgeList.Add(item);
        }
    }
    #endregion


    #region 解析物品的JSON信息

    void ParseItemJson()
    {
        itemList = new List<Item>();
        TextAsset itemText = Resources.Load<TextAsset>("Items");
        string itemJson = itemText.text;//物品信息的JSon格式

        JSONObject j = new JSONObject(itemJson);
        foreach (JSONObject temp in j.list)
        {
            //遍历这个列表目的是要解析jSon文件中对应的属性
            //这个JSon文件保存的是消耗品类型的属性，也就是说，这个对象就是Consumable类的实例对象中的属性
            //对消耗品类中的私有类型进行解析
            //首先解析item类型
            string typeStr = temp["type"].str;
            Item.ItemType itemType = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), typeStr);


            //接下来进行解析基类中共有属性
            int id = (int)temp["id"].n;
            string name = temp["name"].str;
            Item.Quality quality = (Item.Quality)System.Enum.Parse(typeof(Item.Quality), temp["quality"].str);
            string description = temp["description"].str;
            int capacity = (int)temp["capacity"].n;
            int buyPrice = (int)temp["buyPrice"].n;
            int sellPrice = (int)temp["sellPrice"].n;
            string sprite = temp["sprite"].str;
            //以上就将Json文件中的所有属性都解析完了，接下来就该用解析所得的属性，创建消费者对象了
            Item consum = null;//这样也算是初始化了一个consum对象
            switch (itemType)
            {
                case Item.ItemType.Consumble:
                    int hp = (int)temp["hp"].n;
                    int mp = (int)temp["mp"].n;
                    //放在这里进行初始化对象 原因是hp 和mp是在选择语句这里进行赋值的所以，若不在这里初始化对象，那么就会报mp，hp值不确定的错误
                    //一定要注意
                    consum = new Consumable(id, name, itemType, quality, description, capacity, buyPrice, sellPrice, sprite, hp, mp);
                    break;
                case Item.ItemType.Equipment:
                    int strength = (int)temp["strength"].n;
                    int intellect = (int)temp["intellect"].n;
                    int agility = (int)temp["agility"].n;
                    int stamina = (int)temp["stamina"].n;
                    Equipment.EquipType equipType = (Equipment.EquipType)System.Enum.Parse(typeof(Equipment.EquipType), temp["equipType"].str);
                    consum = new Equipment(id, name, itemType, quality, description, capacity, buyPrice, sellPrice, sprite, strength, intellect, agility, stamina, equipType);
                    break;
                case Item.ItemType.Weapon:
                    int damage = (int)temp["damage"].n;
                    Weapon.WeaponType weaponType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), temp["weaponType"].str);
                    consum = new Weapon(id, name, itemType, quality, description, capacity, buyPrice, sellPrice, sprite, damage,weaponType);

                    break;
                case Item.ItemType.Material:
                    consum = new Material(id, name, itemType, quality, description, capacity, buyPrice, sellPrice, sprite);
                    break;

            }
            itemList.Add(consum);
            //print(consum);


        }
    }
    #endregion


    #region 通过索引获得对象  并管理Inventory

    public Item GetItemByID(int id)
    {
        foreach (Item temp in itemList)
        {
            if (temp.id == id)
            {
                return temp;
            }
        }
        return null;
    }
    #endregion


    #region 管理ToolTip工具条类
    public void ShowToolTip(string text)
    {//显示工具条提示
        if (IspickedItem == true)
        {
            return;
        }
        toolTip.Show(text);
        isToolTip = true;
    }

    public void HideToolTip()
    {//隐藏工具条提示
        toolTip.Hide();
        isToolTip = false;
    }
    #endregion

    /// <summary>
    /// 拾取物品的操作
    /// </summary>
    /// <param name="currentItem"></param>
    /// <param name="amountPicked"></param>
    public void SetPickedItem(Item currentItem, int amountPicked, Transform currentSlot = null)
    {//设置鼠标跟随物品的属性设置,即拾取物品的操作，此时也需要将工具条提示给隐藏掉
        //拾取物品时保存物品的位置信息，即保存物品所在槽的信息
        this.CurrentSlot = currentSlot;        
        pickedItem.SetItem(currentItem, amountPicked);
        ShowPicked();//鼠标上抓取物品之后需要将这个物品显示出来
        toolTip.Hide();
        IspickedItem = true;
        //鼠标拾取物品后上面代码已经进行显示了，但是执行拾取物品跟随的操作却在update方法中，
        //只能到下一帧执行，所以实际操作时会闪一下，解决方法是 将跟随代码拷贝到这里一份
        Vector2 position;//将鼠标的位置转换到在canvas上的位置表示
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
        pickedItem.transform.localPosition = position;//将鼠标位置给跟随的鼠标移动的物体
    }
    /// <summary>
    /// 设置拾取物品后物品槽上是否有物品的标志位
    /// </summary>
    public void SetCurrenSlot(bool slot)
    {
        this.isItemSlot = slot;
    }
    /// <summary>
    /// 设置物品的父对象
    /// </summary>
    /// <param name="item"></param>
    public void SetItemParent(ItemUI item)
    {
        item.transform.SetParent(this.CurrentSlot);
        item.transform.position = this.currentSlot.position;
        item.transform.localScale = new Vector3(1, 1, 1);
        item.AddAmount(0);//只为了添加一个动画
    }
    /// <summary>
    /// 控制鼠标上的物品放到物品槽上的数量
    /// </summary>
    public void RemoveItem(int amount=1)
    {
        pickedItem.RemoveAmount(amount);//默认减少一个
        if (pickedItem.amount <= 0)
        {//此时就相当于是将所有物品都放到了槽上，此时鼠标上的物品就要隐藏掉，同时把标志位置为false
            HidePicked();
            IspickedItem = false;//表示现在鼠标上没有物品
        }
    }
    /// <summary>
    /// 获取鼠标上物品的数量
    /// </summary>
    /// <returns></returns>
    public int GetPickedAmount()
    {
        return this.pickedItem.amount;
    }
    /// <summary>
    /// 显示跟随鼠标的物品
    /// </summary>
    public void ShowPicked()
    {
        pickedItem.gameObject.SetActive(true);
    }
    /// <summary>
    /// 隐藏跟随鼠标的物品
    /// </summary>
    public void HidePicked()
    {
        pickedItem.gameObject.SetActive(false);
    }
    /// <summary>
    /// 获取锻造系统配方的列表
    /// </summary>
    /// <returns></returns>
    public List<Formula> GetForgeList()
    {
        return this.forgeList;
    }
    void Update()
    {
        if (IspickedItem == true)
        {
            Vector2 position;//将鼠标的位置转换到在canvas上的位置表示
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            pickedItem.transform.localPosition = position;//将鼠标位置给跟随的鼠标移动的物体
        }
        if (isToolTip == true)
        {
            Vector2 position;//将鼠标的位置转换到在canvas上的位置表示
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetLocalPos(position+toolTipOffset);
        }
        //实现鼠标上的物品的丢弃
        if (IspickedItem == true && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)==false)
        {
            HidePicked();
            isPickedItem = false;
        }
    }
    /// <summary>
    /// 保存此次所有面板的信息
    /// </summary>
    public void SaveItemButton()
    {
        Knapsack.Instance.SaveItemPanel();
        Chest.Instance.SaveItemPanel();
        CharacterPanel.Instance.SaveItemPanel();
        ForgePanel.Instance.SaveItemPanel();
        PlayerPrefs.SetInt("CoinAmount", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().InitCoin);//将金币的数量保存到缓冲区中
    }
    /// <summary>
    /// 加载上次保存的所有面板信息
    /// </summary>
    public void LoadItemButton()
    {
        Knapsack.Instance.LoadItemPanel();
        Chest.Instance.LoadItemPanel();
        CharacterPanel.Instance.LoadItemPanel();
        ForgePanel.Instance.LoadItemPanel();
        if (PlayerPrefs.HasKey("CoinAmount")==true)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().InitCoin = PlayerPrefs.GetInt("CoinAmount");
        }
    }
}
