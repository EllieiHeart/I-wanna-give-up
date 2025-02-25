using UnityEngine;
using System.Collections;

public class SpeedZone : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 2f;  // Increase speed 2x
    [SerializeField] private float slowMultiplier = 0.5f; // Reduce speed to 50%
    [SerializeField] private float effectDuration = 1f;   // Duration of the effect (1 second)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = other.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                StartCoroutine(ApplySpeedEffect(ballRb));
            }
        }
    }

    private IEnumerator ApplySpeedEffect(Rigidbody2D ballRb)
    {
        // Speed up the ball
        ballRb.linearVelocity *= speedMultiplier;
        ballRb.angularVelocity *= speedMultiplier;

        // Wait for 1 second
        yield return new WaitForSeconds(effectDuration);

        // Slow down the ball
        ballRb.linearVelocity *= slowMultiplier;
        ballRb.angularVelocity *= slowMultiplier;
    }
}
