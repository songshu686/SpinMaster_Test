using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanel : MonoBehaviour
{
    public GameObject targetPanel; // 把 View_LevelSelect 拖进来

    void Start()
    {
        // 获取原生按钮并绑定点击事件
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                if (targetPanel != null) 
                {
                    targetPanel.SetActive(true); // 唤醒面板
                }
            });
        }
    }
}
