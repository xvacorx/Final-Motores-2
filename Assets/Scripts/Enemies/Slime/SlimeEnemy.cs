using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    public Transform firePoint;
    public float rayDistance = 10f;

    Transform player;
    AngleSwitch angleSwitch;
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        angleSwitch = GetComponent<AngleSwitch>();
    }
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector2.left, rayDistance);
        if (hit.collider.CompareTag("Player"))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }

        if (angleSwitch.isLateralView)
        {
            LookAtPlayerLateral();
        }
        else
        {
            LookAtPlayerFrontal();
        }
    }

    void LookAtPlayerFrontal()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction.x >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void LookAtPlayerLateral()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction.z >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePoint.position, firePoint.position + Vector3.right * rayDistance);
        }
    }
}