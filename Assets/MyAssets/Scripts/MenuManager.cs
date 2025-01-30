using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OpenScene(int index)
    {
        AudioManager.Instance.Play("Click");
        SceneManager.LoadScene(index);
    }
}