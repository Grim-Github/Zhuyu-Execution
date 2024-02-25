using Lexone.UnityTwitchChat;
using UnityEngine;
using UnityEngine.AI;

public class Pepe : MonoBehaviour
{
    [SerializeField] private AudioSource Asource;
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] public Chatter chatter;
    [SerializeField] private GameObject freakedOutParticle;
    private GameManager gameManager;
    public bool isBad = false;
    private Animator animator;
    [SerializeField] private int cyclesToDespawn = 5;
    public int cycles = 0;
    private bool running = false;
    private bool isFreakedout = false;
    public Vector3 center; // Center of the walking area
    public Vector3 size;   // Size of the walking area
    public NavMeshAgent agent;
    public float stopTime = 2f; // Time to stop in seconds
    private float remainingStopTime = 0;
    private float startAgentSpeed;
    private Vector3 targetPosition;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // Start walking immediately
        WalkToRandomPosition();
        startAgentSpeed = agent.speed;
    }


    public void UpdateTexts()
    {
        gameManager.textUpTime = 1;
        gameManager.nameText.text = chatter.tags.displayName;
        gameManager.nameText.color = chatter.GetNameColor();

        gameManager.messageText.text = chatter.message;
    }

    Transform GetNearestSpawnPoint()
    {
        GameObject[] allSpawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");

        Transform nearestFoodComponent = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject foodComponent in allSpawnPoints)
        {
            float distance = Vector3.Distance(transform.position, foodComponent.transform.position);

            if (distance < nearestDistance)
            {
                nearestFoodComponent = foodComponent.transform;
                nearestDistance = distance;
            }
        }

        if (nearestDistance <= Mathf.Infinity)
        {
            return nearestFoodComponent;
        }
        else
        {
            return null; // No suitable object found within the search radius
        }
    }

    public void Freakout()
    {
        if(agent.enabled == false)
        {
            return;
        }

        isFreakedout = true;
        cycles = cyclesToDespawn;
        agent.speed = startAgentSpeed * 2;
        Instantiate(freakedOutParticle, transform.position + Vector3.up * 2, Quaternion.identity);
        running = true;
        agent.SetDestination(GetNearestSpawnPoint().position);
    }

    void Update()
    {
        if (agent.enabled == true)
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                if (running)
                {
                    animator.SetBool("Running", true);
                }
                else
                {
                    animator.SetBool("Walking", true);
                }

            }
            else
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
            }

            // Check if agent is not walking
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                if (remainingStopTime <= 0)
                {
                    cycles++;
                    if (cycles == cyclesToDespawn)
                    {
                        agent.SetDestination(GetNearestSpawnPoint().position);
                        if (isBad)
                        {
                            running = true;
                            agent.speed = startAgentSpeed * 2;
                        }
                    }
                    else
                    {
                        WalkToRandomPosition();
                    }

                    remainingStopTime = stopTime;

                    if (cycles > cyclesToDespawn)
                    {
                        { 
                            if(isBad)
                            {
                                gameManager.LoseGame();
                            }

                            Destroy(gameObject);
                        }
                    }


                }
                else
                {
                    remainingStopTime -= Time.deltaTime;
                }

            }
        }

    }

    void WalkToRandomPosition()
    {
        // Generate a random target position within the defined area
        targetPosition = center + new Vector3(Random.Range(-size.x / 2, size.x / 2),
                                                0,
                                                Random.Range(-size.z / 2, size.z / 2));

        // Move towards the target position
        agent.SetDestination(targetPosition);

    }


    // Optionally, you can draw the walking area in the editor for visual reference
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }

    public void Death()
    {
        if(!isBad)
        {
            gameManager.LoseGame();
        }

        Asource.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
        agent.enabled = false;
        Destroy(gameObject, 5);
    }

}
