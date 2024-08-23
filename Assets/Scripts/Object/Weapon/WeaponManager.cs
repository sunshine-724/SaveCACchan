using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    [SerializeField] List<GameObject> prefabWeaponObject; //プレハブの武器オブジェクトリスト(アクティブ状態の武器だけ使用可能)

    // Update is called once per frame
    void Update()
    {

    }


    //受け取った武器に変更する
    public void ChangeWeapon(WeaponType weaponType)
    {
        Debug.Log(weaponType + "に切り替えました");
        DoActive(weaponType);
        uiManager.ChangeDisplayWeapon(weaponType);
    }

    //指定された武器以外全て非アクティブにする
    private void DoActive(WeaponType weaponType)
    {
        foreach (GameObject obj in prefabWeaponObject)
        {
            Weapon weapon = obj.GetComponent<Weapon>();
            if (weapon != null && weapon.type == weaponType)
            {
                obj.SetActive(true);
                continue;
            }
            obj.SetActive(false);
        }
    }

    public void Attack(WeaponType type)
    {

        foreach (GameObject obj in prefabWeaponObject)
        {
            Weapon weapon = obj.GetComponent<Weapon>();
            if (weapon != null && weapon.type == type)
            {
                weapon.AttackGun();
                break;
            }
        }
    }
}
