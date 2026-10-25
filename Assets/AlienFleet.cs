using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class AlienFleet : MonoBehaviour
{
    [Header("Inscribed")]

    public GameObject alienPrefab;

    public AlienCounter alienCounter;

    public List<GameObject> aliens = new List<GameObject> { };

    public Transform spaceship;          // drag the Spaceship in from the scene
    public GameObject gameOverPanel;     // drag GameOverPanel in from the Canvas
    public GameObject youWinPanel;       // drag YouWinPanel in from the Canvas

    float speed = .1f;
    float baseSpeed = .1f;   // speed with a full fleet
    float maxSpeed = .3f;    // speed when one alien is left
    int startCount;          // how many aliens we started with

    float leftBound = -16;
    float rightBound = 16;
    float lowerBound = 4.8f;
    float downTime = .1f;
    bool movingDown = false;
    bool gameOver = false;

    void Start()
    {
        Time.timeScale = 1f;

        // Bounds come from what the camera can actually see (minus a margin)
        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        rightBound = halfWidth - 1f;
        leftBound = -rightBound;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject alien = Instantiate<GameObject>(alienPrefab);
                alien.transform.position = new Vector3(2 * i - 12, 10 - 2 * j, 0);
                aliens.Add(alien);
                alien.GetComponent<Alien>().fleet = this;
            }
        }

        startCount = aliens.Count;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (youWinPanel != null) youWinPanel.SetActive(false);
    }

    void Update()
    {
        alienCounter.score = aliens.Count;
    }

    void FixedUpdate()
    {
        if (gameOver) return;

        if (aliens.Count == 0)
        {
            YouWin();
            return;
        }

        // Fewer aliens left = faster fleet. Keep the current direction (the sign of speed).
        float fractionKilled = 1f - (float)aliens.Count / startCount;
        speed = Mathf.Sign(speed) * Mathf.Lerp(baseSpeed, maxSpeed, fractionKilled);

        //move all aliens
        foreach (GameObject alien in aliens)
        {
            Vector3 position = alien.transform.position;
            if (!movingDown)
            {
                position.x += speed;
            }
            else
            {
                position.y += -Mathf.Abs(speed);
            }
            alien.transform.position = position;
        }

        //change alien direction
        float left = 0, right = 0;
        foreach (GameObject alien in aliens)
        {
            Vector3 position = alien.transform.position;
            if (position.x > right)
            {
                right = position.x;
            }
            if (position.x < left)
            {
                left = position.x;
            }
        }
        if (speed > 0 && right >= rightBound || speed < 0 && left <= leftBound)
        {
            speed = -speed;
            movingDown = true;
            Invoke("startSidewaysMotion", downTime);
        }

        CheckCantWin();
    }

    void startSidewaysMotion()
    {
        movingDown = false;
    }

    // The ship only shoots upward, so once any alien is at or below the ship's
    // height, no dart can ever reach it and the game can't be won.
    void CheckCantWin()
    {
        if (spaceship == null) return;

        foreach (GameObject alien in aliens)
        {
            if (alien.transform.position.y <= spaceship.position.y)
            {
                GameOver("The aliens got past your ship - you can't win anymore!");
                return;
            }
        }
    }

    public void GameOver(string reason)
    {
        if (gameOver) return;
        gameOver = true;
        Debug.Log("GAME OVER: " + reason);
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    void YouWin()
    {
        if (gameOver) return;
        gameOver = true;
        Debug.Log("YOU WIN!");
        Time.timeScale = 0f;
        if (youWinPanel != null) youWinPanel.SetActive(true);
    }
}