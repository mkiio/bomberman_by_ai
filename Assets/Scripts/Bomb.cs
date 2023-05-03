using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float countdownTime = 2.0f; // The amount of time before the bomb detonates
    public float explosionRadius = 2.0f; // The radius of the bomb's explosion
    public LayerMask explosionLayers; // The layers of game objects that can be damaged by the bomb's explosion

    private float countdownTimer; // The current countdown timer value
    private bool hasExploded; // Whether the bomb has already exploded

    void Start()
    {
        countdownTimer = countdownTime;
        hasExploded = false;
    }

    void Update()
    {
        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayers);

        foreach (Collider collider in colliders)
        {
            // Check if there's line of sight between the bomb and the collider
            if (HasLineOfSight(collider.gameObject))
            {
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.Damage();
                }
            }
        }

        Destroy(gameObject);
    }

    bool HasLineOfSight(GameObject target)
    {
        RaycastHit hit;
        Vector3 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            if (hit.collider.gameObject == target)
            {
                return true;
            }
        }

        return false;
    }
}
