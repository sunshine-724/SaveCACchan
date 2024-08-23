using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType type; //この武器の種類

    [SerializeField] GameObject bullet; //銃弾
    [SerializeField] float existBulletTime = 1.5f; //銃弾の存在時間
    [SerializeField] float bulletSpeed = 1.0f; //銃弾のスピード
    bool isMoveBullet = false;

    // Update is called once per frame
    void Update()
    {
       
    }

    public void AttackGun()
    {
        Vector3 bulletPositon = this.transform.position;

        //銃弾を発射
        StartCoroutine(ShotBullet(bulletPositon));
    }

    private IEnumerator ShotBullet(Vector3 pos)
    {
        GameObject obj =  Instantiate(bullet,this.transform);


        yield return new WaitForSeconds(bulletSpeed);


        if(obj != null)
        {
            Destroy(obj);
        }

    }
}
