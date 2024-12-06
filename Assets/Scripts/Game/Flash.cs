using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void StartFlash(float flashDuration, Color flashColor, int numberOfFlashes) {
        StartCoroutine(FlashCoroutine(flashDuration, flashColor, numberOfFlashes));
    }

    public IEnumerator FlashCoroutine(float flashDuration, Color flashColor, int numberOfFlashes) {
        Color startColor = _spriteRenderer.color;
        float flashInterval = flashDuration / (numberOfFlashes * 2); // Half for flash on, half for flash off

        for (int i = 0; i < numberOfFlashes; i++) {
            // Set to flash color
            _spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashInterval);

            // Revert to start color
            _spriteRenderer.color = startColor;
            yield return new WaitForSeconds(flashInterval);
        }

        // Ensure the color is reset to the original
        _spriteRenderer.color = startColor;
    }
}
