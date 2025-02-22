using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsManager : MonoBehaviour
{
    [HideInInspector]
    public int collectedMoney;
    public Text collectedMoneyText;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MoneyAmount"))
        {
            Debug.Log("IF  " + PlayerPrefs.GetInt("MoneyAmount"));
            collectedMoney = PlayerPrefs.GetInt("MoneyAmount");
        }
        else
        {
            PlayerPrefs.SetInt("MoneyAmount",0);
            collectedMoney = PlayerPrefs.GetInt("MoneyAmount");
            Debug.Log("else" + PlayerPrefs.GetInt("MoneyAmount"));
        }
        
        ShowAndSave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            AddMoney(50000);
    }

    public void AddMoney(int amount)
    {
        collectedMoney += amount;
        Debug.Log(amount);
        ShowAndSave();
    }

    public void LessMoney(int amount)
    {
        collectedMoney -= amount;
        ShowAndSave();
    }

    public void ShowAndSave()
    {
        SetMoneyText(collectedMoney);
        PlayerPrefs.SetInt("MoneyAmount", collectedMoney);
    }

    public void SetMoneyText(int amount)
    {
        collectedMoneyText.text = "$" + amount.ToString();
    }
}
