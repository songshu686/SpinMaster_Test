using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBtnLogic : MonoBehaviour
{
    public int levelIndex = 1; 

    void Start()
    {
        int maxUnlocked = PlayerPrefs.GetInt("MaxUnlockedLevel", 1);
        if (levelIndex > maxUnlocked)
        {
            Button btn = GetComponent<Button>();
            if (btn != null) btn.interactable = false; 
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f); 
        }
    }

    public void OnLevelClicked()
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex); // 存下当前关卡
        PlayerPrefs.Save();

        if (levelIndex == 1) SceneManager.LoadScene("SampleScene");
        else if (levelIndex == 2) SceneManager.LoadScene("Level2");
    }
}