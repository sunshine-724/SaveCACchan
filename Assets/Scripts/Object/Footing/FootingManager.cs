using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FootingManager : MonoBehaviour
{
    [SerializeField] UIManager uIManager;

    [SerializeField] Circle circle;
    [SerializeField] List<GameObject> Prefab_footing; //各足場のプレハブオブジェクト

    Queue<GameObject> footingObjects; //現在ある足場の数

    const int maxFootingNumber = 3; //置ける足場の最大個数(3個に設定)

    private void Awake()
    {
        footingObjects = new Queue<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //特定の足場を設置する
    public void PutFooting(Vector2 point,FootingType footingType)
    {
        if (CanPutFooting(point))
        {
            //クリックしたところに足場を生成
            foreach(GameObject obj in Prefab_footing)
            {
                Footing footing = obj.GetComponent<Footing>();
                if (footing != null)
                {
                    if (footingType == footing.footingType)
                    {
                        GameObject newObj = Instantiate(obj, point, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
                        if(newObj != null)
                        {
                            //新しいオブジェクトに対し、Footingクラスを付与し、識別子(FootingType)を割り振る
                            Footing newCom =  newObj.AddComponent<Footing>();
                            newCom.Init(footingType);
                            footingObjects.Enqueue(newObj); //現在出現している足場キューに登録
                        }
                        
                        break;
                    }
                }
            }
        }
    }

    //足場を置けるかどうかを判断する
    bool CanPutFooting(Vector2 point)
    {
        //条件1.現在ある足場の個数が最大個数に達していないかどうか
        if (footingObjects.Count < maxFootingNumber)
        {
            //条件2.マウスの場所がサークル内にあるかどうか
            if (circle.CheckPointinCircle(point))
            {
                return true;
            }
            
        }
        else if(footingObjects.Count == 3)
        {
            //足場がすでに3つある場合、最初に追加した足場を削除し、新たな足場を追加する
            GameObject obj = footingObjects.Peek();
            Destroy(obj);
            footingObjects.Dequeue();
            return true;
        }
        return false;
    }

    //座標にある足場を探す
    public GameObject SearchFootingObject(Vector2 point)
    {
        foreach (GameObject gameObject in footingObjects)
        {
            Footing footing = gameObject.GetComponent<Footing>(); //足場にアタッチしているクラスを取得
            if (footing == null)
            {
                continue;
            }
            else
            {
                if (footing.ExsitThisObject(point,footing.footingType))
                {
                    return footing.gameObject;
                }
                
            }
        }

        return null;
    }


    //足場を伸ばす
    public void ExtendFooting(Vector2 point)
    {
        
    }

    //足場を変更する
    public void ChangeFooting(FootingType footing)
    {
        //UIに変更を反映させる
        uIManager.ChangeDisplayFooting(footing);
    }

}
