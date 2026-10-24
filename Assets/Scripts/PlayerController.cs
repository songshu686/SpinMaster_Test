using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public int maxHealth = 6;
    public int currentHealth;

    private AudioSource source;
    private Camera mainCamera;

    void Start()
    {
        source = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        mainCamera = Camera.main; // 获取主相机
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // 1. 获取鼠标位置
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // 2. 【核心隐形墙】：获取相机的屏幕左下角和右上角边界
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

            // 3. 把鼠标位置死死限制在相机视野内
            mousePos.x = Mathf.Clamp(mousePos.x, bottomLeft.x, topRight.x);
            mousePos.y = Mathf.Clamp(mousePos.y, bottomLeft.y, topRight.y);

            // 4. 移动过去
            transform.position = Vector3.MoveTowards(transform.position, mousePos, moveSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        
        if (source != null) source.Play(); // 播放受击音效
        
        currentHealth -= damage;
        if(currentHealth <0) currentHealth = 0;
        Debug.Log("玩家受伤，当前血量: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Debug.Log("游戏结束！");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EndGame();
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log("玩家回血，当前血量: " + currentHealth);
    }
}