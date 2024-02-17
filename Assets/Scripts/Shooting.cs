using UnityEngine;
using UnityEngine.Rendering;

public class Shooting : MonoBehaviour
{
    RaycastHit hit;
    [Header("Components")]
    [SerializeField] private GameObject[] blooddecalPrefab;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private GameObject bulletTravel;

    [Header("Audio")]
    [SerializeField] private AudioSource audioS;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip[] zhuyu_clip;

    [Header("Layers")]
    [SerializeField] private LayerMask pepeHit, bloodSplatter;
    [SerializeField] public Volume pppDead;

    [Header("Variables")]
    [SerializeField] private float shootingRate = 1;
    [SerializeField] private float shootingAgroRange = 5;

    private float remainingshootingRate = 1;
    private GameManager gameManager;


    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void PostProcessing()
    {

        if (pppDead.weight > 0)
        {
            pppDead.weight -= Time.deltaTime;
        }

    }

    void Update()
    {
        PostProcessing();

        if (remainingshootingRate > 0)
        {
            remainingshootingRate -= Time.deltaTime;
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000 , pepeHit))
        {        

            if (hit.transform.CompareTag("Pepe"))
            {
                hit.transform.GetComponent<Pepe>().UpdateTexts();
            }
            else
            {
                gameManager.DisableTexts();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (GetComponent<FirstPersonCameraRotation>().enableScope)
                {
                    if (remainingshootingRate <= 0)
                    {
                        Shoot(hit);
                    }
                }
            }
        }
        else
        {
            gameManager.DisableTexts();
        }
    }

    void PlayZhuyuKill()
    {
        audioS.PlayOneShot(zhuyu_clip[Random.Range(0, zhuyu_clip.Length)]);
    }

    void Shoot(RaycastHit hit)
    {
        GetComponent<FirstPersonCameraRotation>().enableScope = false;
        audioS.PlayOneShot(clip);
        remainingshootingRate = shootingRate;

        GameObject bullet = Instantiate(bulletTravel, transform.position, Quaternion.identity);
        bullet.GetComponent<TravelTowards>().destination = hit.point;

        GameObject[] allPepesTransforms = GameObject.FindGameObjectsWithTag("Pepe");

        if (hit.transform.GetComponent<Rigidbody>())
        {
            hit.rigidbody.AddForce(hit.point.normalized *15 , ForceMode.Impulse);
        }

        audioS.PlayOneShot(reloadClip);
        if (hit.transform.GetComponent<Animator>() != null)
        {
            hit.transform.GetComponent<Animator>().SetTrigger("Dead");
            GameObject decalObject = Instantiate(bloodPrefab, hit.point + (hit.normal * 0.025f), Quaternion.identity) as GameObject;
            decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            decalObject.transform.parent = hit.transform.GetChild(0).transform;
            hit.transform.GetComponent<Pepe>().Death();
            pppDead.weight = 1;
            

            if (hit.transform.GetComponent<Pepe>().isBad == true)
            {
                Invoke("PlayZhuyuKill", 1);
            }

            Destroy(hit.transform.GetComponent<Collider>());

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000 , bloodSplatter))
            {
                GameObject bloodObject = Instantiate(blooddecalPrefab[Random.Range(0,blooddecalPrefab.Length)], hit.point + (hit.normal * 0.025f), Quaternion.identity) as GameObject;
                bloodObject.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

            }
        }
        else
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + (hit.normal * 0.025f), Quaternion.identity) as GameObject;
            bulletHole.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
        }

        foreach (GameObject p in allPepesTransforms)
        {
            if (Vector3.Distance(hit.transform.position, p.transform.position) <= shootingAgroRange)
            {
                if(p.GetComponent<Pepe>().agent.enabled == true)
                {
                    p.GetComponent<Pepe>().Freakout();
                }
            }
        }


    }

    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10);
    }
}
