using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public int sizePrice, timePrice;
    public Text sizeText, timeText;
    public Animator toastAnim;
    private CoinsManager coinsManager;

    private void Start()
    {
        coinsManager = FindObjectOfType<CoinsManager>();

        sizePrice = PlayerPrefs.GetInt("SizePrice", sizePrice);
        timePrice = PlayerPrefs.GetInt("TimePrice", timePrice);

        ShowText();

        if (PlayerPrefs.HasKey("SizeMax"))
            sizeText.text = "Max";

        if (PlayerPrefs.HasKey("TimeMax"))
            timeText.text = "Max";
    }

    private void ShowText()
    {
        sizeText.text = sizePrice.ToString();
        timeText.text = timePrice.ToString();
    }

    public void SizeUp()
    {
        if (PlayerPrefs.HasKey("SizeMax"))
            return;

        AudioManager.Instance.Play("Click");

        if (coinsManager.collectedMoney < sizePrice)
        {
            toastAnim.SetTrigger("Toast");
            return;
        }
        ObjectsDetector objectsDetector = FindObjectOfType<ObjectsDetector>();

        objectsDetector.StartCoroutine(objectsDetector.SizeUp(true, .1f));
        objectsDetector.SaveSize();

        coinsManager.LessMoney(sizePrice);

        sizePrice += 500;

        if (sizePrice > 4500)
        {
            sizeText.text = "Max";
            PlayerPrefs.SetString("SizeMax", "");
        }
        else
        {
            PlayerPrefs.SetInt("SizePrice", sizePrice);
            sizeText.text = sizePrice.ToString();
        }
    }

    public void TimeUp()
    {
        if (PlayerPrefs.HasKey("TimeMax"))
            return;

        AudioManager.Instance.Play("Click");

        if (coinsManager.collectedMoney < timePrice)
        {
            toastAnim.SetTrigger("Toast");
            return;
        }

        float time = PlayerPrefs.GetFloat("Time", FindObjectOfType<Timer>().maxTimeInSeconds);
        time += 5;

        FindObjectOfType<Timer>().SetTime(time);
        PlayerPrefs.SetFloat("Time", time);
        coinsManager.LessMoney(timePrice);

        timePrice += 350;

        if (timePrice > 1650)
        {
            timeText.text = "Max";
            PlayerPrefs.SetString("TimeMax", "");
        }
        else
        {
            PlayerPrefs.SetInt("TimePrice", timePrice);
            timeText.text = timePrice.ToString();
        }
    }
}