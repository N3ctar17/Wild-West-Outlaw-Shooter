using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // For TextMeshPro (use UnityEngine.UI if using regular Text)

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private int _maxAmmo = 6; // Maximum bullets in the magazine

    [SerializeField]
    private float _reloadTime = 2f; // Time it takes to reload

    [SerializeField]
    private TextMeshProUGUI ammoText; // Reference to the UI Text element

    private int _currentAmmo; // Current bullets in the magazine
    private bool _isReloading = false; // Whether the gun is reloading

    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastFireTime;
    private bool _canFire = true;


    public AudioSource src;
    public AudioClip sfx1; // Gunfire sound
    public AudioClip sfx2; // Clicking sound for out of ammo
    public AudioClip sfx3; // Reload sound

    private void Start() {
        _currentAmmo = _maxAmmo; // Start with a full magazine
        UpdateAmmoUI(); // Update the UI at the start
    }

    // Update is called once per frame
    void Update() {
        if (_isReloading || !_canFire) return; // Prevent firing during or immediately after reloading

        if (_fireContinuously || _fireSingle)
        {
            float timeSinceLastFire = Time.time - _lastFireTime;

            if (timeSinceLastFire >= _timeBetweenShots)
            {
                if (_currentAmmo > 0)
                {
                    FireBullet();
                    src.clip = sfx1;
                    src.Play();

                    _lastFireTime = Time.time;
                    _fireSingle = false;

                    _currentAmmo--; // Reduce ammo
                    UpdateAmmoUI(); // Update the UI after shooting
                }
                else
                {
                    PlayOutOfAmmoSound();
                }
            }
        }

        // Check for reload input
        if (Keyboard.current.rKey.wasPressedThisFrame && _currentAmmo < _maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }


    private void FireBullet() {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f; // Ensure Z position is 0 for 2D

        // Calculate direction from player to mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Calculate the angle in degrees and adjust by -90 degrees if the bullet sprite is initially vertical
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Instantiate the bullet and set its rotation
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * _bulletSpeed;
    }

    private IEnumerator Reload() {
        _isReloading = true;
        _canFire = false; // Disable firing

        // Clear firing input to prevent shooting after reload
        _fireSingle = false;
        _fireContinuously = false;

        Debug.Log("Reloading...");
        PlayReloadSound();

        // Simulate reload time
        yield return new WaitForSeconds(_reloadTime);

        _currentAmmo = _maxAmmo; // Refill the magazine
        _isReloading = false;

        Debug.Log("Reload complete! Ammo refilled.");
        UpdateAmmoUI(); // Update the UI after reloading

        yield return new WaitForSeconds(0.1f); // Small delay to avoid immediate firing
        _canFire = true; // Re-enable firing
    }




    private void PlayOutOfAmmoSound() {
        if (src.clip != sfx2 || !src.isPlaying) // Avoid overlapping clicking sounds
        {
            src.clip = sfx2;
            src.Play();
        }

        Debug.Log("Click! Out of ammo. Press R to reload.");
    }

    private void PlayReloadSound() {
        if (src.clip != sfx3 || !src.isPlaying) // Avoid overlapping reload sounds
        {
            src.clip = sfx3;
            src.Play();
        }
    }

    private void UpdateAmmoUI() {
        if (ammoText != null)
        {
            ammoText.text = $"{_currentAmmo}/{_maxAmmo}";
        }
    }

    private void OnFire(InputValue inputValue) {
        _fireContinuously = inputValue.isPressed;

        if (inputValue.isPressed)
        {
            _fireSingle = true;
        }
    }
}
