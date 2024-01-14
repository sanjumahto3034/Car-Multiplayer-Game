using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform bulletsParent;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField]
    private Transform bullet
    {
        get
        {
            if (bulletsParent.GetChild(0) == null)
            {
                Debug.LogWarning("No Bullet Available");
                return null;
            }
            return bulletsParent.GetChild(0);
        }
    }
    [Header("Gun Properties")]
    [SerializeField] private float BulletFireInSecond;
    private float RateOfFire;
    private float remainingTimeToNextShoot;
    private void Awake()
    {
        RateOfFire = BulletFireInSecond / bulletsParent.childCount;
    }


    private bool IsShooting;
    public void Shoot(bool IsPressed)
    {
        IsShooting = IsPressed;
        if (!IsPressed)
        {
            remainingTimeToNextShoot = 0;
        }
    }
    private void Update()
    {
        if (!IsShooting)
            return;
        if (remainingTimeToNextShoot <= 0)
        {
            Fire();
            remainingTimeToNextShoot = RateOfFire;
        }
        remainingTimeToNextShoot -= Time.deltaTime;

    }
    void Fire()
    {
        GameObject bullet_object = InitilizeBullet(bullet.gameObject);

        Action<Vector3> hitPoint = (Vector3 end) =>
        {
            Vector3 start = bulletSpawnPoint.position;
            Vector3 direction = (end - start).normalized;
            bullet_object.GetComponent<Rigidbody>().velocity = direction * 150f;
            DeInitilizeBullet(bullet_object);
        };
        Action<RaycastHit> rayCastHit = (RaycastHit hitInfo) =>
        {
            Debug.Log("Hit Object Name [ " + hitInfo.transform.gameObject.name + " ]");
        };
        FPS_LocalGameManager.GetHitPoint(rayCastHit, hitPoint);
    }
    void DeInitilizeBullet(GameObject _bullet) => StartCoroutine(DeInitilizeAfterDelay(_bullet));



    IEnumerator DeInitilizeAfterDelay(GameObject _bullet)
    {
        yield return new WaitForSeconds(3f);
        Destroy(_bullet.GetComponent<Rigidbody>());
        _bullet.transform.localScale = Vector3.zero;
        _bullet.transform.parent = bulletsParent;
        _bullet.transform.localPosition = Vector3.zero;
        _bullet.transform.localEulerAngles = Vector3.zero;
    }

    GameObject InitilizeBullet(GameObject _bullet)
    {
        _bullet.transform.localScale = Vector3.one;
        _bullet.AddComponent<Rigidbody>();
        _bullet.transform.parent = null;
        _bullet.transform.position = bulletSpawnPoint.position;

        return _bullet;
    }

}
