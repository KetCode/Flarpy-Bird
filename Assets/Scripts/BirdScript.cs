using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody2D;
    public float flapStrength; 
    public LogicScript logic;
    public bool birdIsAlive = true;
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            Jump();
        }
        
        GameOverWhenOffScreen();
    }

    void Jump()
    {
        myRigidbody2D.velocity = Vector2.up * flapStrength;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        logic.GameOver();
        birdIsAlive = false;
    }

    void GameOverWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if (birdIsAlive && (screenPosition.x > _camera.pixelWidth || screenPosition.y > _camera.pixelHeight ||
                            screenPosition.x < 0 || screenPosition.y < 0))
        {
            logic.GameOver();
            birdIsAlive = false;
        }
    }
}
