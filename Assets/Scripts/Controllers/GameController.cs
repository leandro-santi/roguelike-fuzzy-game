using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("UI")] public GameObject gameUI;
    public GameObject playerUI;
    public GameObject pauseUI;

    public static GameController Instance;

    public bool canPlayTheGame;
    public float maxTimer;

    private float _timer;
    private bool _canCountTimer;
    private bool _pause;
    private bool _isPaused;
    private bool _gameStarted;

    void Start()
    {
        _timer = 0f;

        _canCountTimer = true;
        canPlayTheGame = false;
        _pause = false;
        _isPaused = false;
        _gameStarted = false;

        Instance = this;
    }

    void Update()
    {
        if (!_gameStarted) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _pause = !_pause;
        }

        if (_pause)
        {
            if (!_isPaused) PauseGame();
        }
        else
        {
            if (_isPaused) ExitPauseGame();
        }

        if (!_canCountTimer || !canPlayTheGame) return;

        if (_timer <= maxTimer) _timer += Time.deltaTime;
        else
        {
            Debug.Log("Let's Go!");
            _canCountTimer = false;
            FinishGame();
        }
    }

    public void EnableGameStart()
    {
        gameUI.SetActive(false);
        playerUI.SetActive(true);

        canPlayTheGame = true;

        _gameStarted = true;
    }

    private void PauseGame()
    {
        playerUI.SetActive(false);
        pauseUI.SetActive(true);

        canPlayTheGame = false;
        _isPaused = true;
    }

    private void ExitPauseGame()
    {
        playerUI.SetActive(true);
        pauseUI.SetActive(false);

        canPlayTheGame = true;
        _isPaused = false;
    }

    private void FinishGame()
    {
        pauseUI.SetActive(true);

        canPlayTheGame = false;

        _gameStarted = false;
    }

    public void RestartGame()
    {
        Debug.Log("Was called");
        SceneManager.LoadScene("SampleScene");
    }
}