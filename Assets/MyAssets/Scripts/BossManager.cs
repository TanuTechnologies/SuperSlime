using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject winPanel, retryPanel;
    public GameObject[] bossPrefabs;

    void Awake()
    {
        int bossIndex = PlayerPrefs.GetInt("BossIndex", 0);
        print(bossIndex);
        Instantiate(bossPrefabs[bossIndex], transform);
    }
}