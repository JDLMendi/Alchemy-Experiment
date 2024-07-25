using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticles : MonoBehaviour
{
    public AudioClip[] audioClips;  // Array to hold the audio clips
    private AudioSource audioSource;  // AudioSource component
    private Color originalLiquidColor;

    public ParticleSystem ps;


    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If no AudioSource is found, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        SpriteRenderer liquidSpriteRenderer = GetChildSpriteRenderer("Liquid");
        if (liquidSpriteRenderer != null)
        {
            originalLiquidColor = liquidSpriteRenderer.color;
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
        ParticleSystem instantiatedParticleSystem = Instantiate(ps, collision.transform.position, Quaternion.identity);
        var mainModule = instantiatedParticleSystem.main;
        mainModule.startColor = originalLiquidColor; // Set to the original color
        instantiatedParticleSystem.Play();

        // Apply the mixed color to the child object named "Liquid"
        SetChildSpriteColor("Liquid", mixedColor);
        originalLiquidColor = mixedColor;


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
        if (audioSource != null)
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
            audioSource.clip = randomClip;
            audioSource.Play();

            Debug.Log("Played SFX.");
        }
        else
        {
            Debug.LogWarning("No Audio Source Found");
        }

    }
}
