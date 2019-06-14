using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float m_Speed = 5;

    void Start()
    {
        Destroy(gameObject, 5);    
    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * m_Speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            GameManager.Instance.OnEnemyHitPlayer();
            if(GetComponent<Collider2D>())
            {
                GetComponent<Collider2D>().enabled = false;
            }
            else
            {
                GetComponentInChildren<Collider2D>().enabled = false;
            }
        }
    }

    public void ForceDetectCollision(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }
}
