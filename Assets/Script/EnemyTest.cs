using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class EnemyTest : MonoBehaviour
{
    public float speed = 2f;
    public float returnSpeed = 7f;
    public float delay = 2f;

    private float currentSpeed;

    CircleCollider2D CircleCollider2D;
    Rigidbody2D rb2d;
    Coroutine moveCoroutine;
    Transform targetTransform = null;
    Vector3 endPosition;
    Vector3 spawnPosition;

    private float spawnArea = 15f;
    public float currentAngle = 0;

    public bool followTarget = true;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
        currentSpeed = speed;

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

        currentAngle += UnityEngine.Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
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

    private IEnumerator ReturnSpawnPos(Rigidbody2D rb)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        endPosition = Vector2.zero;
        followTarget = false;
        targetTransform = null;
        endPosition = spawnPosition;
        currentSpeed = returnSpeed;

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
        currentSpeed = speed;

        moveCoroutine = StartCoroutine(NormalMove());
    }

    void OnTriggerEnter2D(Collider2D collision)     // 범위에 플레이어 들어감
    {
        if (collision.gameObject.CompareTag("Player") && followTarget)
        {
            targetTransform = collision.gameObject.transform;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, speed));
        }
    }

    private void OnDrawGizmos()     // 범위표시
    {
        if (CircleCollider2D != null)
        {
            Gizmos.DrawWireSphere(transform.position, CircleCollider2D.radius);
        }
    }
}
