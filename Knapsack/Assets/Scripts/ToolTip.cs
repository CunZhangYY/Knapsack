using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {
    private Text toolTipText;
    private Text contentText;

    private CanvasGroup textAlpha;

    private  float targetAlpha = 1.0f;

    public float smoothing = 3.0f;

	// Use this for initialization
	void Start () {
        toolTipText = GetComponent<Text>();
        contentText = transform.Find("ContentText").GetComponent<Text>();
        textAlpha = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
        if (textAlpha.alpha != targetAlpha)
        {
            textAlpha.alpha = Mathf.Lerp(textAlpha.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(textAlpha.alpha - targetAlpha) < 0.01f)
            {
                textAlpha.alpha = targetAlpha;
            }
        }
	}


    #region 显示与隐藏工具提示条
    public void Show(string text)
    {
        targetAlpha = 1.0f;
        toolTipText.text = text;
        contentText.text = text;
    }

    public void Hide()
    {
        targetAlpha = 0;
    }

    public void SetLocalPos(Vector3 pos)
    {
        transform.localPosition = pos;//这个当地坐标表示很重要
    }
    #endregion
}
