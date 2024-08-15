using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footing : MonoBehaviour
{
    public Vector3 point { get; private set; } //座標
    public FootingType footingType;//足場の種類
    private bool canChangeLength; //このオブジェクトの長さを変更しても良いかどうか


    public void Init(FootingType type)
    {
        canChangeLength = false;
        point = this.transform.position; //地面は動かないので座標を取得するのは一度きり
        footingType = type;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //この足場の長さ変更権を指定されたように変更する
    public void ChangeTheModificationRights(bool state)
    {

    }

    //各足場の種類に応じて、座標がこのオブジェクトの領域内に含まれているかどうか
    public bool ExsitThisObject(Vector2 point,FootingType footingType)
    {
        switch (footingType)
        {
            case FootingType.rectangle:
                if (Mathf.Abs(point.x - this.point.x) < (this.GetComponent<RectTransform>().sizeDelta.x) / 2)
                {
                    if (Mathf.Abs(point.y - this.point.y) < (this.GetComponent<RectTransform>().sizeDelta.x) / 2)
                    {
                        Debug.Log("選択されました");
                        return true;
                    }
                }
                break;

            case FootingType.triangle:
                break;

            default:break;
        }

        return false;
    }
}
