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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lt = GetComponent<Line_Trajectory>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            setToMousePos(ref startPoint);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = new Vector3();
            setToMousePos(ref currentPoint);

            Vector3 midPoint = new Vector3((startPoint.x + rb.transform.position.x) / 2, (startPoint.y + rb.transform.position.y) / 2, 10f);
            Vector3 targetPoint = new Vector3(2 * midPoint.x - currentPoint.x, 2 * midPoint.y - currentPoint.y, 10f);

            lt.renderLine(targetPoint, rb.position);
        }

        if (Input.GetMouseButtonUp(0))
        {
            setToMousePos(ref endPoint);

            ForceVector = new Vector2(
                Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y)
                );

            rb.AddForce(ForceVector * power, ForceMode2D.Impulse);
            lt.endLine();
        }
       
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
            Vector2 PushVector = new Vector2(
                Mathf.Clamp(-1 * rb.velocity.x * 100, min.x, max.x),
                Mathf.Clamp(-1 * rb.velocity.y * 100, min.y, max.y)
                );
            rb.AddForce(PushVector, ForceMode2D.Impulse);
            Debug.Log(PushVector);
        }
    }

}
