using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/**
 * Author: Aleksandra Rusek
 * 
 * Controls the behavior of boss behaviour in the game.
 */
public class M_WiggleBoss : MonoBehaviour
{
    public GameObject objectToBob; /* The game object representing the boss. */
    public float amplitudeX = 1f; /* The amplitude of the bobbing motion in the X-axis. */
    public float frequencyX = 1f; /* The frequency of the bobbing motion in the X-axis. */
    public float amplitudeY = 1f; /* The amplitude of the bobbing motion in the Y-axis. */
    public float frequencyY = 1f; /* The frequency of the bobbing motion in the Y-axis. */
    private Vector3 refPos; /* Stores the initial position of the object. */
    [SerializeField] private GameObject explosionEffectPrefab; /* The prefab for the explosion effect when the boss dies. */
    [SerializeField] private GameObject healthObjects; /* The health objects associated with the boss. */
    [SerializeField] private AudioClip deathSound; /* The sound played when the boss dies. */
    [SerializeField] private AudioClip extraSound; /* The sound played when the boss receives an extra score. */
    [SerializeField] private ParticleSystem hitParticleSystem; /* Particle system for the hit effect when the boss is hit. */
    [SerializeField] private ParticleSystem simplehit1ParticleSystem; /* Particle system for the first part of a simple hit effect. */
    [SerializeField] private ParticleSystem simplehit2ParticleSystem; /* Particle system for the second part of a simple hit effect. */
    private AudioSource audioSource; /* Audiosource */
    [SerializeField] float volumeSound = 0.1f; /* Level of volume. */

    /**
     * Start is called before the first frame update.
     * It copies the initial position of the boss, sets up audio components, stops particle systems,
     * and assigns a default object to wiggle if none is specified.
     */
    public void Start()
    {
        CopyTransform(objectToBob.GetComponent<Transform>(), hitParticleSystem.GetComponent<Transform>());
        CopyTransform(objectToBob.GetComponent<Transform>(), simplehit1ParticleSystem.GetComponent<Transform>());
        CopyTransform(objectToBob.GetComponent<Transform>(), simplehit2ParticleSystem.GetComponent<Transform>());

        audioSource = objectToBob.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = objectToBob.AddComponent<AudioSource>();
            audioSource.volume = volumeSound;
        }
        hitParticleSystem.Stop();
        simplehit1ParticleSystem.Stop();
        simplehit2ParticleSystem.Stop();
        if (objectToBob == null)
        {
            objectToBob = this.gameObject;
        }
        refPos = objectToBob.transform.position;
    }
    /**
     * Update is called once per frame.
     * This method is responsible for making the boss wiggle.
     */
    public void Update()
    {
        Wiggle();

    }
    /**
     * Function that makes the boss wiggle.
     */
    private void Wiggle()
    {
        float dx = amplitudeX * (Mathf.PerlinNoise(Time.time * frequencyX, 1f) - 0.5f);
        float dy = amplitudeY * (Mathf.PerlinNoise(1f, Time.time * frequencyY) - 0.5f);
        Vector3 pos = new Vector3(refPos.x, refPos.y, refPos.z);
        pos = pos + objectToBob.transform.up * dy;
        pos = pos + objectToBob.transform.right * dx;
        objectToBob.transform.position = pos;

    }

    /**
     * Initiates the explosion of the boss.
     */
    public void Explode()
    {
        healthObjects.SetActive(false);
        PopGameObject(objectToBob);
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        if (explosionEffectPrefab == null)
            return;
        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);
    }
    /**
     * Initiates a special hit on the boss.
     */
    public void Hit()
    {
        if (extraSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(extraSound);
        }
        hitParticleSystem.Play();
        StartCoroutine(StopParticleSystemAfterDelay((1f), hitParticleSystem));
    }
    /**
     * Initiates a simple hit on the boss.
     */
    public void SimpleHit()
    {
        simplehit1ParticleSystem.Play();
        simplehit2ParticleSystem.Play();
        StartCoroutine(StopParticleSystemAfterDelay(1f, simplehit1ParticleSystem));
        StartCoroutine(StopParticleSystemAfterDelay(1f, simplehit2ParticleSystem));
    }

    /**
     * Stops the Particle System after a delay.
     * @param delay The delay before stopping the Particle System.
     * @param part The Particle System that needs to be stopped.
     */
    IEnumerator StopParticleSystemAfterDelay(float delay, ParticleSystem part)
    {
        yield return new WaitForSeconds(delay);

        part.Stop();
    }

    /**
     * Initiates the popping animation for the game object.
     * @param gameObjectToPop The game object to pop.
     */
    private async void PopGameObject(GameObject gameObjectToPop)
    {
        var imageComponent = gameObjectToPop.GetComponent<Image>();
        if (imageComponent != null)
        {
            var deflateSequence = DOTween.Sequence();
            deflateSequence.Join(imageComponent.rectTransform.DOScale(Vector3.zero, 1f));

            await deflateSequence.Play()
                               .AsyncWaitForCompletion();
        }

    }
    /**
     * Copies the transform from the source to the destination.
     * @param source The source transform.
     * @param destination The destination transform.
     */
    public void CopyTransform(Transform source, Transform destination)
    {
        destination.position = source.position;
        destination.rotation = source.rotation;
        destination.localScale = source.localScale;
    }

}
