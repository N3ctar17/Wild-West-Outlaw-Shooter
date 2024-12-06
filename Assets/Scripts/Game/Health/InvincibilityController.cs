using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityController : MonoBehaviour
{
    private HealthController _healthController;
    private Flash _spriteFlash;

    private void Awake() {
        _healthController = GetComponent<HealthController>();
        _spriteFlash = GetComponent<Flash>();
    }

    public void StartInvincibility(float invincibilityDuration, Color flashColor, int numberOfFlashes) {
        StartCoroutine(InvincibilityCoroutine(invincibilityDuration, flashColor, numberOfFlashes));
    }

    private IEnumerator InvincibilityCoroutine(float invincibilityDuration, Color flashColor, int numberOfFlashes) {
        _healthController.IsInvincible = true;

        // Start flashing
        yield return _spriteFlash.FlashCoroutine(invincibilityDuration, flashColor, numberOfFlashes);

        // End invincibility
        _healthController.IsInvincible = false;
    }

}
