using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Start()
    {
        var obj = ObjectPoolManager.instance.GetGo("Player");
        obj.SetActive(false);
        obj.SetActive(true);
    }
}
