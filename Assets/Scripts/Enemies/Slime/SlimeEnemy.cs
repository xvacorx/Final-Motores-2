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
        if (player != null)
        {
            if (angleSwitch.isLateralView)
            {
                LookAtPlayerLateral();

                if (Mathf.Abs(transform.position.x - player.position.x) == 0f)
                {

                    if (IsPlayerInRangeAndInFrontLateral())
                    {
                        if (canFire)
                        {
                            StartCoroutine(FireProjectile());
                        }
                    }
                }
            }
            else
            {
                LookAtPlayerFrontal();
                if (Mathf.Abs(transform.position.z - player.position.z) == 0f)
                    if (IsPlayerInRangeAndInFrontFrontal())
                    {
                        if (canFire)
                        {
                            StartCoroutine(FireProjectile());
                        }
                    }
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

    bool IsPlayerInRangeAndInFrontLateral()
    {
        float distance = Mathf.Abs(transform.position.z - player.position.z);
        bool isPlayerInRange = distance <= detectionRange;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        float dotProduct = Vector3.Dot(-transform.right, directionToPlayer.normalized);

        return isPlayerInRange && dotProduct > 0.5f;
    }

    bool IsPlayerInRangeAndInFrontFrontal()
    {
        float distance = Mathf.Abs(transform.position.x - player.position.x);
        bool isPlayerInRange = distance <= detectionRange;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        float dotProduct = Vector3.Dot(-transform.right, directionToPlayer.normalized);

        return isPlayerInRange && dotProduct > 0.5f;
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