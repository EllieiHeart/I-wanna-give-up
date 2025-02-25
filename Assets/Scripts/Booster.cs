using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float boostForce = 20f; // Adjust as needed
    private AudioSource boostSound; // AudioSource for the boost sound

    private void Start()
    {
        boostSound = GetComponent<AudioSource>(); // Get AudioSource from Booster object
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // Ensure the ball has the "Ball" tag
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                ballRb.linearVelocity = Vector2.zero; // Reset velocity
                ballRb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse); // Launch upward

                if (boostSound != null)
                {
                    boostSound.Play(); // Play the boost sound
                }
            }
        }
    }
}
