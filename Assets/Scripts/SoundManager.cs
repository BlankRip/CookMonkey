using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME_KEY, 1.0f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private const string PLAYER_PREFS_SFX_VOLUME_KEY = "SfxVolume";

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
    private float volume = 1.0f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;

        GameEvents.Instance.OnPlayerPickSomething += Player_OnPickSomething;
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeCompleted -= DeliveryManager_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeFailed -= DeliveryManager_OnRecipeFailed;

        BaseCounter.OnAnyObjectPlacedHere -= BaseCounter_OnAnyObjectPlacedHere;
        CuttingCounter.OnAnyCut -= CuttingCounter_OnAnyCut;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;

        GameEvents.Instance.OnPlayerPickSomething -= Player_OnPickSomething;
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

    private void Player_OnPickSomething(Player sender)
    {
        PlaySound(objectPickup, sender.transform.position);
    }

    public void PlayFootStepsSound(Vector3 position, float volumeMultiplier)
    {
        PlaySound(footSteps, position, volumeMultiplier);
    }

    public void PlayeCountdownSound()
    {
        PlaySound(warning, Vector3.zero);
    }

    public void PlayeWarningSound(Vector3 position)
    {
        PlaySound(warning, position);
    }

    public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1.0f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1.0f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1.05f)
        {
            volume = 0.0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME_KEY, volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}
