using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public AudioClip[] audioClips;  // Array to hold the audio clips
    private Color _originalLiquidColor;
    public ParticleSystem particleSys;
    private AudioSource _audioSource;  // AudioSource component
    public float colorTransitionDuration = 1.0f; // Duration for color transition

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            // If no AudioSource is found, add one
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        SpriteRenderer liquidSpriteRenderer = GetChildSpriteRenderer("Liquid");
        if (liquidSpriteRenderer != null)
        {
            _originalLiquidColor = liquidSpriteRenderer.color;
        }
    }

    SpriteRenderer GetChildSpriteRenderer(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            return child.GetComponent<SpriteRenderer>();
        }
        return null;
    }

    // This method is called when another collider enters the trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Retrieve the color of the collided object
        Color collidedColor = GetSpriteColor(collision.gameObject);

        // Retrieve the color of the current object
        Color currentColor = GetSpriteColor(gameObject);

        // Mix the colors
        Color mixedColor = MixColors(collidedColor, currentColor);

        // Instantiate and configure the particle system
        ParticleSystem instantiatedParticleSystem = Instantiate(particleSys, collision.transform.position, Quaternion.identity);
        var mainModule = instantiatedParticleSystem.main;
        mainModule.startColor = _originalLiquidColor; // Set to the original color
        instantiatedParticleSystem.Play();

        // Apply the mixed color to the child object named "Liquid" with a transition
        StartCoroutine(TransitionColor("Liquid", _originalLiquidColor, mixedColor, colorTransitionDuration));
        _originalLiquidColor = mixedColor;

        PlayRandomClip();

        // Destroy the colliding object
        Destroy(collision.gameObject);
    }

    Color GetSpriteColor(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.color;
        }
        return Color.white; // Default color if no sprite renderer is found
    }

    void SetChildSpriteColor(string childName, Color color)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }
        else
        {
            Debug.LogWarning($"Child named {childName} not found.");
        }
    }

    Color MixColors(Color color1, Color color2)
    {
        // Mix the colors (you can adjust the mixing logic as needed)
        return (color1 + color2) / 2f;
    }

    // Function to play a random audio clip
    public void PlayRandomClip()
    {
        if (_audioSource != null)
        {
            if (audioClips.Length == 0)
            {
                Debug.LogWarning("No audio clips assigned.");
                return;
            }

            // Select a random audio clip
            int randomIndex = Random.Range(0, audioClips.Length);
            AudioClip randomClip = audioClips[randomIndex];

            // Play the selected audio clip
            _audioSource.clip = randomClip;
            _audioSource.Play();

            Debug.Log("Played SFX.");
        }
        else
        {
            Debug.LogWarning("No Audio Source Found");
        }
    }

    IEnumerator TransitionColor(string childName, Color startColor, Color endColor, float duration)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                float elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                spriteRenderer.color = endColor;
            }
        }
        else
        {
            Debug.LogWarning($"Child named {childName} not found.");
        }
    }
}
