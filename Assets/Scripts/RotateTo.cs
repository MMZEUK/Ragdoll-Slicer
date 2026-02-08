using Unity.Mathematics;
using UnityEngine;

public class RotateTo : MonoBehaviour
{
    public Transform target;
    public ConfigurableJoint hipJoint;

    void Start()
    {
        JointDrive drive = hipJoint.slerpDrive;
        drive.positionSpring = 30000f;
        drive.positionDamper = 30000f;
        drive.maximumForce = Mathf.Infinity;
        hipJoint.slerpDrive = drive;
    }

    void FixedUpdate()
    {
        if (!target || !hipJoint) return;

        // direction to target
        Vector3 dir = target.position - hipJoint.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion lookRotation = Quaternion.LookRotation(dir);

        // keep upright (no tilt)
        float y = lookRotation.eulerAngles.y;
        Quaternion upright = Quaternion.Euler(0f, y, 0f);

        // ConfigurableJoint uses inverse rotation
        hipJoint.targetRotation = Quaternion.Inverse(upright);
    }
}
