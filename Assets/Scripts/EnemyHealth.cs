using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;

    public ConfigurableJoint[] Joints;

    public ConfigurableJoint HipJiont;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
            Destroy(this.gameObject,5);

        }
    }

    private void Die()
    {
        if (Joints.Length > 0)
        {
            foreach (ConfigurableJoint go in Joints)
            {
                JointDrive xDrive = go.angularXDrive;
                xDrive.positionSpring = 15f;
                go.angularXDrive = xDrive;

                // Angular YZ
                JointDrive yzDrive = go.angularYZDrive;
                yzDrive.positionSpring = 15f;
                go.angularYZDrive = yzDrive;
            }
        }

        JointDrive hipxDrive = HipJiont.angularXDrive;
        hipxDrive.positionSpring = 0f;
        HipJiont.angularXDrive = hipxDrive;

        // Angular YZ
        JointDrive hipyzDrive = HipJiont.angularYZDrive;
        hipyzDrive.positionSpring = 0f;
        HipJiont.angularYZDrive = hipyzDrive;

        JointDrive drive = HipJiont.slerpDrive;
        drive.positionSpring = 0f;
        drive.positionDamper = 0f;
        drive.maximumForce = Mathf.Infinity;
        HipJiont.slerpDrive = drive;

        this.gameObject.GetComponent<RotateTo>().enabled = false;
        this.gameObject.GetComponent<Enemycontroller>().enabled = false;
        
    }
}
