using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerScored;
    public event EventHandler OnPlayerHitObstacle;

    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private LayerMask _passLayer;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale != 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            _rigidbody.AddForce(transform.up * _jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer) == _obstacleLayer.value)
        {
            OnPlayerHitObstacle?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer) == _passLayer.value)
        {
            OnPlayerScored?.Invoke(this, EventArgs.Empty);
        }
    }

    public void GoToStartPosition()
    {
        transform.position = new Vector3(-5, 0, 0);
    }
}
