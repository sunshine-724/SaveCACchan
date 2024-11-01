using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    [SerializeField] List<Weapon> prefabWeaponObject; //プレハブの武器オブジェクトリスト(アクティブ状態の武器だけ使用可能)

    [SerializeField] float initialXPositionError = 2.0f; //攻撃オブジェクトの初期値をプレイヤーの中心座標からどれくらい離せるか

    // Update is called once per frame
    void Update()
    {

    }


    //受け取った武器に変更する
    public void ChangeWeapon(WeaponType weaponType)
    {
        //Debug.Log(weaponType + "に切り替えました");
        DoActive(weaponType);
        //uiManager.ChangeDisplayWeapon(weaponType);
    }

    //指定された武器以外全て非アクティブにする(UI)
    private void DoActive(WeaponType weaponType)
    {
        //foreach (Weapon weaponObj in prefabWeaponObject)
        //{
        //    GameObject obj = weaponObj.gameObject;
        //    if (weaponObj != null && weaponObj.type == weaponType)
        //    {
        //        obj.SetActive(true);
        //        continue;
        //    }

        //    obj.SetActive(false);
        //}
    }

    public void Attack(WeaponType type,Vector3 playerPos,Direction direction,Player1 player1)
    {
        foreach (Weapon weaponObj in prefabWeaponObject)
        {
            if (weaponObj != null && weaponObj.type == type)
            {
                switch (type)
                {
                    case WeaponType.bubble:
                        if(direction == Direction.left)
                        {
                            playerPos.x -= initialXPositionError;
                        }
                        else
                        {
                            playerPos.x += initialXPositionError;
                        }
                        GameObject gameObj = Instantiate(weaponObj.gameObject,playerPos,Quaternion.Euler(0.0f,0.0f,0.0f));
                        BubbleObject com = gameObj.GetComponent<BubbleObject>();
                        if (com != null)
                        {
                            com.Init(direction,player1);
                        }
                        break;
                }
                break;
            }
        }
    }
}
