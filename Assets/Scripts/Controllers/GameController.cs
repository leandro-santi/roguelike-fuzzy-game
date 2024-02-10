using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("UI")] public GameObject gameUI;
    public GameObject playerUI;
    public GameObject pauseUI;
    public TextMeshProUGUI timerText;

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
        canPlayTheGame = true;
        _pause = false;
        _isPaused = false;
        _gameStarted = false;

        Instance = this;
    }

    void Update()
    {
        if (!_canCountTimer || !canPlayTheGame) return;

        if (_timer <= maxTimer)
        {
            _timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(_timer / 60);
            int remainingSeconds = Mathf.FloorToInt(_timer % 60);

            timerText.text = minutes.ToString("00") + ":" + remainingSeconds.ToString("00");
        }
        else
        {
            Debug.Log("Game Over!");

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

    public void FinishGame()
    {
        pauseUI.SetActive(true);

        canPlayTheGame = false;
        _canCountTimer = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}