using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.UI.Image;


public class Footing : MonoBehaviour
{
    public Vector2 currentPoint { get; private set; } //中心座標
    public FootingType footingType;//足場の種類

    private float maxYPoint; //動く足場の上限
    private float minYPoint; //動く足場の下限
    private int state; //今この足場がどのような状態か 0:stop,1,up,2:down
    [SerializeField] float speed = 0.05f; //動く足場のスピード


    private bool canChangeLeftLength;
    private bool canChangeRightLength;
    [SerializeField] float minExtendLength = 0.2f;
    [SerializeField] float maxExtendLength = 10.0f;

    [SerializeField]
    [Range(0, 1)]
    float error = 0.995f; //端からどれくらい離れていてもマウスが反応するか

    public void Init(FootingType type)
    {
        currentPoint = this.transform.position;
        canChangeLeftLength = false;
        canChangeRightLength = false;
        footingType = type;

        switch (type)
        {
            case FootingType.stopRectangle:
                state = 0;
                break;

            case FootingType.moveUpRectangle:
                maxYPoint = currentPoint.y + 10.0f;
                minYPoint = currentPoint.y;
                state = 1;
                break;

            case FootingType.moveDownRectangle:
                maxYPoint = currentPoint.y;
                minYPoint = currentPoint.y - 10.0f;
                state = 2;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentPoint = this.transform.position;

        //必要に応じて足場を伸ばす
        StreachFooting();

        //足場を移動する
        OnMove();
    }

    //private void FixedUpdate()
    //{
    //        currentPoint = this.transform.position;

    //        //必要に応じて足場を伸ばす
    //        StreachFooting();

    //        //足場を移動する
    //        OnMove();
    //}

    //必要に応じて足場を伸ばす
    private void StreachFooting()
    {
        if (canChangeLeftLength)
        {
            // 現在のマウスデバイスを取得
            var mouse = Mouse.current;
            if (mouse != null)
            {
                //マウスの移動量を取得する
                Vector2 mouseDelta = mouse.delta.ReadValue();
                mouseDelta *= -1;
                Vector2 newCenterPoint = new Vector2(currentPoint.x - (mouseDelta.x) / 2, currentPoint.y);
                Debug.Log("右端を変更しています");

                float magnification = (mouseDelta.x + (this.transform.localScale.x)) / (this.transform.localScale.x); //倍率
                Debug.Log(magnification * this.transform.localScale.x);
                //magnification *= correction;
                if(minExtendLength < magnification*this.transform.localScale.x && magnification*this.transform.localScale.x < maxExtendLength)
                {
                    this.transform.position = newCenterPoint;
                    this.transform.localScale = new Vector3(magnification * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
                }
                //if (magnification * this.transform.localScale.x > 0.2f)
                //{
                //    this.transform.position = newCenterPoint;
                //    this.transform.localScale = new Vector3(magnification * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
                //}
            }
        }
        else if (canChangeRightLength)
        {
            // 現在のマウスデバイスを取得
            var mouse = Mouse.current;
            if (mouse != null)
            {
                //マウスの移動量を取得する
                Vector2 mouseDelta = mouse.delta.ReadValue();
                Vector2 newCenterPoint = new Vector2(currentPoint.x + (mouseDelta.x) / 2, currentPoint.y);

                //中心座標をずらす
                Debug.Log("左端を変更しています");

                float magnification = (mouseDelta.x + (this.transform.localScale.x)) / (this.transform.localScale.x); //倍率
                //magnification *= correction;
                //if (magnification * this.transform.localScale.x > 0.2f)
                //{
                //    this.transform.position = newCenterPoint;
                //    this.transform.localScale = new Vector3(magnification * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
                //}

                if(minExtendLength < magnification*this.transform.localScale.x && magnification*this.transform.localScale.x < maxExtendLength)
                {
                    this.transform.position = newCenterPoint;
                    this.transform.localScale = new Vector3(magnification * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
                }
            }
        }
    }

    private void OnMove()
    {
        switch (state)
        {
            case 0:
                break;
            case 1:
                MoveUp();
                break;

            case 2:
                MoveDown();
                break;
        }
    }

    private void MoveUp()
    {
        //Vector3 tmp = currentPoint;
        //tmp.y += speed;
        //transform.position = tmp;

        //tmp.y += 0.6f; //自分自身を参照しないよう補正をかける

        //RaycastHit2D hit = Physics2D.Raycast(tmp, Vector2.up, 0.01f);

        //if (currentPoint.y >= maxYPoint)
        //{
        //    state = 2;
        //}

        //if (hit.collider != null)
        //{
        //    if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Obstacles"))
        //    {
        //        state = 2;
        //    }
        //}

        Vector2 targetPoint = currentPoint;
        targetPoint.y += speed;
        float currentVelocity = ((targetPoint - currentPoint) / Time.deltaTime).magnitude;

        targetPoint.y = Mathf.SmoothDamp(currentPoint.y, targetPoint.y, ref currentVelocity, currentVelocity);
        this.transform.position = targetPoint;

        RaycastHit2D hit = Physics2D.Raycast(targetPoint, Vector2.down, 0.01f);

        if (currentPoint.y <= minYPoint)
        {
            state = 1;
        }

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Obstacles"))
            {
                state = 1;
            }
        }
    }

    private void MoveDown()
    {
        //Vector3 tmp = currentPoint;
        //tmp.y -= speed;
        //transform.position = tmp;

        //tmp.y -= 0.6f; //自分自身を参照しないよう補正をかける

        //RaycastHit2D hit = Physics2D.Raycast(tmp, Vector2.down, 0.01f);

        //if (currentPoint.y <= minYPoint)
        //{
        //    state = 1;
        //}

        //if (hit.collider != null)
        //{
        //    if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Obstacles"))
        //    {
        //        state = 1;
        //    }
        //}

        Vector2 targetPoint = currentPoint;
        targetPoint.y -= speed;
        float currentVelocity = ((targetPoint - currentPoint) / Time.deltaTime).magnitude;

        targetPoint.y = Mathf.SmoothDamp(currentPoint.y, targetPoint.y, ref currentVelocity,currentVelocity);
        this.transform.position = targetPoint;

        RaycastHit2D hit = Physics2D.Raycast(targetPoint, Vector2.down, 0.01f);

        if (currentPoint.y <= minYPoint)
        {
            state = 1;
        }

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Obstacles"))
            {
                state = 1;
            }
        }
    }

    //この足場の長さ変更権を指定されたように変更する
    public void FinishChangeRight()
    {
        canChangeRightLength = false;
        canChangeLeftLength = false;
    }

    //各足場の種類に応じて、座標がこのオブジェクトの領域内に含まれているかどうか
    public bool ExsitThisObject(Vector2 point,FootingType footingType)
    {
        float sizeX = this.GetComponent<Renderer>().bounds.size.x;
        float sizeY = this.GetComponent<Renderer>().bounds.size.y;

        //stopRectangleのみ引き伸ばすのを許可する
        switch (footingType)
        {
            case FootingType.stopRectangle:
                //左に伸ばすのかどうか
                if (currentPoint.x - (sizeX / 2) < point.x && (point.x) < currentPoint.x - (sizeX/2) + error)
                {
                    if (Mathf.Abs(point.y - this.currentPoint.y) < this.GetComponent<Renderer>().bounds.size.y)
                    {
                        Debug.Log("左側が選択されました");
                        canChangeLeftLength = true;
                        canChangeRightLength = false;
                        return true;
                    }
                }else if((currentPoint.x + (sizeX/2) - error) < point.x && point.x < (currentPoint.x + (sizeX/2))){
                    //右に伸ばすかどうか
                    if (Mathf.Abs(point.y - this.currentPoint.y) < this.GetComponent<Renderer>().bounds.size.y)
                    {
                        Debug.Log("右側が選択されました");
                        canChangeRightLength = true;
                        canChangeLeftLength = false;
                        return true;
                    }
                }
                break;

            case FootingType.moveUpRectangle:
                break;

            case FootingType.moveDownRectangle:
                break;

            default:break;
        }

        return false;
    }
}
