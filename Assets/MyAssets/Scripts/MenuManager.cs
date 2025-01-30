using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ShowRewardVideo()
    {
        AudioManager.Instance.Play("Click");
        AdsManager.Instance.ShowRewardedAd();
    }

    public void RateUs()
    {
        AudioManager.Instance.Play("Click");
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void MoreGames()
    {
        AudioManager.Instance.Play("Click");
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void OpenScene(int index)
    {
        AudioManager.Instance.Play("Click");
        SceneManager.LoadScene(index);
    }
}