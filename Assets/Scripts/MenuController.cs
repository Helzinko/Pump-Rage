using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMP_Text highScoreText;
    void Start()
    {
        var _highscoreTemp = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = "Highscore: <color=red>\n" + _highscoreTemp + "</color>";
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartProgress()
    {
        var _highscoreTemp = 0;
        PlayerPrefs.SetInt("highScore", 0);
        highScoreText.text = "Highscore: <color=red>\n" + _highscoreTemp + "</color>";
    }
}
