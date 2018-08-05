using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

	public Item item { get; set; }//此处item的实例化将会在创建该物品信息时在Slot类中实现，将JSON解析后获得的实例对象赋值给他
    public int amount { get; set; }
    #region UI 引用初始化，即 获取组件，这样就可以在当某个程序调用改引用属性时 立刻实现挂载
    private Image itemImage;
    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
            {
                //itemImage = GameObject.Find("ItemImage").GetComponent<Image>();//不能这样用 因为可能会被实例化出来很多的相同对象
                itemImage = GetComponent<Image>();
            }
            return itemImage;
        }
    }
    private Text amountText;
    private Text AmountText
    {
        get
        {
            if (amountText == null)
            {
                amountText = transform.FindChild("ItemText").GetComponent<Text>();//将text组件获取 给引用
            }
            return amountText;
        }
    }
    #endregion
    //这个地方需要实现一个方法将amoun与自身的text文本绑定，
    /// <summary>
    /// 主要是制作UI从大到小的变化动画
    /// </summary>
    private Vector3 targetScale = new Vector3(1, 1, 1);
    private float smoothing = 4.0f;

        void Update()
    {
        if (transform.localScale.x != targetScale.x)
        {
            float scale = Mathf.Lerp(transform.localScale.x, targetScale.x, smoothing * Time.deltaTime);
            transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Abs(transform.localScale.x - targetScale.x)<0.02f)
            {
                transform.localScale = targetScale;
            }
        }
    }
    //创建以方法来实现对上述两个属性的初始化
    public void SetItem(Item item,int amount = 1)
    {
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);//设置动画的执行位置，只要有对物品做变动的都要进行动画控制
        this.item = item;
        this.amount = amount;
        //更新图片，以及text
        ItemImage.sprite = Resources.Load<Sprite>(item.sprite);
        if (this.amount == 1)
        {
            string s = "";
            AmountText.text = s;
        }else
        {
            AmountText.text = this.amount.ToString();
        }

    }
    public void AddAmount(int amount=1)
    {
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        this.amount += amount;
        AmountText.text = this.amount.ToString();
    }
    //设置物品的数量
    public void SetAmount(int amount)
    {
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        this.amount = amount;
        if (this.amount == 1)
        {
            string s = "";
            AmountText.text = s;
        }
        else
        {
            AmountText.text = this.amount.ToString();
        }
    }
    /// <summary>
    /// 减少物品自身的数量
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveAmount(int amount = 1)
    {
        
        this.amount -= amount;
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        if (this.amount == 1)
        {
            string s = "";
            AmountText.text = s;
        }
        else
        {
            AmountText.text = this.amount.ToString();
        }
    }
    #region 实现两个物品信息的交换
    public void ExchangeItem(ItemUI item)
    {
        int amount = item.amount;
        Item temp = item.item;
        item.SetItem(this.item, this.amount);
        this.SetItem(temp, amount);
        
    }
    #endregion
}
