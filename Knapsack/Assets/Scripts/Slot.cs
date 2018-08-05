using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    public GameObject itemImage;//创建对物品对象预设的引用
    [HideInInspector]



    public void StoreItem(Item item)
    {
        //插入物品时分两种情况
        //向一个有物品的物品槽上添加物品，此时只需修改物品上的属性amount
        //向一个空物品槽上添加物品，此时需要根据要添加的物品对象信息 实例化出一个游戏对象挂载到slot上
        if (transform.childCount != 0/*&& transform.GetChild(0).GetComponent<ItemUI>().amount < item.capacity*/)
        {//当满足槽上有物品，且物品数量小于限定数量时
            //即说明此时只需将自身上已存在的物品的数量加一
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
        else
        {//否则就是在空的槽上创建新对象
            GameObject itemObject = GameObject.Instantiate(itemImage);//实例化一个物品对象
            itemObject.transform.SetParent(transform);//将实例化的游戏对象挂载到物品槽上
            itemObject.transform.localScale = new Vector3(1, 1, 1);//将物品的尺寸归为原始设置的尺寸
            itemObject.transform.position = transform.position;
            transform.GetChild(0).GetComponent<ItemUI>().SetItem(item);//此时就将这个JSON解析获得的对象信息  放到刚创建的物品的ItemUI中的item中，实现对信息的保存
            //接下来是依据JSON对象item的信息对新创建的游戏对象进行重置
        }
    }
    #region 实现鼠标进入与退出时监听的接口，功能是实现对提示条的显示控制
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string name = transform.GetChild(0).GetComponent<ItemUI>().item.GetNameToolTip();
            InventoryManager.Instance.ShowToolTip(name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            InventoryManager.Instance.HideToolTip();
        }
    }


    #endregion

    #region 实现对鼠标按下的监听控制
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {//如果点击了鼠标右键 则执行穿戴操作
            if (transform.childCount > 0 && InventoryManager.Instance.IspickedItem==false)
            {//大于0 说明物品槽上有物品
                ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
                if(currentItem.item is Equipment||currentItem.item is Weapon)
                {
                    CharacterPanel.Instance.PutOn(currentItem);
                }
            }
        }
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (transform.childCount > 0)
        {//此时说明物品槽上有物品
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();//获取此时物品槽上物品的引用
            if (InventoryManager.Instance.IspickedItem==false)
            {//即鼠标上没有被选中物品 此时将物品槽上的物品加到鼠标上
                if (Input.GetKey(KeyCode.LeftControl))
                {//如果持续按下左Ctrl键则将一般物品放到鼠标上
                    //此处执行物品信息到鼠标上pickedItem的信息传递
                    int amountPicked = (currentItem.amount + 1) / 2;
                    int currentRemain = currentItem.amount - amountPicked;//当前物品槽上物品还剩余多少
                    InventoryManager.Instance.SetPickedItem(currentItem.item,amountPicked,this.transform);
                    if (currentRemain <= 0)
                    {
                        Destroy(currentItem.gameObject);
                        InventoryManager.Instance.SetCurrenSlot(true);

                    }
                    else
                    {
                        currentItem.SetAmount(currentRemain);
                        InventoryManager.Instance.SetCurrenSlot(false);

                    }
                }
                else//否则即将所有物品都放到鼠标上
                {
                    InventoryManager.Instance.SetPickedItem(currentItem.item,currentItem.amount,this.transform);
                    Destroy(currentItem.gameObject);
                    InventoryManager.Instance.SetCurrenSlot(true); 
                }
            }else
            {//即是说鼠标上已经有物品了
                if (currentItem.item.id == InventoryManager.Instance.PickedItem.item.id)
                {//判断当前槽上的物品是否与鼠标上的物品一样，如果一样的话执行如下操作
                    if (Input.GetKey(KeyCode.LeftControl))
                    {//如果此时按下了ctrl键，那么将鼠标上的物品放到槽上一个，否则全部放上去
                        if (currentItem.item.capacity > currentItem.amount)
                        {//判断物品槽上物品是否已经装满，若没装满则还可以继续装，若装满，则不执行放入操作，直接返回即可
                            InventoryManager.Instance.RemoveItem();//默认减少一个
                            currentItem.AddAmount();//物品槽上的物品数量默认增加一个
                        }else
                        {
                            return;
                        }

                    }else
                    {//没有按ctrl的时候，如果槽上有足够的空间，那么就将所有的物品都放到槽上去，如果空间不足，就将槽装满即可
                        if (currentItem.item.capacity > currentItem.amount)
                        {//判断物品槽上物品是否已经装满，若没装满则还可以继续装，若装满，则不执行放入操作，直接返回即可
                            int remainCapacity = currentItem.item.capacity - currentItem.amount;
                            if (remainCapacity >= InventoryManager.Instance.GetPickedAmount())
                            {//即预留的空间大于鼠标上的物品数量 那么将鼠标上所有物品都放到槽上去
                                //因为两次都使用了，pickedItem上的数量值所以下面这两句的顺序不能改变，
                                currentItem.AddAmount(InventoryManager.Instance.GetPickedAmount());//物品槽上的物品数量增加为鼠标上的物品数量
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.GetPickedAmount());//将鼠标上的信息减少到0
                            }
                            else
                            {//否则即说明，物品槽上的空间不足那么只将物品槽装满即可
                                InventoryManager.Instance.RemoveItem(remainCapacity);
                                currentItem.AddAmount(remainCapacity);
                            }
                            
                        }
                        else
                        {
                            return;
                        }
                    }
                }else
                {//物品槽上的物品与鼠标上的物品不相同，此时交换这两个物品槽上的物品
                    print(InventoryManager.Instance.IsItemSlot);
                    if (InventoryManager.Instance.IsItemSlot==true)
                    {//即 当被取得物品槽上所有物品都被选取的情况下，才可以执行交换方法，像那种只拾取了一部分的，不允许执行交换操作
                        InventoryManager.Instance.SetItemParent(currentItem);//设置当前物品槽上物品的父对象
                                                                             //将鼠标上的物品放到当前物品槽上，此时需要另外创建新对象
                         for(int i = 0; i < InventoryManager.Instance.GetPickedAmount(); i++)
                        {
                            this.StoreItem(InventoryManager.Instance.PickedItem.item);
                            if (InventoryManager.Instance.PickedItem.amount == i + 1)
                            {
                                InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.amount);
                            }
                        }

                    }
                    
                }
            }
        }else
        {//否则说明物品槽上没有物品
            if (InventoryManager.Instance.IspickedItem == true)
            {//如果鼠标上有物品，那么要将物品放到空槽上
                if (Input.GetKey(KeyCode.LeftControl))
                {//若果按下ctrl键 那么将鼠标上的物品一个放入空格子，不用考虑第二次在这个语句中的操作，因为第二次，槽上就有物品了，点击鼠标执行的事件，将是上面的操作
                    this.StoreItem(InventoryManager.Instance.PickedItem.item);
                    InventoryManager.Instance.RemoveItem();
                }else
                {//不按下ctrl时将所有物品都放入到槽上
                    for(int i = 0; i < InventoryManager.Instance.PickedItem.amount;i++)
                    {
                        this.StoreItem(InventoryManager.Instance.PickedItem.item);
                        if (InventoryManager.Instance.PickedItem.amount ==i+1)
                        {
                            InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.amount);
                        }
                    }
                }
            }
        }
    }
    #endregion
}
