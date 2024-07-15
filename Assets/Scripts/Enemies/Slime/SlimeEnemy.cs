using System.Collections;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    public Transform firePoint;
    public float rayDistance = 10f;
    public GameObject projectilePrefab;
    public float fireRate = 3f; 

    Transform player;
    AngleSwitch angleSwitch;
    private bool canFire = true;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        angleSwitch = GetComponent<AngleSwitch>();
        StartCoroutine(FireProjectile());
    }

    void Update()
    {
        Vector2 direction = Vector2.left;
        if (angleSwitch.isLateralView)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.left;
        }

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, rayDistance);

        Debug.Log("Raycast Hit: " + hit.collider.tag);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (canFire)
            {
                StartCoroutine(FireProjectile());
            }
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

    IEnumerator FireProjectile()
    {
        canFire = false;
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
