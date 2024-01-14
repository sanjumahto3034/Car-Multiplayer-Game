using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon weapon;


    private void Update()
    {
        //true
        if (Input.GetKeyDown(KeyCode.Mouse0))
            weapon.Shoot(true);

        //false
        if (Input.GetKeyUp(KeyCode.Mouse0))
            weapon.Shoot(false);
    }
}
