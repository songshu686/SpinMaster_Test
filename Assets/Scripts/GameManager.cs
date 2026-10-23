using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 【新增这句】：用来存你的面板
    public GameObject gameOverPanel; 

    public int currentLevel = 1;
    public int enemiesToKill = 5;
    public int enemiesKilled = 0;
    public GameObject enemyPrefab;

    public GameObject explosionEffectPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // 读取关卡等逻辑...
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        enemiesToKill = 3 + currentLevel; 
        enemiesKilled = 0;
        InvokeRepeating("SpawnEnemy", 1f, 2f);
    }

    void SpawnEnemy()
    {
        if (enemiesKilled >= enemiesToKill) { CancelInvoke("SpawnEnemy"); return; }
        Vector3 spawnPos = new Vector3(Random.Range(-6f, 6f), 6f, 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
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

    // 【重点修改这里】
    public void EndGame()
    {
        // 直接激活你拖进来的面板，不用 Find 了！
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
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