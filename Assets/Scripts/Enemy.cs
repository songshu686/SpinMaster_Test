using System.Collections;
using System.Collections.Generic;
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
        // 找玩家
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;
        
        // 追着玩家跑
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // 1. 碰到飞刀，加击杀数，然后死亡
        if (collision.CompareTag("Knife"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnEnemyKilled(); 
            }
            Die();
        }
        // 2. 碰到玩家，玩家扣血，然后死亡
        else if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(1);
            }
            Die();
        }
    }

    // 统一的死亡逻辑
    void Die()
    {
        isDead = true;

        // 【核心】：在敌人死的位置，生成爆炸特效
        if (GameManager.Instance != null && GameManager.Instance.explosionEffectPrefab != null)
        {
            Instantiate(GameManager.Instance.explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 瞬间隐藏并销毁
        gameObject.SetActive(false);
        Destroy(gameObject, 0.05f);
    }
}