using UnityEngine;
using System.Collections;
using System.Text;

public class Inventory : MonoBehaviour {
    protected  Slot[] slotList;//创建保存所有物品槽信息的数组列表

    protected CanvasGroup canvasGroup;
    private float smoothing = 4;
    private float target = 1;
	// Use this for initialization
	virtual public  void Start () {

        slotList = GetComponentsInChildren<Slot>();//获取所有物品槽信息
        canvasGroup = GetComponent<CanvasGroup>();
	}
    public void Update()
    {
        if (canvasGroup.alpha != target)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, target, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - target) < 0.03f)
            {
                canvasGroup.alpha = target;
            }
        }
    }


    #region 需要一个方法 来将物品装进背包或者箱子,该方法让需要的子类继承
    public bool StoreItem(int id)
    {//根据物品的ID来判断物品种类
        //因为所有的物品信息都保存在InvenManager的itemList列表中，所以调用其中的方法来获取对应物品的对象
        Item item = InventoryManager.Instance.GetItemByID(id);
        return StoreItem(item);//将查找到的对象给下面重载函数继续判断成功与否
    }
    public bool StoreItem(Item item)
    {//根据物品对象来判断物品种类
        if (item == null)
        {//如果查询为空 说明物品不在物品对象列表中 即物品不存在
            Debug.LogWarning("输入的ID没有该物品");
            return false;
        }
        else
        {//否则即说明物品存在，接下来将物品 存入背包
            if (item.capacity == 1)
            {//该物品放入的物品槽中只允许放入 一个该物品时
                //调用将物品添加到空物品槽的方法，物品加入物品槽的方法 在物品槽脚本中实现
                foreach(Slot slot in slotList)
                {
                    if (slot.transform.childCount == 0)
                    {//即物品槽下面没有物品
                        //将物品装入槽
                        slot.StoreItem(item);//执行插入算法
                        return true;//表示查找并添加成功
                    }
                }
                //即说明没有空格子了,
                Debug.LogWarning("没有空的物品槽了,查找成功 但添加失败");
                return false;
            }else
            {//及物品限定数量大于1
                foreach(Slot slot in slotList)
                {
                    //分两种情况
                    //物品槽上该物品数量小于该物品的限定数量，此时只需将物品对象的amonut数量加一
                    //物品槽上该物品数量大于等于该物品限定数量，此时需要另外找到一个空格子将物品放入
                    if (slot.transform.childCount!=0&& slot.transform.GetChild(0).GetComponent<ItemUI>().amount<item.capacity&& slot.transform.GetChild(0).GetComponent<ItemUI>().item.id == item.id/*判断物品槽上的物品是否与待添加物品相同*/)
                    {//找到的物品槽要满足三个条件 物品槽中已经有物品了  物品槽中的物品是要待插入的物品种类  物品槽中的物品的数量小于该物品的限定数量  
                        //符合上述的三个条件才能算是找到了一个可插入的与待查入物品相同的物品槽信息
                        //此时便可以执行插入操作，此时只用执行amount增加的方法即可
                        slot.StoreItem(item);//执行插入算法
                        return true;
                    }else
                    {
                        if (slot.transform.childCount == 0)
                       {//此时说明物品槽上的物品都与待查入物品不同，或者已满， 所以遍历到了物品槽为空的物品槽
                            //此时将该物品插入到空物品槽中
                            slot.StoreItem(item);//执行插入算法
                            return true;
                        }
                    }
                }
            }
        }
        return true;
    }
    #endregion

    /// <summary>
    /// 控制箱子和背包的显示
    /// </summary>
    public void DisplaySwitch()
    {
        if (target <= 0.1)
        {//显示
            canvasGroup.blocksRaycasts = true;
            target = 1;
        }else
        {//隐藏
            canvasGroup.blocksRaycasts = false;//隐藏之后阻止交互
            target = 0;
        }
    }
    /// <summary>
    /// 保存所有面板信息的具体方法
    /// </summary>
    public void SaveItemPanel()
    {
        StringBuilder sb = new StringBuilder();
        foreach(Slot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {//说明槽上有物品，此时需要将物品属性信息 添加到字符串
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                sb.Append(itemUI.item.id + "," + itemUI.amount + "-");//将属性id和数量添加到字符串
                                                                      //注意此时字符串的结尾都会有一个“-”符号，这样就给接下来根据“-”来分割字符串带来了麻烦，
                //麻烦的解决方法就是将解析所得物品的最后一个对象舍弃
            }
            else
            {
                //否则说明槽是空的，空槽用“0-”，来进行表示，
                sb.Append("0-");
            }
        }
        //遍历完物品槽 就将物品槽信息保存在了sb中了，此时需要将它存储在本地缓存中,sb不是字符串类型，需要转换
        PlayerPrefs.SetString(this.gameObject.transform.name, sb.ToString());
    }
    /// <summary>
    /// 加载所有面板信息的具体方法
    /// </summary>
    public void LoadItemPanel()
    {
        if (PlayerPrefs.HasKey(this.gameObject.name) == false)
        {//判断如果缓冲区不存在this.gameObject.name这个文件信息，那么就返回不再进行解析
            return;
        }
        string str = PlayerPrefs.GetString(this.gameObject.name);//通过关键字名 获取缓冲区文件
        string[] itemArray = str.Split('-');
        for(int i = 0; i < itemArray.Length - 1; i++) //用减1来避免最后一个字符的错误
        {//不用foreach遍历是因为，要使用索引来作为物品槽的位置指针
            if (itemArray[i] != "0")
            {//若果数组对象结果不是0，便进行分割属性以及添加到相应物品槽的操作
                string[] itemStr = itemArray[i].Split(',');//分割出属性来
                int id = int.Parse(itemStr[0]);
                int amount = int.Parse(itemStr[1]);
                Item item = InventoryManager.Instance.GetItemByID(id);//通过id获取物品
                for (int j = 0; j < amount; j++)
                {//通过遍历物品数量来穿件物品
                    slotList[i].StoreItem(item);
                }
            }
            
        }
    }
}
