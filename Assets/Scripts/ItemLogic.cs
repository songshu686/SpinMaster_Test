using UnityEngine;
using Doozy.Engine.UI; // 引入 Doozy

public class ItemLogic : MonoBehaviour
{
    public bool isBomb = true; 
    public float moveSpeed = 5f; 
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        UIView view = GetComponent<UIView>();
        if (view != null) view.Show();

        if (isBomb) Destroy(gameObject, 3f); // 3秒自动消失
    }

    void Update()
    {
        if (isBomb && player != null)
        {
            // 追踪玩家
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knife") || collision.CompareTag("Player"))
        {
            // 1. 播放爆炸特效
            if (GameManager.Instance != null && GameManager.Instance.explosionEffectPrefab != null)
            {
                Instantiate(GameManager.Instance.explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // 2. 寻找玩家进行秒杀
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc == null) pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            if (pc != null)
            {
                if (isBomb) pc.TakeDamage(999); // 炸弹秒杀
            }

            // 3. 瞬间消失（直接隐藏物体，不播动画，立刻消失）
            gameObject.SetActive(false); 
        }
    }
}