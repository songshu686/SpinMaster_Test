using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    public GameObject knifePrefab;
    public float rotateSpeed = 180f;
    public float radius = 1f;
    public int knifeCount = 3; 

    private List<GameObject> knives = new List<GameObject>();
    private float currentAngle = 0f;

    void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel",1);
        knifeCount=2+currentLevel;
        rotateSpeed = 180f+(currentLevel * 20f);
        for (int i = 0; i < knifeCount; i++)
        {
            GameObject k = Instantiate(knifePrefab, transform);
            knives.Add(k);
        }
    }

    void Update()
    {
        currentAngle += rotateSpeed * Time.deltaTime;

        for (int i = 0; i < knives.Count; i++)
        {
            float angle = currentAngle + (i * (360f / knives.Count));
            float rad = angle * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            knives[i].transform.localPosition = pos;
            // 让飞刀朝着切线方向（看起来像在切割）
            knives[i].transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}