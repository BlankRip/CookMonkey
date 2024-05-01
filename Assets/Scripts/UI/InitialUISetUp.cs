using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialUISetUp : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToActivateOnAwake;

    private void Awake()
    {
        foreach (GameObject go in objectsToActivateOnAwake)
        {
            go.SetActive(true);
        }
    }
}
