using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss instance;

    private void Awake()
    {
        instance = this;
    }
}
