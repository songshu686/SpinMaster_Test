using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayLatestLevel : MonoBehaviour
{
    public void OnPlayClicked()
    {
        // 读取目前最高打到哪一关
        int maxUnlocked = PlayerPrefs.GetInt("MaxUnlockedLevel", 1);
        
        // 如果最高解锁到了 2 或以上，强制锁定进第 2 关
        if (maxUnlocked >= 2)
        {
            PlayerPrefs.SetInt("CurrentLevel", 2);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Level2");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("SampleScene");
        }
    }
}