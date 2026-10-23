using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 【关键】：必须引用 TMP 的命名空间

public class UIManager : MonoBehaviour
{
    public TMP_Text healthText;    // 对应左上角血量文本
    public TMP_Text killCountText; // 对应右上角击杀数文本

    private PlayerController player;
    private GameManager gameManager;

    void Start()
    {
        // 游戏开始时，自动在场景里找到玩家和大总管
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // 每一帧实时更新文字
        if (player != null && healthText != null)
        {
            healthText.text = "Health: " + player.currentHealth;
        }
        if (gameManager != null && killCountText != null)
        {
            killCountText.text = "KillCount:" + gameManager.enemiesKilled + "/" + gameManager.enemiesToKill;
        }
    }
}