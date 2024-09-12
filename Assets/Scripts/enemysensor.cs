using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemysensor : MonoBehaviour
{
    public GameObject PlayerObj;
    public float inspect_range;
    public GameObject parentObj;
    // Update is called once per frame
    void Update()
    {
        if(PlayerObj != null){
            if(Vector2.Distance(gameObject.transform.position,PlayerObj.transform.position) < inspect_range){
                parentObj.GetComponent<enemycontrol2>().ischased = true;
            }
        }
    }
}
