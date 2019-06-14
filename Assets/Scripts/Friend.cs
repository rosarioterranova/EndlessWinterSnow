using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : Enemy
{
    [SerializeField] List<Sprite> Friends = new List<Sprite>();

    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = Friends[Random.Range(0, Friends.Count)];
        Destroy(gameObject, 5);
    }
}
