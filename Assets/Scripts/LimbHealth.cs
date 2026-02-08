using System.Linq;
using UnityEngine;

public class LimbHealth : MonoBehaviour
{
    public float health = 50f;
    public ConfigurableJoint joint;
    public GameObject breakEffect;

    public GameObject[] Limb;

    public EnemyHealth enemyHealth;

    public float damage;



    public void TakeDamage(float damage)
    {
        health -= damage;

        Instantiate(breakEffect, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            BreakLimb();
        }
    }

    void BreakLimb()
    {
        if (joint != null)
        {
            enemyHealth.TakeDamage(damage);

            joint.xMotion = ConfigurableJointMotion.Free;
            joint.yMotion = ConfigurableJointMotion.Free;
            joint.zMotion = ConfigurableJointMotion.Free;

            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Free;

                JointDrive xDrive = joint.angularXDrive;
                xDrive.positionSpring = 10f;
                joint.angularXDrive = xDrive;

                // Angular YZ
                JointDrive yzDrive = joint.angularYZDrive;
                yzDrive.positionSpring = 10f;
                joint.angularYZDrive = yzDrive;

        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        Destroy(this);

    }
}
