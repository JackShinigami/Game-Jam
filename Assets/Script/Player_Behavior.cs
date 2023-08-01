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
    private Vector3 targetPoint;

    private int jumpCount = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lt = GetComponent<Line_Trajectory>();
    }

    // Update is called once per frame
    private void Update()
    {
        handleMovement();
        if (isStopped())
            jumpCount = 0;
       
    }
    protected virtual void setToMousePos(ref Vector3 pos)
    {
        pos = Input_Manager.Instance.MouseWorldPos;
        pos.z = 10;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 max = new Vector2(100, 100);
        Vector2 min = new Vector2(-100, -100);
       
        if(collision.gameObject.CompareTag("Suriken"))
        {
            addForceToPlayer(rb.velocity * -1 * 100, min, max, 1);
        }
        else if(collision.gameObject.CompareTag("Jumper"))
        {
            addForceToPlayer(Vector2.up * 50, min, max, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 max = new Vector2(100, 100);
        Vector2 min = new Vector2(-100, -100);
        if (collision.gameObject.CompareTag("Fan"))
        {
            addForceToPlayer(collision.gameObject.transform.up, min, max, 2);
        }
    }
    private void handleMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            setToMousePos(ref startPoint);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = new Vector3();
            setToMousePos(ref currentPoint);

            Vector3 midPoint = new Vector3((startPoint.x + rb.transform.position.x) / 2, (startPoint.y + rb.transform.position.y) / 2, 10f);
            targetPoint = new Vector3(2 * midPoint.x - currentPoint.x, 2 * midPoint.y - currentPoint.y, 10f);

            if(jumpCount < 2)
                lt.renderLine(targetPoint, rb.position);
        }

        if (Input.GetMouseButtonUp(0))
        {

            if(jumpCount <= 1)
                addForceToPlayer(targetPoint -  new Vector3(rb.position.x, rb.position.y, 10), minPower, maxPower, power);

            lt.endLine();
            jumpCount++;
        }
    }    
    private void addForceToPlayer(Vector2 vector, Vector2 min, Vector2 max, float power)
    {
        Vector2 ForceVector = new Vector2(
                Mathf.Clamp(vector.x, min.x, max.x),
                Mathf.Clamp(vector.y , min.y, max.y)
            );
        rb.AddForce(ForceVector * power, ForceMode2D.Impulse);
    }

    private bool isStopped()
    {
        return rb.velocity == Vector2.zero;
    }
}
