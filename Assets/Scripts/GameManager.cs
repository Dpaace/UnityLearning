using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    public bool isCoopMode = false;

    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _pauseMenuPanel;

    private Animator _pauseAnimator;


    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true && isCoopMode==false)
        {
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.Q) && _isGameOver == true && isCoopMode == false)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true && isCoopMode == true)
        {
            SceneManager.LoadScene(3); 
        }

        if (Input.GetKeyDown(KeyCode.Q) && _isGameOver == true && isCoopMode == true)
        {
            SceneManager.LoadScene(3);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
        }

        if(_isGameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _spawnManager.StartSpawning();
            }
        }


    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
