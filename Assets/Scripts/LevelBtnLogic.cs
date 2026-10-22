using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBtnLogic : MonoBehaviour
{
    // 【新增这句】：有了它，Inspector里才会出现 Level Index 的输入框
    public int levelIndex = 1; 

    // 这个方法给按钮点击用
    public void OnLevelClicked()
    {
        // 记住玩家点的是第几关
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();
        
        // 跳转到打怪场景，确保你打怪场景的名字叫 SampleScene
        SceneManager.LoadScene("SampleScene");
    }
}