using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomGoal : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private UnityEvent onExit = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            onExit.Invoke();
        }
    }
}
