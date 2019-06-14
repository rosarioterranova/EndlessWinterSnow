using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanDetectCollision : MonoBehaviour
{
    [SerializeField] Enemy m_Enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Enemy.ForceDetectCollision(collision);
    }
}
