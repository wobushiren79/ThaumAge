using System;
using UnityEditor;
using UnityEngine;

public class MagicCpt : BaseMonoBehaviour
{
    public Rigidbody rbMaigc;
    public Action<MagicCpt, Collider> actionForTriggerEnter;

    public void Awake()
    {
        rbMaigc = gameObject.GetComponent<Rigidbody>();
    }


    public void OnTriggerEnter(Collider other)
    {
        actionForTriggerEnter?.Invoke(this, other);
    }
}