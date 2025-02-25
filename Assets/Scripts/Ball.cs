using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private int lives = 3; // Ball starts with 3 lives
    [SerializeField] private Vector3 spawnPosition; // Original spawn position
    [SerializeField] private AudioSource audioSource; // Reference to AudioSource
    [SerializeField] private AudioClip hitSound; // Sound when ball hits a point
    [SerializeField] private AudioClip gameOverSound; // Sound when final life is lost

    private void Start()
    {
        spawnPosition = transform.position; // Store the initial spawn position
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "Dead":
                LoseLife();
                break;

            case "Bouncer":
                GameManager.instance.UpdateScore(10, 1);
                PlayHitSound();
                break;

            case "Point":
                GameManager.instance.UpdateScore(20, 1);
                PlayHitSound();
                break;

            case "Side":
                GameManager.instance.UpdateScore(10, 0);
                PlayHitSound();
                break;

            case "Flipper":
                GameManager.instance.multiplier = 1;
                break;

            default:
                break;
        }
    }

    private void LoseLife()
    {
        lives--;

        if (lives > 0)
        {
            // Respawn the ball if lives remain
            Respawn();
        }
        else
        {
            // Final life lost - Play game over sound and end the game
            if (audioSource != null && gameOverSound != null)
            {
                audioSource.PlayOneShot(gameOverSound);
            }
            GameManager.instance.GameEnd();
        }
    }

    private void Respawn()
    {
        transform.position = spawnPosition; // Move ball to initial position
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Reset velocity
    }

    private void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}
