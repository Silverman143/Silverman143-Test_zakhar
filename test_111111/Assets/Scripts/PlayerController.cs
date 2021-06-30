using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[System.Serializable]
public class OnPoint: UnityEvent<PointHandler>
{
}

public enum Status
{
    Walk, Attack, Idle
}

public class PlayerController : MonoBehaviour
{


    public float attackSpeed = 0.3f;
    private float stoppingDistance = 0.26f;

    private Camera mainCamera;
    private NavMeshAgent navMeshAgent;
    private Animator anim;
    private BulletsPool bulletsPool;
    public List<PointHandler> points;
    public OnPoint onPoint;
    public Status status;
    private string walkAnimName = "isWalking";
    private string shootAnimName = "Shoot";


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        mainCamera = FindObjectOfType<Camera>();
        bulletsPool = FindObjectOfType<BulletsPool>();

        foreach(PointHandler _point in points)
        {
            onPoint.AddListener(_point.Player_on_point);
        }
        onPoint.AddListener(FindObjectOfType<GameHandler>().Check_finish);
    }

    private void Update()
    {
        switch (status)
        {
            case Status.Idle:
                return;

            case Status.Attack:
                Attack();
                return;

            case Status.Walk:
                Move_to_target();
                return;
        }
    }

    private void Move_to_target()
    {
        float _distance = Vector3.Distance(transform.position, points[0].transform.position);
        if (_distance >= stoppingDistance)
        {
            navMeshAgent.SetDestination(points[0].transform.position);
        }
        else
        {
            onPoint.Invoke(points[0]);
            transform.LookAt(points[0].target);
            Change_status(Status.Attack);
            points.Remove(points[0]);
        }
    }

    private void Attack()
    {
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            RaycastHit _hit;
            switch (_touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(mainCamera.ScreenPointToRay(_touch.position), out _hit))
                    {
                        Shoot(_hit.point);
                    }
                    return;
            }
        }
    }

    private void Shoot(Vector3 _target)
    {
        anim.SetTrigger(shootAnimName);
        bulletsPool.Get_bullet(_target);
    }

    public void Change_status(Status _status)
    {
        status = _status;
        if (_status == Status.Attack)
        {
            if (anim.GetBool(walkAnimName))
            {
                anim.SetBool(walkAnimName, false);
            }
        }
        else if (_status == Status.Walk)
        {
            Debug.Log("Activate");
            if (!anim.GetBool(walkAnimName))
            {
                anim.SetBool(walkAnimName, true);
            }
        }
        else if (_status == Status.Idle)
        {
            if (!anim.GetBool(walkAnimName))
            {
                anim.SetBool(walkAnimName, false);
            }
        }
    }
}
