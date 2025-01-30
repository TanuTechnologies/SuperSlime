using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackItems : MonoBehaviour
{
    public float force;
    public float itemThrowInterval;
    public Transform throwPoint, throwPoint1, throwPoint2;

    private int throwPointIndex = 0;
    public GameObject trailEffectPrefab;
    [HideInInspector] public int throwItemsFunCount, itemCount;

    private void Start()
    {
        itemCount = ItemsManager.Instance.collectedItemsName.Count;
    }

    public void StartThrow()
    {
        InvokeRepeating(nameof(ThrowItems), 0, itemThrowInterval);
    }

    private void ThrowItems()
    {
        for (int i = 0; i < 3; i++)
        {
            if (itemCount <= 0)
            {
                CancelInvoke(nameof(ThrowItems));
                Invoke(nameof(CheckBossKilled), 1);
                return;
            }

            AudioManager.Instance.Play("Click");

            GameObject trailObj = Instantiate(trailEffectPrefab, Vector3.zero, Quaternion.identity, transform);
            trailObj.transform.localPosition = Vector3.zero;

            if (throwPointIndex == 2)
                throwPointIndex = 0;
            else
                throwPointIndex++;

            ThrowItem(trailObj);
        }

        CheckFunCallCount();
    }

    private void CheckFunCallCount()
    {
        throwItemsFunCount++;

        if (throwItemsFunCount > 24)
        {
            throwItemsFunCount = 0;
            CancelInvoke(nameof(ThrowItems));
            CheckBossKilled();
        }
    }

    private void ThrowItem(GameObject item)
    {
        switch (throwPointIndex)
        {
            case 0:
                item.GetComponent<Rigidbody>().AddForce(throwPoint.forward * force, ForceMode.Impulse);
                break;

            case 1:
                item.GetComponent<Rigidbody>().AddForce(throwPoint1.forward * force, ForceMode.Impulse);
                break;

            case 2:
                item.GetComponent<Rigidbody>().AddForce(throwPoint2.forward * force, ForceMode.Impulse);
                break;
        }
    }

    private void CheckBossKilled()
    {
        FindObjectOfType<Boss>().CheckBossIsKilled();
    }

    public void OnClickNext()
    {
        PlayerPrefs.DeleteKey("Size");
        PlayerPrefs.DeleteKey("Time");
        PlayerPrefs.DeleteKey("SizeMax");
        PlayerPrefs.DeleteKey("TimeMax");
        PlayerPrefs.DeleteKey("SizePrice");
        PlayerPrefs.DeleteKey("TimePrice");

        AudioManager.Instance.Play("Click");
        AdsManager.Instance.ShowInterstitialAd();

        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 0) + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        PlayerPrefs.SetInt("BossIndex", PlayerPrefs.GetInt("BossIndex", 0) + 1);
    }

    public void OnClickRetry()
    {
        AudioManager.Instance.Play("Click");
        AdsManager.Instance.ShowInterstitialAd();

        int currentLevelno = PlayerPrefs.GetInt("Level", 0);
        SceneManager.LoadScene(currentLevelno);
    }
}