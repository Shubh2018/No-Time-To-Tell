using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text winText;
    public TMP_Text nextSceneText;
    public TMP_Text loseText;
    public TMP_Text retryText;
    public TMP_Text quitText;
    int min;
    int sec;

    private void Start()
    {
        winText.enabled = false;
        nextSceneText.enabled = false;
        loseText.enabled = false;
        retryText.enabled = false;
        quitText.enabled = false;
    }

    private void Update()
    {
        if (GameManager.instance.TotalTime > 0)
        {
            GameManager.instance.TotalTime -= Time.deltaTime;
            sec = (int)(GameManager.instance.TotalTime % 60);
            min = (int)(GameManager.instance.TotalTime / 60);
        }
        
        if(sec >= 10)
            text.text = min.ToString() + ":" + sec.ToString();
        else
            text.text = min.ToString() + ":0" + sec.ToString();

        if (GameManager.instance.HasReachedDoor == true && GameManager.instance.IsDoorOpen == true)
        {
            winText.text = "Level Cleared!";
            winText.enabled = true;
            nextSceneText.enabled = true;
        }

        else if(GameManager.instance.IsDead)
        {
            loseText.text = "Level Failed";
            loseText.enabled = true;
            retryText.enabled = true;
            quitText.enabled = true;
        }

        if (GameManager.instance.TotalTime <= 0)
            GameManager.instance.TimeUp = true;
        else
            GameManager.instance.TimeUp = false;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
