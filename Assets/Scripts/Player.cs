using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public or private reference
    //data type(int, float, bool, string)
    //every variable has a name
    //optional value assigned

    [SerializeField]
    private float _speed = 3.5f;

    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

   
    
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;

    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;


    // Start is called before the first frame update
    void Start()
    {
        //take the current position = new position(0,0,0)
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();



        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerOne == true)
        {
            CalculateMovement();
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && isPlayerOne == true)
            {
                FireLaser();
            }
        }

        if(isPlayerTwo == true)
        {
            CalculateMovementPLayerTwo();
            if (Input.GetKeyDown(KeyCode.KeypadEnter) && Time.time > _canFire && isPlayerTwo == true)
            {
                FireLaser();
            }
        }
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(_speed * Time.deltaTime * direction);
        
        //if (transform.position.y >= 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y <= -3.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}
        // Same as above if else statement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void CalculateMovementPLayerTwo()
    {
        if(Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        //Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        //transform.Translate(_speed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Vector3 _laserOffset = new Vector3(0, 1.05f, 0);

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        if(_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _rightEngine.SetActive(true);
        }


        _uiManager.UpdateLives(_lives);
        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score); 
    }
}
