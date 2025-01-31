using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private bool timerIsRunning;
    private float timeRemaining;
    public float maxTimeInSeconds;
    public GameObject timeUpPanel;

    private void Awake()
    {
        SetTime(PlayerPrefs.GetFloat("Time", maxTimeInSeconds));
    }

    public void SetTime(float time)
    {
        maxTimeInSeconds = time;
        timeRemaining = maxTimeInSeconds;
        DisplayTime();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (timerIsRunning)
        {
            UpdateTimer();
        }
    }

    public void StartTimer()
    {
        // Set the initial time remaining to the maximum time
        timeRemaining = maxTimeInSeconds;

        // Start the timer
        timerIsRunning = true;
    }

    void UpdateTimer()
    {
        // Update the time remaining
        timeRemaining -= Time.deltaTime;

        // Check if the timer has reached zero
        if (timeRemaining <= 0)
        {
            // Stop the timer when it reaches zero
            timeRemaining = 0;
            timerIsRunning = false;

            FindObjectOfType<PlayerController>().enabled = false;

            timeUpPanel.SetActive(true);
            timeUpPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);

            if (GameObject.Find("BonusLevel"))
                FindObjectOfType<GameManager>().Win();
            else
                FindObjectOfType<GameManager>().Win();
        }

        // Display the remaining time in minutes and seconds
        DisplayTime();
    }

    private void LoadBossScene()
    {
        SceneManager.LoadScene("Boss");
    }

    void DisplayTime()
    {
        // Calculate minutes and seconds from the remaining time
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Update the UI text with the remaining time
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Change text color to red when 5 seconds or less remaining
        if (timeRemaining <= 5)
        {
            timerText.color = Color.red;
        }
        else
        {
            // Reset text color to the default color (e.g., white)
            timerText.color = Color.white;
        }
    }
}
