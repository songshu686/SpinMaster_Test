using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    private bool isDead = false; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead || player == null) return; 
        
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // 已经死了就不做任何判定

        // 1. 碰到飞刀：算击杀，敌人消失
        if (collision.CompareTag("Knife"))
        {
            if (GameManager.Instance != null) 
            {
                GameManager.Instance.OnEnemyKilled(); // 加击杀数
            }
            Die(); 
        }
        // 2. 碰到玩家：玩家扣血，敌人消失（不加击杀数）
        else if (collision.CompareTag("Player"))
        {
            // 找到玩家身上的脚本并扣血
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(1); 
            }
            Die(); // 敌人消失，但不加击杀数
        }
    }

    // 纯粹的消失逻辑，不加分
    void Die()
    {
        isDead = true; 
        
        // 彻底隐藏并关闭碰撞
        gameObject.SetActive(false); 
        Destroy(gameObject, 0.05f); 
    }
}