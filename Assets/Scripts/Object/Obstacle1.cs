using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle1 : MonoBehaviour
{
    [SerializeField] GameObject fireObj;
    [SerializeField] float TimeCycle; // スペル修正
    public bool isAppear { get; private set; }

    private void OnEnable()
    {
        if (fireObj != null)
        {
            StartCoroutine(OnDisableFireObj());
        }
    }

    IEnumerator OnEnableFireObj()
    {
        fireObj.SetActive(true);
        yield return new WaitForSeconds(TimeCycle); // WaitForSecondsを使用
        StartCoroutine(OnDisableFireObj()); // ここでDisableに切り替える
    }

    IEnumerator OnDisableFireObj()
    {
        fireObj.SetActive(false);
        yield return new WaitForSeconds(TimeCycle); // WaitForSecondsを使用
        StartCoroutine(OnEnableFireObj()); // ここでEnableに切り替える
    }
}
