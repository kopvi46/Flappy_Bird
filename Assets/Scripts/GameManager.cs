using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _obstaclePrafab;
    [SerializeField] private float _gameSpeed;

    private Queue<Transform> _obstaclesQueue = new Queue<Transform>();
    private int _queueLength = 7;
    private int _spacingX = 4;
    private int _spacingY = 1;
    private int _halfMapSize = 10;
    private float _obstacleReplacmentTimer;
    private float _obstacleReplacmentFrequency;
    System.Random _randomYSpacing;

    private void Start()
    {
        _randomYSpacing = new System.Random();

        _obstacleReplacmentTimer = -_halfMapSize / _gameSpeed;
        _obstacleReplacmentFrequency = _spacingX / _gameSpeed;

        int spacing = 0;

        for (int i = 0; i < _queueLength; i++)
        {
            Transform t;

            _obstaclesQueue.Enqueue(t = Instantiate(_obstaclePrafab, transform));

            t.position = new Vector3(spacing, _randomYSpacing.Next(-_spacingY, _spacingY), 0);
            spacing += _spacingX;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.left  * _gameSpeed * Time.deltaTime);

        _obstacleReplacmentTimer += Time.deltaTime;

        if (_obstacleReplacmentTimer > _obstacleReplacmentFrequency)
        {
            Transform t = _obstaclesQueue.Dequeue();
            _obstaclesQueue.Enqueue(t);

            t.position = new Vector3(t.position.x + (_spacingX * _queueLength), _randomYSpacing.Next(-_spacingY, _spacingY), 0);

            _obstacleReplacmentTimer = 0;
        }
    }
}
