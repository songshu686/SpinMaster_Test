using UnityEngine;
using Doozy.Engine.UI;

public class ChestLogic : MonoBehaviour
{
    public float moveSpeed = 2f; 
    private Vector2 targetPos;
    public float minX = -4f, maxX = 4f;
    public float minY = -3f, maxY = 3f;

    void Start()
    {
        targetPos = GetRandomPosition();
        
        UIView view = GetComponent<UIView>();
        if (view != null) view.Show();

        Destroy(gameObject, 5f); 
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = GetRandomPosition();
        }
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 刀碰到宝箱：无作用，无特效，直接跳过
        if (collision.CompareTag("Knife")) return;

        // 玩家碰到宝箱：加血，爆特效，瞬间消失
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null) pc.Heal(1); // 回 1 血

            // 爆特效
            if (GameManager.Instance != null && GameManager.Instance.explosionEffectPrefab != null)
            {
                Instantiate(GameManager.Instance.explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // 瞬间消失
            gameObject.SetActive(false); 
        }
    }
}