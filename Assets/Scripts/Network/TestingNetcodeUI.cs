using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] Button startHostButton;
    [SerializeField] Button startClintButton;

    private void Start()
    {
        startHostButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.StartHost();
            this.gameObject.SetActive(false);
        });
        startClintButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.StartClient();
            this.gameObject.SetActive(false);
        });
    }
}
