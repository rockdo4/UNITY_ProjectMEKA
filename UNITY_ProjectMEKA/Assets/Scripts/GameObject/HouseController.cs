using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public float houseMaxHp;
    public Defines.GateType gateType;

    [HideInInspector]
    public float houseHp;

    private void Awake()
    {
        houseHp = houseMaxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            houseHp -= 1f;
        }
    }

    private void Update()
    {
        if(houseHp <= 0f)
        {
            // �������� ���� �Լ� ȣ��
            //Debug.Log("STAGE FAIL");
        }
    }
}
