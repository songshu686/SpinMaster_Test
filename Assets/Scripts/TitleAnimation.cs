using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 originalPos = rect.anchoredPosition; // 记录当前位置
        rect.anchoredPosition = new Vector2(originalPos.x, 500f); // 初始移到屏幕上方

        rect.DOAnchorPos(originalPos, 0.8f)
            .SetEase(Ease.OutBack); // OutBack 就是回弹效果
    }
}