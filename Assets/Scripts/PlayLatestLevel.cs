using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayLatestLevel : MonoBehaviour
{
    public void OnPlayClicked()
    {
        // 读取目前最高打到哪一关（默认1）
        int maxUnlocked = PlayerPrefs.GetInt("MaxUnlockedLevel", 1);
        
        // 把最高关卡存进去
        PlayerPrefs.SetInt("CurrentLevel", maxUnlocked);
        PlayerPrefs.Save();

        // 根据最高关卡跳转
        if (maxUnlocked == 1) SceneManager.LoadScene("SampleScene"); 
        else SceneManager.LoadScene("Level2"); 
    }
}