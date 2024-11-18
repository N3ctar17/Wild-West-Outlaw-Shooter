using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private float _timeBetweenShots;

    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastFireTime;
    public AudioSource src;
    public AudioClip sfx1;

    



    // Update is called once per frame
    void Update()
    {
        if (_fireContinuously || _fireSingle) {
            float timeSinceLastFire = Time.time - _lastFireTime;

            if (timeSinceLastFire >= _timeBetweenShots) {
                FireBullet();
                src.clip = sfx1;
                src.Play();

                _lastFireTime = Time.time;
                _fireSingle = false;
            }
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



    private void OnFire(InputValue inputValue) {
        _fireContinuously = inputValue.isPressed;

        if (inputValue.isPressed) {
            _fireSingle = true;
        }
    }
}
