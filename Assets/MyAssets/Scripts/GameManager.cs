using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject retryPanel, winPanel, startPanel, commingSoonPanel, gamePanel;
    public Text levelText;
    [HideInInspector] public int currentLevelno;
    public Timer timer;
    public bool gameStarted;

    void Awake()
    {
        Application.targetFrameRate = 60;
        currentLevelno = PlayerPrefs.GetInt("Level", 0);

        if (SceneManager.GetActiveScene().buildIndex != currentLevelno)
            SceneManager.LoadScene(currentLevelno);

        levelText.text = "Level " + currentLevelno+1.ToString();
    }

    private void Start()
    {
        ItemsManager.Instance.collectedItemsName = new List<string>();
        AudioManager.Instance.audioBtnImage.transform.parent.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetInt("Level", currentLevelno + 1);
        } 
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.DeleteKey("Time");
            PlayerPrefs.DeleteKey("Size");
        }

        // Check for user input to trigger the screenshot
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Capture the screenshot and save it to the specified file path
            CaptureScreenshot();
        }
    }
    public string screenshotFolder = "Screenshots";

    void CaptureScreenshot()
    {
        // Create a folder if it doesn't exist
        string folderPath = System.IO.Path.Combine(Application.persistentDataPath, screenshotFolder);
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        // Generate a unique file name using a timestamp
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string screenshotFileName = "Screenshot_" + timestamp + ".png";

        // Combine the file name and the folder path where the screenshot will be saved
        string screenshotPath = System.IO.Path.Combine(folderPath, screenshotFileName);

        // Capture the screenshot and save it to the specified file path
        ScreenCapture.CaptureScreenshot(screenshotPath);

        // Print a message to the console to indicate that the screenshot has been captured
        Debug.Log("Screenshot captured and saved to: " + screenshotPath);
    }

    public void Reload()
    {
        AudioManager.Instance.Play("Click");

        AdsManager.Instance.ShowInterstitialAd();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        AudioManager.Instance.Play("Click");
        startPanel.SetActive(false);
        gamePanel.SetActive(true);

        AudioManager.Instance.audioBtnImage.transform.parent.gameObject.SetActive(false);

        timer.StartTimer();
        gameStarted = true;
    }

    public void Win()
    {
        Invoke("InvokeWinGame", 1);
    }

    private void InvokeWinGame()
    {
        print("Win");
        AudioManager.Instance.Play("Win");

        if (currentLevelno < 10)
        {
            winPanel.SetActive(true);
            winPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
        }
        else
        {
            commingSoonPanel.SetActive(true);
            commingSoonPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
        }
    }

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        AudioManager.Instance.Play("Click");
        AdsManager.Instance.ShowInterstitialAd();
        PlayerPrefs.SetInt("Level", currentLevelno + 1);

        PlayerPrefs.DeleteKey("Size");
        PlayerPrefs.DeleteKey("Time");
        PlayerPrefs.DeleteKey("SizeMax");
        PlayerPrefs.DeleteKey("TimeMax");
        PlayerPrefs.DeleteKey("SizePrice");
        PlayerPrefs.DeleteKey("TimePrice");
    }

    public void SoundToggle()
    {
        AudioManager.Instance.Play("Click");
        AudioManager.Instance.SoundToggle();
    }

    public void Home()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void ShowRewardVideo()
    {
        AdsManager.Instance.ShowRewardedAd();
    }

    public void OpenScene(int index)
    {
        AudioManager.Instance.Play("Click");
        SceneManager.LoadScene(index);
    }
}
