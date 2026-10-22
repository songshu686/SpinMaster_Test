using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseBtn : MonoBehaviour
{
    // 不要用 UIView 了，直接拖 GameObject
    public GameObject targetPanel; 

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                if (targetPanel != null) 
                {
                    targetPanel.SetActive(false); // 直接隐藏物体
                }
            });
        }
    }
}