using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> displayWeaponList;
    [SerializeField] List<GameObject> displayFootingList;
    [SerializeField] List<GameObject> displayHealthList;

    public GameObject displayWeapon { get; private set; }
    public GameObject displayFooting { get; private set; }

    public void ChangeDisplayWeapon(WeaponType type)
    {
        //DoActiveWeapon(type);
    }

    void DoActiveWeapon(WeaponType type)
    {
        //foreach(GameObject obj in displayWeaponList)
        //{
        //    Weapon weapon = obj.GetComponent<Weapon>();
        //    if(weapon != null && weapon.type == type)
        //    {
        //        displayWeapon = obj;
        //        obj.SetActive(true);
        //        continue;
        //    }

        //    obj.SetActive(false);
        //}
    }

    public void ChangeDisplayFooting(FootingType type)
    {
        DoActiveFooting(type);
    }

    void DoActiveFooting(FootingType type)
    {
        foreach (GameObject obj in displayFootingList)
        {
            Footing footing = obj.GetComponent<Footing>();
            if (footing != null && footing.footingType == type)
            {
                displayFooting = obj;
                obj.SetActive(true);
                continue;
            }

            obj.SetActive(false);
        }
    }

    public void DecreaseDisplayHealth()
    {
        for(int k = 0; k < displayHealthList.Count; k++)
        {
            if (displayHealthList[k].activeSelf == true)
            {
                displayHealthList[k].SetActive(false);
                return;
            }
        }
    }

    public void IncreaseDisplayHealth()
    {
        for (int k = displayHealthList.Count - 1; k >= 0; k--)
        {
            if (displayHealthList[k].activeSelf == false)
            {
                displayHealthList[k].SetActive(false);
                return;
            }
        }
    }
}
