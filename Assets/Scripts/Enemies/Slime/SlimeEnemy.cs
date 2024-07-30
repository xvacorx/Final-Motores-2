using System.Collections;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 3f;
    public float fireDelay = 0.1f;
    public float detectionRange = 10f; 

    Transform player;
    AngleSwitch angleSwitch;
    bool canFire = true;

    Animator animator;

    private void Start()
    {
        player = GameObject.Find("Player")?.transform;
        angleSwitch = GetComponent<AngleSwitch>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            if (canFire)
            {
                StartCoroutine(FireProjectile());
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
        animator.SetTrigger("shoot");
        canFire = false;
        yield return new WaitForSeconds(fireDelay);
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        yield return new WaitForSeconds(fireRate - fireDelay);
        canFire = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}