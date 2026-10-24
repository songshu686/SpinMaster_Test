using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBtnLogic : MonoBehaviour
{
    public int levelIndex = 1; 

    public void OnLevelClicked()
    {
        // 1. 记住玩家点的是第几关
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();
        
        // 2. 根据关卡跳转
        if (levelIndex == 1)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (levelIndex == 2)
        {
            SceneManager.LoadScene("Level2");
        }
    }
}