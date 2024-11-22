using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class EnemyMovement : MonoBehaviour
{
    private EnemyStatus enemyStatus;

    
    public float delay = 2f;

    private float currentSpeed;

    CircleCollider2D CircleCollider2D;
    Rigidbody2D rb2d;
    Coroutine moveCoroutine;
    Transform targetTransform = null;
    Vector3 endPosition;
    Vector3 spawnPosition;

    private float spawnArea = 4f;
    public float currentAngle = 0;

    public bool followTarget = true;

    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();

        spawnPosition = transform.position;
        currentSpeed = enemyStatus.speed;

        rb2d = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();

        endPosition = transform.position;
        StartCoroutine(NormalMove());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);  // 이동경로 표시
        Debug.DrawLine(rb2d.position, spawnPosition, Color.green);
    }

    public IEnumerator NormalMove()
    {
        while (true)
        {
            NewPosition();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            yield return new WaitForSeconds(delay);
        }
    }


    private void NewPosition()  // 기본움직임
    {
        if(!followTarget) return;

        endPosition += Vector3FromAngle(currentAngle) * 0.2f;   // 이동할 위치
        currentAngle += UnityEngine.Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);

    }

    private Vector3 Vector3FromAngle(float angle)   // 각도계산
    {
        angle = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
    }

    private IEnumerator Move(Rigidbody2D rb, float sp)
    {
        float distance = (transform.position - endPosition).sqrMagnitude;

        while (distance > float.Epsilon)
        {
            float spawnDistance = (transform.position - spawnPosition).sqrMagnitude;

            if(spawnDistance > spawnArea*spawnArea)
            {
                StartCoroutine(ReturnSpawnPos(rb));
            }

            if (targetTransform != null)
            {
                endPosition = targetTransform.position;
            }

            if (rb != null)
            {
                Vector3 newPosition = Vector3.MoveTowards(rb.position, endPosition, currentSpeed * Time.deltaTime);

                rb.MovePosition(newPosition);
                distance = (rb.position - (Vector2)endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator ReturnSpawnPos(Rigidbody2D rb)  //스폰지점으로 복귀
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        endPosition = Vector2.zero; // 멈추고
        followTarget = false;       // 타겟해제
        targetTransform = null;
        endPosition = spawnPosition;    // 도착지점 = 스폰지점
        currentSpeed = enemyStatus.returnSpeed;     // 속도빠르게변경
        enemyStatus.invincible = true;  // 무적

        enemyStatus.currentHp = enemyStatus.maxHp;  // 체력회복
        CircleCollider2D.radius = 5f;       // 범위 정상화


        yield return new WaitForSeconds(1f);

        while ((rb.position - (Vector2)endPosition).sqrMagnitude > float.Epsilon)
        {
            if (rb != null)
            {
                Vector3 newPosition = Vector3.MoveTowards(rb.position, endPosition, currentSpeed * Time.deltaTime);
                rb.MovePosition(newPosition);
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForSeconds(1f);

        followTarget = true;
        currentSpeed = enemyStatus.speed;
        enemyStatus.invincible = false; // 무적해제
        moveCoroutine = StartCoroutine(NormalMove());
    }

    private void OnDrawGizmos()     // 인식범위표시
    {
        if (CircleCollider2D != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, CircleCollider2D.radius*0.1f);
            Gizmos.DrawWireSphere(spawnPosition, spawnArea);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)     // 범위에 플레이어 들어감
    {
        if (!targetTransform)
        {
            if (collision.gameObject.CompareTag("Player") && followTarget)
            {
                targetTransform = collision.gameObject.transform;
                CircleCollider2D.radius = 10f;

                MoveValue();
            }
        }
    }

    void MoveValue()
    {
        if (enemyStatus.EnemyType == 0)      // 슬라임 등 일반적인 움직임
        {
            FollowPlayer();
        }

        if (enemyStatus.EnemyType == 1)     // 멧돼지 등 돌진하는 유형
        {
            StartCoroutine(Rush());

        }
    }

    void FollowPlayer()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        
        moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
    }

    private IEnumerator Rush()
    {
        FollowPlayer();

        yield return new WaitForSeconds(2f);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        followTarget = false;
        currentSpeed = 0;

        yield return new WaitForSeconds(1.5f);

        Vector3 directionToPlayer = (targetTransform.position - transform.position).normalized;
        Vector3 rushDistance = transform.position + directionToPlayer * 1.2f;  // 돌진 거리

        currentSpeed = 2f;

        while (Vector3.Distance(rb2d.position, rushDistance) > 0.1f)
        {
            Vector3 RushPosition = Vector3.MoveTowards(rb2d.position, rushDistance, currentSpeed * Time.deltaTime);
            rb2d.MovePosition(RushPosition);
            yield return new WaitForFixedUpdate();
        }

        followTarget = true;
        currentSpeed = enemyStatus.speed;
        StartCoroutine(Rush());
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            followTarget = false;
            currentSpeed = 0f;

            StartCoroutine(ResumeMove());
        }

    }

    private IEnumerator ResumeMove()
    {
        yield return new WaitForSeconds(1f);

        followTarget = true;
        currentSpeed = enemyStatus.speed;

        MoveValue();
    }
}
