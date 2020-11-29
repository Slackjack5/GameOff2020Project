using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Punisher : AggressiveEnemy
{
    [SerializeField] private float shootDelayTime = 0.1f;
    [SerializeField] private UnityEvent onHit = new UnityEvent();

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            OnHit();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        onHit.Invoke();
        StartCoroutine(ShootDelay());
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootDelayTime);
        Shoot();
    }
}
