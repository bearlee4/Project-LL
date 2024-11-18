using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform
    public Vector2 minBounds; // 카메라가 이동할 수 있는 최소 x, y 좌표
    public Vector2 maxBounds; // 카메라가 이동할 수 있는 최대 x, y 좌표

    private Vector3 offset;   // 카메라와 플레이어 사이의 초기 오프셋


    // Start is called before the first frame update
    void Start()
    {
        // 초기 오프셋 계산
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 플레이어 위치에 오프셋을 더해 새로운 카메라 위치 계산
        Vector3 desiredPosition = player.position + offset;

        // 카메라 위치를 최소 및 최대 범위 내로 제한
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        // z 축은 원래 위치 유지
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
