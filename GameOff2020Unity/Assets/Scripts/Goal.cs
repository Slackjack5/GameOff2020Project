using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private UnityEvent onEnter = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            onEnter.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
