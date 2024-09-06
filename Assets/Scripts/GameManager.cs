using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private Transform _obstaclePrafab;
    [SerializeField] private RectTransform _loseScreen;
    [SerializeField] private TextMeshProUGUI _playerScoreVisual;
    [SerializeField] private float _gameSpeed;
    [SerializeField] private int _obstacleSpacingX = 4;
    [SerializeField] private int _obstacleSpacingY = 1;

    private Queue<Transform> _obstaclesQueue = new Queue<Transform>();
    private int _playerScore;
    private int _queueLength = 7;
    private int _halfMapSize;
    private float _obstacleReplacementTimer;
    private float _obstacleReplacementDelay;
    private float _obstacleReplacementFrequency;
    private System.Random _randomYSpacing;

    private void Start()
    {
        _randomYSpacing = new System.Random();

        _halfMapSize = (int)_ground.localScale.x / 2;
        _obstacleReplacementDelay = _halfMapSize / _gameSpeed;
        _obstacleReplacementFrequency = _obstacleSpacingX / _gameSpeed;

        //Creating obstacles
        for (int i = 0; i < _queueLength; i++)
        {
            _obstaclesQueue.Enqueue(Instantiate(_obstaclePrafab, transform));
        }

        StartNewGame();

        Player.Instance.OnPlayerScored += Player_OnPlayerScored;
        Player.Instance.OnPlayerHitObstacle += Player_OnPlayerHitObstacle;
    }

    private void Player_OnPlayerScored(object sender, System.EventArgs e)
    {
        _playerScore++;
        _playerScoreVisual.text = "Player score: " + _playerScore.ToString();
    }

    private void Player_OnPlayerHitObstacle(object sender, System.EventArgs e)
    {
        Time.timeScale = 0f;

        _loseScreen.gameObject.SetActive(true);
    }

    private void Update()
    {
        //Move obstacles forward player
        transform.Translate(Vector3.left  * _gameSpeed * Time.deltaTime);

        _obstacleReplacementTimer += Time.deltaTime;

        //Replacing first passed and already invisible obstacle from start at the end of the queue
        if (_obstacleReplacementTimer > _obstacleReplacementFrequency)
        {
            Transform t = _obstaclesQueue.Dequeue();
            _obstaclesQueue.Enqueue(t);

            t.position = new Vector3(t.position.x + (_obstacleSpacingX * _queueLength), _randomYSpacing.Next(-_obstacleSpacingY, _obstacleSpacingY), 0);

            _obstacleReplacementTimer = 0;
        }
    }

    public void StartNewGame()
    {
        _loseScreen.gameObject.SetActive(false);

        _playerScore = 0;
        _playerScoreVisual.text = "Player score: " + _playerScore.ToString();

        Time.timeScale = 1f;

        _obstacleReplacementTimer = -_obstacleReplacementDelay;

        transform.position = Vector3.zero;

        int spacing = 0;

        //Making space between obstacles
        foreach (Transform t in _obstaclesQueue)
        {
            t.position = new Vector3(spacing, _randomYSpacing.Next(-_obstacleSpacingY, _obstacleSpacingY), 0);
            spacing += _obstacleSpacingX;
        }

        //Place player at default position
        Player.Instance.GoToStartPosition();
    }
}
