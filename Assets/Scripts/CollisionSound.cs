using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Ensure that Rigidbody component is attached
public class CollisionSound : MonoBehaviour
{
    public AudioClip[] collisionSound; // The sound to play on collision
    public float volume = 1.0f; // Volume of the collision sound

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionSound != null)
        {
            AudioSource.PlayClipAtPoint(collisionSound[Random.Range(0 , collisionSound.Length)], transform.position, volume);
        }
    }
}
