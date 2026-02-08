using UnityEngine;

public class Enemycontroller : MonoBehaviour
{
    public Transform player;
    public Rigidbody hips;
    public float moveForce = 40f;
    public float stopDistance = 2f;
    public float maxSpeed;

    void FixedUpdate()
    {
        if (!player) return;

        Vector3 direction = (player.position - hips.position).normalized;

        // Add force toward player
        hips.AddForce(direction * moveForce, ForceMode.Force);

        // Clamp max speed so it doesn't go crazy
        if (hips.linearVelocity.magnitude > maxSpeed)
        {
            hips.linearVelocity = hips.linearVelocity.normalized * maxSpeed;
        }
    }

}
