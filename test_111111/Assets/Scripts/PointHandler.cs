using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointHandler : MonoBehaviour
{
    public Transform target;
    public UnityEvent enemiesEnded = new UnityEvent();
    private List<EnemyController> enemies = new List<EnemyController>();
    

    private void Start()
    {
        Transform _npcs = transform.GetChild(0);
        foreach (Transform _child in _npcs)
        {
            if (_child.TryGetComponent<EnemyController>(out EnemyController _enemy))
            {
                _enemy.onDying.AddListener(Remove_enemy);
                enemies.Add(_enemy);
            }
        }
        target = transform.Find("NPCs");
    }

    public void Remove_enemy(EnemyController _enemy)
    {
        enemies.Remove(_enemy);
        if (enemies.Count==0)
        {
            enemiesEnded.Invoke();
        }
    }

    private void Activate_enemies()
    {
        foreach(EnemyController _enemy in enemies)
        {
            _enemy.Change_status(Status.Attack);
        }
    }

    public void Player_on_point(PointHandler _point)
    {
        if (this == _point)
        {
            Activate_enemies();
        }
    }

}
