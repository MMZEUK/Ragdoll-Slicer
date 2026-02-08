using UnityEngine;
using System.Collections;

public class ThrowableWeapon : MonoBehaviour
{
    public float damage = 25f;
    public float spinForce = 20f;
    public float rotationOffset = 0f;

    private Rigidbody rb;
    private bool hasHit = false;
    private bool hasStuck = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction, float force)
    {
        hasHit = false;
        hasStuck = false;

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(direction * force, ForceMode.Impulse);
        rb.AddTorque(transform.right * spinForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasStuck) return;

        // Damage logic (safe null check)
        if (collision.gameObject.layer == 7)
        {
            ConfigurableJoint joint = collision.gameObject.GetComponent<ConfigurableJoint>();
            if (joint != null && joint.angularXDrive.positionSpring > 15)
            {
                LimbHealth limb = collision.collider.GetComponent<LimbHealth>();
                if (limb != null)
                    limb.TakeDamage(damage);
            }
        }

        // Ignore player
        if (collision.gameObject.CompareTag("Player"))
            return;

        StickIntoTarget(collision);
    }

    void StickIntoTarget(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            hasStuck = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;

            ContactPoint contact = collision.contacts[0];
            transform.position = contact.point;

            // Set rotation directly
            Quaternion targetRot = Quaternion.LookRotation(-contact.normal) * Quaternion.Euler(rotationOffset, 0f, 0f);
            transform.rotation = targetRot;

            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.rigidbody;

            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            Destroy(gameObject, 20f);
        }else
        {
            Destroy(gameObject, 5f);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    
    }

}