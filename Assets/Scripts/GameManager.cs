using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("UI与音效")]
    public AudioSource sfxSource;
    public GameObject gameOverPanel;

    [Header("关卡数据")]
    public int currentLevel = 1;
    public int enemiesToKill = 5;
    public int enemiesKilled = 0;

    [Header("预制体")]
    public GameObject enemyPrefab;
    public GameObject explosionEffectPrefab;
    public GameObject bombPrefab;
    public GameObject chestPrefab;

    // 记录道具刷新的次数
    private int bombSpawnedCount = 0; 
    private int chestSpawnedCount = 0; 

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        
        // 读取关卡
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        
        // 设置击杀目标
        if (currentLevel == 2) 
        {
            enemiesToKill = 10; // 第二关10个怪
        }
        else 
        {
            enemiesToKill = 3 + currentLevel;
        }
        
        enemiesKilled = 0;
        bombSpawnedCount = 0; // 重置道具计数
        chestSpawnedCount = 0;
        
        // 开始刷怪
        float spawnInterval = Mathf.Max(0.5f, 2.5f - (currentLevel * 0.5f));
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);

        // 第二关开始刷道具（每 6 秒判断一次刷什么）
        if (currentLevel == 2)
        {
            InvokeRepeating("SpawnItem", 3f, 6f); 
        }

        Debug.Log("当前进入：第 " + currentLevel + " 关，需击杀：" + enemiesToKill + " 个");
    }

    void SpawnEnemy()
    {
        if (enemiesKilled >= enemiesToKill) { CancelInvoke("SpawnEnemy"); return; }
        
        Vector3 spawnPos = new Vector3(Random.Range(-4f, 4f), 4.5f, 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    void SpawnItem()
    {
        // 如果炸弹刷了 2 个，且宝箱也刷了 2 个，就彻底停止刷道具
        if (bombSpawnedCount >= 2 && chestSpawnedCount >= 2)
        {
            CancelInvoke("SpawnItem");
            return;
        }

        PlayerController player = FindObjectOfType<PlayerController>();
        
        // 【核心逻辑】：判断血量 <= 3 且 宝箱没刷满 2 个
        if (player != null && player.currentHealth <= 3 && chestSpawnedCount < 2)
        {
            // 刷宝箱：坐标在屏幕内随机 (X:-4 到 4, Y:-2 到 2)
            Vector3 chestPos = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), 0);
            Instantiate(chestPrefab, chestPos, Quaternion.identity);
            chestSpawnedCount++;
        }
        else if (bombSpawnedCount < 2)
        {
            // 如果血量健康，或者宝箱暂时不能刷，就刷炸弹：从正上方出 (Y = 5f)
            Vector3 bombPos = new Vector3(Random.Range(-4f, 4f), 5f, 0);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
            bombSpawnedCount++;
        }
        else if (chestSpawnedCount < 2)
        {
            // 炸弹刷满了，但玩家残血了，继续补刷宝箱
            Vector3 chestPos = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), 0);
            Instantiate(chestPrefab, chestPos, Quaternion.identity);
            chestSpawnedCount++;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("击杀数：" + enemiesKilled + "/" + enemiesToKill);
        if (enemiesKilled >= enemiesToKill) LevelComplete();
    }

    void LevelComplete()
    {
        Debug.Log("通关！解锁下一关！");
        DG.Tweening.DOTween.KillAll(); 
        
        int maxUnlocked = PlayerPrefs.GetInt("MaxUnlockedLevel", 1);
        if (currentLevel >= maxUnlocked)
        {
            PlayerPrefs.SetInt("MaxUnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene("MainMenu"); 
    }

    public void EndGame()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
}