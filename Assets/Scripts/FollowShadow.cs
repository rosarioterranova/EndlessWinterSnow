using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShadow : MonoBehaviour
{
    [SerializeField] Transform m_Player;

    void Update()
    {
        transform.position = new Vector2(m_Player.transform.position.x, transform.position.y);
    }
}
