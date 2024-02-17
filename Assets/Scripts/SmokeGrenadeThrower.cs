using UnityEngine;

public class SmokeGrenadeThrower : MonoBehaviour
{
    [SerializeField] private GameObject smokeGrenade;
    [SerializeField] private float throwSpeed = 10;
    [SerializeField] private float yOffset = 3;
    [SerializeField] private Vector3 throwDirection = Vector3.zero;

    public void ThrowGrenade()
    {
        GameObject grenade = Instantiate(smokeGrenade, transform.position + Vector3.up * yOffset, Quaternion.identity);
        grenade.GetComponent<Rigidbody>().AddForce(throwDirection * throwSpeed, ForceMode.Impulse);
        grenade.GetComponentInChildren<ParticleSystem>().startColor = Random.ColorHSV();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; // You can change the color as desired
        Gizmos.DrawLine(transform.position + Vector3.up * yOffset, transform.position + throwDirection);

    }


}
