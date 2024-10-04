using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType type; //この武器の種類

    [SerializeField] GameObject bullet; //銃弾
    [SerializeField] float existBulletTime = 1.5f; //銃弾の存在時間
    [SerializeField] int maxBulletNumber = 3; //同時に存在できる銃弾の数

    int currentBulletNumber = 0;
    public void AttackGun()
    {
        Vector3 bulletPositon = this.transform.position;

        //銃弾を発射
        StartCoroutine(ShotBullet(bulletPositon));
    }

    private IEnumerator ShotBullet(Vector3 pos)
    {
        GameObject fireObj = null;
        if(currentBulletNumber < maxBulletNumber)
        {
            fireObj = Instantiate(bullet, this.transform);
            if (fireObj != null)
            {
                currentBulletNumber++;
            }
        }
        else
        {
            yield break;
        }

        yield return new WaitForSeconds(existBulletTime);

        if(fireObj != null)
        {
            currentBulletNumber--;
            Destroy(fireObj);
        }
    }
}
