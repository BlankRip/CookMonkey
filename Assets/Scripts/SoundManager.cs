using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] Player player;
    [SerializeField] DeliveryCounter deliveryCounter;
    [Space] [Space]

    [SerializeField] private AudioClip[] chop;
    [SerializeField] private AudioClip[] deliveryFail;
    [SerializeField] private AudioClip[] deliverySuccess;
    [SerializeField] private AudioClip[] footSteps;
    [SerializeField] private AudioClip[] objectDrop;
    [SerializeField] private AudioClip[] objectPickup;
    [SerializeField] private AudioClip[] trash;
    [SerializeField] private AudioClip[] warning;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;

        player.OnPickSomething += Player_OnPickSomething;
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeCompleted -= DeliveryManager_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeFailed -= DeliveryManager_OnRecipeFailed;

        BaseCounter.OnAnyObjectPlacedHere -= BaseCounter_OnAnyObjectPlacedHere;
        CuttingCounter.OnAnyCut -= CuttingCounter_OnAnyCut;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;

        player.OnPickSomething -= Player_OnPickSomething;
    }

    private void DeliveryManager_OnRecipeCompleted()
    {
        PlaySound(deliverySuccess, deliveryCounter.transform.position);
    }
    private void DeliveryManager_OnRecipeFailed()
    {
        PlaySound(deliveryFail, deliveryCounter.transform.position);
    }


    private void BaseCounter_OnAnyObjectPlacedHere(BaseCounter sender)
    {
        PlaySound(objectDrop, sender.transform.position);

    }
    private void CuttingCounter_OnAnyCut(CuttingCounter sender)
    {
        PlaySound(chop, sender.transform.position);
    }
    private void TrashCounter_OnAnyObjectTrashed(TrashCounter sender)
    {
        PlaySound(trash, sender.transform.position);

    }

    private void Player_OnPickSomething()
    {
        PlaySound(objectPickup, player.transform.position);
    }

    public void PlayFootStepsSound(Vector3 position, float volume)
    {
        PlaySound(footSteps, position, volume);
    }

    public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1.0f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1.0f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
