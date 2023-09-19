using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuu : MonoBehaviour
{

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("Game Manager is Null");
        }
    }
    public void LoadSinglePlayerGame()
    {
        Debug.Log("Single Player Game Loading...");
        SceneManager.LoadScene(2);
    }

    public void LoadCoOpMode()
    {
        Debug.Log("Co-Op Mode Game Loading...");
        
        SceneManager.LoadScene(3);
    }
}
