using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[System.Serializable]
public class OnDying : UnityEvent<EnemyController>
{
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float healthMax = 3f;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private string walkAnimationName = "Walk";
    private float health;
    public Status status;
    public Transform player;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private List<Rigidbody> ragDoll = new List<Rigidbody>();
    [SerializeField]private Transform healthBar;


    public OnDying onDying;

    private void Awake()
    {
        var _dall = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody _rb in _dall)
        {
            ragDoll.Add(_rb);
        }

        Disable_ragdoll();
    }

    private void Start()
    {
        status = Status.Idle;
        player = FindObjectOfType<PlayerController>().transform;
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        health = healthMax;
    }

    private void Update()
    {
        switch (status)
        {
            case Status.Attack:
                if (canAttack)
                {
                    Move_to_player();
                }
                break;
        }
    }

    private void Move_to_player()
    {
        navMeshAgent.SetDestination(player.position);
    }

    private void Get_damage(float _value)
    {
        health -= _value;
        Upload_healthbar();
        if (health <= 0)
        {
            status = Status.Idle;
            onDying.Invoke(this);
            Enable_ragdoll();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<Bullet>(out Bullet _bullet))
        {
            Get_damage(_bullet.damageValue);
            Destroy(_bullet.gameObject);
        }
    }

    private void Disable_ragdoll()
    {
        foreach (Rigidbody _rb in ragDoll)
        {
            _rb.gameObject.active = false;
        }
    }

    private void Enable_ragdoll()
    {
        GetComponent<BoxCollider>().enabled = false;
        navMeshAgent.enabled = false;
        animator.enabled = false;

        foreach (Rigidbody _rb in ragDoll)
        {
            _rb.gameObject.active = true;
        }
    }

    private void Upload_healthbar()
    {
        float _value = health / healthMax;
        Vector3 _scale = new Vector3(_value, 1,1);
        healthBar.localScale = _scale;
    }

    public void Change_status(Status _status)
    {
        status = _status;
        if (_status == Status.Attack && canAttack)
        {
            animator.SetBool(walkAnimationName, true);
        }
    }
}
