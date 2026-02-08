using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponThrower : MonoBehaviour
{
    public List<GameObject> Weapons = new List<GameObject>();
    public Transform throwPoint;

    [Header("Charge Settings")]
    public float minThrowForce = 10f;
    public float maxThrowForce = 40f;
    public float chargeSpeed = 20f;

    [Header("UI")]
    public Slider chargeBar;

    private float currentForce;
    private bool isCharging;

    void Start()
    {
        if (chargeBar != null)
            chargeBar.value = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCharging();
        }

        if (Input.GetMouseButton(0))
        {
            ChargeThrow();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseThrow();
        }
    }

    void StartCharging()
    {
        isCharging = true;
        currentForce = minThrowForce;
    }

    void ChargeThrow()
    {
        if (!isCharging) return;

        currentForce += chargeSpeed * Time.deltaTime;
        currentForce = Mathf.Clamp(currentForce, minThrowForce, maxThrowForce);

        // Update UI (0–1 range)
        if (chargeBar != null)
        {
            float normalized = (currentForce - minThrowForce) / (maxThrowForce - minThrowForce);
            chargeBar.value = normalized;
        }
    }

    void ReleaseThrow()
    {
        isCharging = false;

        GameObject randomItem = Weapons[Random.Range(0, Weapons.Count)];
        Debug.Log("Picked: " + randomItem.name);
        GameObject weapon = Instantiate(randomItem, throwPoint.position, throwPoint.rotation);
        ThrowableWeapon tw = weapon.GetComponent<ThrowableWeapon>();

        tw.Throw(throwPoint.forward, currentForce);

        if (chargeBar != null)
            chargeBar.value = 0;
    }
}
