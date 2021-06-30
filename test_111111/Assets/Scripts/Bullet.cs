using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 4;
    [SerializeField] private float lifetime = 2f;
    private float timer;
    public Vector3 target = Vector3.zero;
    public float damageValue = 1f;

    private void Awake()
    {
        timer = lifetime;
    }
    private void Update()
    {
        timer -= 0.1f;
        if (target!= Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
        }
        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
