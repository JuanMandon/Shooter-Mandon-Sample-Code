using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Listens for weapon fire mode changes and briefly displays the corresponding UI icon 
/// (semi-auto, burst, or full-auto) using a smooth fade in and fade out animation.
/// </summary>
public class FireModeUIListener : MonoBehaviour
{
    [SerializeField] private WeaponEventChannel weaponEventChannel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Fire Mode UI Elements")]
    [SerializeField] private GameObject singleFireUI;
    [SerializeField] private GameObject fullAutoUI;
    [SerializeField] private GameObject threeRoundBurstUI;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        weaponEventChannel.OnChangeFireModeEventRaised += HandleFireModeChanged;
    }

    private void OnDisable()
    {
        weaponEventChannel.OnChangeFireModeEventRaised -= HandleFireModeChanged;
    }

    private void HandleFireModeChanged(Weapon.WeaponFireMode fireMode)
    {
        // Disable all UI variants
        singleFireUI.SetActive(false);
        fullAutoUI.SetActive(false);
        threeRoundBurstUI.SetActive(false);

        // Enable the correct firemode
        switch (fireMode)
        {
            case Weapon.WeaponFireMode.SemiAuto:
                singleFireUI.SetActive(true);
                break;
            case Weapon.WeaponFireMode.FullAuto:
                fullAutoUI.SetActive(true);
                break;
            case Weapon.WeaponFireMode.ThreeRoundBurst:
                threeRoundBurstUI.SetActive(true);
                break;
        }

        // Restart fade coroutine
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeDisplayCoroutine());
    }

    private IEnumerator FadeDisplayCoroutine()
    {
        const float fadeInDelay = 0.25f;
        const float visibleDuration = 1.5f;
        const float fadeOutDuration = 1f;

        canvasGroup.alpha = 0f;

        yield return new WaitForSeconds(fadeInDelay);
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(visibleDuration);

        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
