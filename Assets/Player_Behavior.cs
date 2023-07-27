using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{
    [SerializeField] float power = 10f;
    private Rigidbody2D rb;

    [SerializeField] Vector2 minPower;
    [SerializeField] Vector2 maxPower;

    Line_Trajectory lt;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector2 ForceVector;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lt = GetComponent<Line_Trajectory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15; 
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;
            lt.renderLine(startPoint, currentPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;
            ForceVector = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            rb.AddForce(ForceVector * power, ForceMode2D.Impulse);
            lt.endLine();
        }
    }
}
