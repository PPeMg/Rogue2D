using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private float movementSpeed;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        movementSpeed = 1f / moveTime;
    }

    protected IEnumerator SmoothMovement(Vector2 end)
    {
        float remainingDinstance = Vector2.Distance(rigidBody.position, end);

        while (remainingDinstance > float.Epsilon)
        {
            Vector2 newPosition = Vector2.MoveTowards(rigidBody.position, end, movementSpeed*Time.deltaTime);
            rigidBody.MovePosition(newPosition);
            remainingDinstance = Vector2.Distance(rigidBody.position, end);
            yield return null;
        }
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        bool returnVal = false;

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;

        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            returnVal = true;
        }
        return returnVal;
    }

    protected abstract void OnMovementFail(GameObject obstacle);

    protected virtual void AttempMove(int xDir, int yDir)
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (!canMove)
        {
            OnMovementFail(hit.transform.gameObject);
        }
    }
}