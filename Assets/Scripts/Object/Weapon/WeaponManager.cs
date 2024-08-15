using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    [SerializeField] List<GameObject> prefabWeaponObject; //プレハブの武器オブジェクトリスト

    // Update is called once per frame
    void Update()
    {
        
    }

    //受け取った武器に応じて攻撃する
    void Attack(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.morningstar:
                break;
            case WeaponType.yari:
                break;
            case WeaponType.kunai:
                break;
        }
    }

    //受け取った武器に変更する
    public void ChangeWeapon(WeaponType weaponType)
    {
        Debug.Log(weaponType);
        DoActive(weaponType);
        uiManager.ChangeDisplayWeapon(weaponType);
    }

    //指定された武器以外全て非アクティブにする
    private void DoActive(WeaponType weaponType)
    {
        foreach (GameObject obj in prefabWeaponObject)
        {
            Weapon weapon = obj.GetComponent<Weapon>();
            if(weapon != null && weapon.type == weaponType)
            {
                obj.SetActive(true);
                continue;
            }
            obj.SetActive(false);
        }
    }
}
