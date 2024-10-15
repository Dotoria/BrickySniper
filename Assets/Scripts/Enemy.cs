using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;
    private Player player;
    private RaycastHit2D hit;
    private EnemySpawner _spawner;

    public int healthPoint;
    public int attackPoint;
    public int movePoint;
    public Target attackTarget;
    public AttackLogic attackLogic;
    public MoveLogic moveLogic;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawner = GetComponent<EnemySpawner>();
        hit = Physics2D.Raycast(transform.position, transform.forward);

        if (attackTarget == Target.Player)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (moveLogic == MoveLogic.ToPlayer)
        {
            agent.SetDestination(target.position);
            agent.speed = movePoint * 0.5f;
        }
        
        if (attackLogic == AttackLogic.HitPlayer)
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit collider player");
                player.health -= attackPoint;
                _spawner.enemyPool.ReturnToPool(gameObject);
            }
        }

        if (hit.collider.CompareTag("Ball"))
        {
            Debug.Log("Hit collider ball");
            healthPoint--;
        }

        if (healthPoint == 0)
        {
            _spawner.enemyPool.ReturnToPool(gameObject);
        }
    }
}