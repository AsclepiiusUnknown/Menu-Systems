using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking
    }
    State currentState;

    public ParticleSystem deathEffect;
    public ParticleSystem decayEffect;
    public static event System.Action OnDeathStatic;

    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;
    //Spawner spawner;

    float attackDistanceThreshhold = .5f;
    readonly float timeBtwAttacks = 1;
    float damage = 1;
    Material skinMat;

    Color originalColor;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = GetComponent<CapsuleCollider>().radius;
        }
    }

    protected override void Start()
    {
        base.Start();
        
        if (hasTarget)
        {
            currentState = State.Chasing;
            targetEntity.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }
    }

    public void SetCharacteristics(float moveSpeed, int hitsToKill, float enemyHealth, Color skinColor)
    {
        pathFinder.speed = moveSpeed;

        if (hasTarget)
        {
            damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKill);
        }
        startingHealth = enemyHealth;
        var deathMain = deathEffect.main;
        deathMain.startColor = new Color(skinColor.r, skinColor.g, skinColor.b, 1);

        var decayMain = decayEffect.main;
        decayMain.startColor = new Color(skinColor.r, skinColor.g, skinColor.b, 1);

        skinMat = GetComponent<Renderer>().material;
        skinMat.color = skinColor;
        originalColor = skinMat.color;
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir)
    {
        base.TakeHit(damage, hitPoint, hitDir);

        AudioManager.instance.PlaySound("Impacts", transform.position);
        if (damage >= health)
        {
            OnDeathStatic?.Invoke();
            AudioManager.instance.PlaySound("Enemy Deaths", transform.position);
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDir)) as GameObject, 2);
        }
        Destroy(Instantiate(decayEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDir)) as GameObject, 2);
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    private void Update()
    {
        if (target != null)
        {
            if (hasTarget)
            {
                if (Time.time > nextAttackTime)
                {
                    float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                    if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshhold + myCollisionRadius + targetCollisionRadius, 2))
                    {
                        nextAttackTime = Time.time + timeBtwAttacks;
                        AudioManager.instance.PlaySound("Enemy Attacks", transform.position);
                        StartCoroutine(Attack());
                    }
                }
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 originalPos = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPos = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        bool hasAppliedDamage = false;

        while(percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                skinMat.color = Color.red;

                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPos, attackPos, interpolation);

            yield return null;
        }

        skinMat.color = originalColor;
        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;
        if (target != null)
        {
            while (hasTarget)
            {
                if (currentState == State.Chasing)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPos = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshhold / 2);
                    if (!dead)
                    {
                        pathFinder.SetDestination(targetPos);
                    }
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
