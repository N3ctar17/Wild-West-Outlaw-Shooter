using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyController : MonoBehaviour
{
    public AudioSource SFX;
    public AudioClip sfx2;
    public void DestroyEnemy(float delay) {
        Destroy(gameObject, delay);
    }
}
