using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ArcManager : MonoBehaviour
{
    public Transform player;
    //public float speed = 5f;
    public float distanceFromPlayer = 1.1f;
    public bool isActive = false;


    private void Start()
    {

    }

    void Update()
    {
        Arc();
    }

    public void Arc()
    {
        if (isActive == true)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 direction = mousePosition - player.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Vector3 newPosition = player.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * distanceFromPlayer;

            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else if (isActive == false)
        {
            return;
        }

    }
}
