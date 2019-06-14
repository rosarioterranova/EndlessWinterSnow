using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            GameManager.Instance.OnStarHitPlayer();
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
}
