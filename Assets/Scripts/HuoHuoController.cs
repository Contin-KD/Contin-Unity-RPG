using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuoHuoController : MonoBehaviour
{
    public CharacterInputSystem _characterInputSystem;
    public Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _characterInputSystem = transform.GetComponent<CharacterInputSystem>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
