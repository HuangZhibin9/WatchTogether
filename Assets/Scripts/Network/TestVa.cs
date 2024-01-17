using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVa : MonoBehaviour
{
    [SerializeField] private NetworkProgress progressManager;

    [Button]
    public void SetClientProgress(string value)
    {
        progressManager.SetClientProgress(float.Parse(value));
    }
}