using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;

    [Space]
    [Header("Success Values")]
    [SerializeField] private Color successColor;
    [SerializeField] private Sprite successSprite;
    private const string SUCCESS_TEXT = "DELIVERY \n SUCCESS";
    [Header("Failed Values")]
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite failedSprite;
    private const string FAILED_TEXT = "DELIVERY \n FAILED";

    private Animator animator;
    private const string POP_UP_ANIMATOR_KEY = "PopUp";

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManger_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManger_OnRecipeFail;

        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeCompleted -= DeliveryManger_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed -= DeliveryManger_OnRecipeFail;
    }

    private void DeliveryManger_OnRecipeSuccess()
    {
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.SetText(SUCCESS_TEXT);
        gameObject.SetActive(true);
        animator.SetTrigger(POP_UP_ANIMATOR_KEY);
    }

    private void DeliveryManger_OnRecipeFail()
    {
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.SetText(FAILED_TEXT);
        gameObject.SetActive(true);
        animator.SetTrigger(POP_UP_ANIMATOR_KEY);
    }

}
