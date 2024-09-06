using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _obstaclePrafab;
    [SerializeField] private RectTransform _loseScreen;
    [SerializeField] private TextMeshProUGUI _playerScoreVisual;
    [SerializeField] private float _gameSpeed;
    [SerializeField] private int _obstacleSpacingX = 4;
    [SerializeField] private int _obstacleSpacingY = 1;

    private Queue<Transform> _obstaclesQueue = new Queue<Transform>();
    private int _playerScore;
    private int _queueLength = 7;
    private int _halfMapSize = 10;
    private float _obstacleReplacmentTimer;
    private float _obstacleReplacmentFrequency;
    private System.Random _randomYSpacing;

    private void Start()
    {
        _randomYSpacing = new System.Random();

        _obstacleReplacmentFrequency = _obstacleSpacingX / _gameSpeed;

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
        transform.Translate(Vector3.left  * _gameSpeed * Time.deltaTime);

        _obstacleReplacmentTimer += Time.deltaTime;

        if (_obstacleReplacmentTimer > _obstacleReplacmentFrequency)
        {
            Transform t = _obstaclesQueue.Dequeue();
            _obstaclesQueue.Enqueue(t);

            t.position = new Vector3(t.position.x + (_obstacleSpacingX * _queueLength), _randomYSpacing.Next(-_obstacleSpacingY, _obstacleSpacingY), 0);

            _obstacleReplacmentTimer = 0;
        }
    }

    public void StartNewGame()
    {
        _loseScreen.gameObject.SetActive(false);

        _playerScore = 0;
        _playerScoreVisual.text = "Player score: " + _playerScore.ToString();

        Time.timeScale = 1f;

        _obstacleReplacmentTimer = -_halfMapSize / _gameSpeed;

        transform.position = Vector3.zero;

        int spacing = 0;

        foreach (Transform t in _obstaclesQueue)
        {
            t.position = new Vector3(spacing, _randomYSpacing.Next(-_obstacleSpacingY, _obstacleSpacingY), 0);
            spacing += _obstacleSpacingX;
        }

        Player.Instance.GoToStartPosition();
    }
}
