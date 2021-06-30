using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    private List<GameObject> bullets = new List<GameObject>();

    public void Get_bullet(Vector3 _target)
    {
        GameObject _bullet = Searching_for_bullet();
        if (_bullet != null)
        {
            _bullet.GetComponent<Bullet>().target = _target;
            _bullet.transform.position = transform.position;
            _bullet.SetActive(true);
        }
        else
        {
            _bullet = Instantiate(Resources.Load<GameObject>("Bullet"), transform.position, Quaternion.identity, transform);
            _bullet.GetComponent<Bullet>().target = _target;
        }
    }

    private GameObject Searching_for_bullet()
    {
        GameObject _bullet = null;
        for(int i =0; i < bullets.Count; i++)
        {
            if (bullets[i].activeInHierarchy == false)
            {
                _bullet = bullets[i];
                return _bullet;
            }
        }
        return _bullet;
    }
}
