using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MaceController : MonoBehaviour
{
    private enum State
    {
        Slam,
        Rest,
        Reset
    }

    [SerializeField, Tooltip("Mace downwards speed")]
    private float downwardSpeed;
    [SerializeField, Tooltip("Mace upwards speed")] 
    private float upwardSpeed;
    [SerializeField, Tooltip("Slam cooldown")]
    private float cooldown;

    private Vector3 restingPosition;
    private State state;
    
    private void Start()
    {
        restingPosition = transform.position;
        state = State.Slam;
    }

    
    private void Update()
    {
        switch (state)
        {
            case State.Slam:
            {
                float step = downwardSpeed * Time.deltaTime;
                transform.position += (Vector3.down * step);
                break;
            }
            case State.Reset:
            {
                HandleReset();
                break;
            }
            case State.Rest:
            default:
            {
                break;
            }
        }
    }

    private void HandleReset()
    {
        float step = upwardSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, restingPosition, step);
        if (transform.position != restingPosition)
        {
            return;
        }

        state = State.Rest;
        _ = StartCoroutine(HandleSlamCooldown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            state = State.Reset;
        }
    }

    private IEnumerator HandleSlamCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        state = State.Slam;
    }
    
    
}

