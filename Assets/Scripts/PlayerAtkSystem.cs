using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkSystem : MonoBehaviour
{
    HuoHuoController controller;
    private void Awake()
    {
        if (controller != null)
        {
            return;
        }
        controller = transform.GetComponent<HuoHuoController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller._characterInputSystem.playerLAtk)
        {
            controller._animator.SetTrigger("LAtk");
        }
    }
}
