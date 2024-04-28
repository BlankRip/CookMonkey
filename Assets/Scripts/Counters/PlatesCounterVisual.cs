using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;
    private float plateOffsetY = 0.1f;

    private void Start()
    {
        plateVisualGameObjectList = new List<GameObject>();
        platesCounter.OnPlateSpawned += OnPlateSpawned;
        platesCounter.OnPlateRemoved += OnPlateRemoved;
    }

    private void OnDestroy()
    {
        platesCounter.OnPlateSpawned -= OnPlateSpawned;
        platesCounter.OnPlateRemoved -= OnPlateRemoved;
    }
    private void OnPlateSpawned()
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        plateVisualTransform.localPosition = new Vector3(0.0f, plateOffsetY * plateVisualGameObjectList.Count, 0.0f);
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

    private void OnPlateRemoved()
    {
        int lastIndex = plateVisualGameObjectList.Count - 1;
        Destroy(plateVisualGameObjectList[lastIndex]);
        plateVisualGameObjectList.RemoveAt(lastIndex);
    }
}
