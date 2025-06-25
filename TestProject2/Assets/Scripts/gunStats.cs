using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public GameObject model;
    [Range(1, 100)] public float damage;
    [Range(1, 1000)] public float maxRadius;
    [Range(0, 10)] public float fireRate;
    [Range(1, 1000)] public int maxAmmo;
    public int ammo;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    public AudioClip[] reloadSound;
    [Range(0, 2)] public float SFXFireVolume;
    [Range(0, 2)] public float SFXReloadVolume;
}
