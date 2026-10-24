using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening; // 必须引入

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
    public GameObject killCountUI;
    public GameObject explosionEffectPrefab;
    public GameObject bombPrefab;
    public GameObject chestPrefab;

    private int bombSpawnedCount = 0; 
    private int chestSpawnedCount = 0; 

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        
        // 读取当前关卡
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        
        // 设置击杀目标
        if (currentLevel == 2)
        {
            currentLevel = 2;
            enemiesToKill = 10; 
        }
        else 
        {
            enemiesToKill = 3 + currentLevel;
        }
        
        enemiesKilled = 0;
        bombSpawnedCount = 0; 
        chestSpawnedCount = 0;
        
        // 刷怪
        float spawnInterval = Mathf.Max(0.5f, 2.5f - (currentLevel * 0.5f));
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);

        // 刷道具（仅第二关）
        if (currentLevel == 2)
        {
            InvokeRepeating("SpawnItem", 3f, 6f); 
        }
    }

    void SpawnEnemy()
    {
        if (enemiesKilled >= enemiesToKill) { CancelInvoke("SpawnEnemy"); return; }
        Vector3 spawnPos = new Vector3(Random.Range(-4f, 4f), 4.5f, 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    void SpawnItem()
    {
        if (bombSpawnedCount >= 2 && chestSpawnedCount >= 2) { CancelInvoke("SpawnItem"); return; }
        PlayerController player = FindObjectOfType<PlayerController>();
        
        if (player != null && player.currentHealth <= 3 && chestSpawnedCount < 2)
        {
            Vector3 chestPos = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), 0);
            Instantiate(chestPrefab, chestPos, Quaternion.identity);
            chestSpawnedCount++;
        }
        else if (bombSpawnedCount < 2)
        {
            Vector3 bombPos = new Vector3(Random.Range(-4f, 4f), 5f, 0);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
            bombSpawnedCount++;
        }
    }

    public void OnEnemyKilled()
    {
        // 【关键修改】：如果已经达标了，再多杀也不加了！
        if (enemiesKilled >= enemiesToKill) return;

        enemiesKilled++;
        Debug.Log("击杀数：" + enemiesKilled + "/" + enemiesToKill);
    
        if (enemiesKilled >= enemiesToKill) 
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        Debug.Log("通关！解锁下一关！");
        
        // 【通关动画和音效】：只有第二关通关才播
        if (currentLevel == 2) 
        {
            if (sfxSource != null) sfxSource.Play();
    
            // 1. 找到文字并播放弹跳动画
            GameObject killTextObj = GameObject.Find("KillCountText");
            if (killTextObj != null)
            {
                killTextObj.transform.localScale = Vector3.one; // 先归位为1
                // 播放动画，并把动画的 ID 设为 99
                killTextObj.transform.DOScale(1.5f, 0.3f).SetLoops(2, LoopType.Yoyo).SetId(99);
            }
        }

// 2. 【关键修改】：清理动画时，放过 ID 为 99 的通关动画
        DG.Tweening.DOTween.KillAll();
        
        // 存档解锁
        int maxUnlocked = PlayerPrefs.GetInt("MaxUnlockedLevel", 1);
        if (currentLevel >= maxUnlocked && currentLevel < 3) 
        {
            PlayerPrefs.SetInt("MaxUnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }

        // 【核心修复】：切场景前强制把下一关的数据写入，不用 Invoke 延迟了，直接切！
        if (currentLevel == 1) 
        {
            PlayerPrefs.SetInt("CurrentLevel", 2); // 强制写入第二关的数据
            PlayerPrefs.Save();
            SceneManager.LoadScene("Level2");
        }
        else 
        {
            // 如果是第二关通关，给个 1.5 秒延迟让音效播完，再回主菜单
            Invoke("LoadMainMenu", 1.5f);
        }
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void EndGame()
    {
        DG.Tweening.DOTween.KillAll();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartLevel()
    {
        DG.Tweening.DOTween.KillAll();
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        DG.Tweening.DOTween.KillAll();
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
}