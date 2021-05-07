using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMP_Text highScoreText;
    
    public GameObject crosshair;
    private Camera _camera;
    private Plane _groundObject;
    
    void Start()
    {
        var soundtrack = GameObject.FindGameObjectWithTag("soundtrack");

        if (soundtrack != null)
            soundtrack.GetComponent<SoundtrackController>().StopMusic();
        
        var _highscoreTemp = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = "Highscore: <color=red>\n" + _highscoreTemp + "</color>";
        
        _camera = Camera.main;
        _groundObject = new Plane(Vector3.up, Vector3.up);
        Cursor.visible = false;
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

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!_groundObject.Raycast(ray, out var rayDistance)) return;
        
        var point = ray.GetPoint(rayDistance);
        var lookPoint = new Vector3(point.x, transform.position.y, point.z);
        
        transform.LookAt(lookPoint);
        
        crosshair.transform.position = point;
    }
}
