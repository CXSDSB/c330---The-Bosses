using UnityEngine;


public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float detectRange = 6f;        // 视野范围（可不用）
    public float chaseTriggerRange = 4f;  // 玩家进入这个范围后追击
    public Transform player;


    private int currentPoint = 0;


    void Start()
    {
        // 开局关闭追逐和攻击，保证只巡逻
        EnemyChase chase = GetComponent<EnemyChase>();
        if (chase != null) chase.enabled = false;


        EnemyAttack attack = GetComponent<EnemyAttack>();
        if (attack != null) attack.enabled = false;
    }


    void Update()
    {
        // 1) 必须有巡逻点
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;


        // 2) 永远先巡逻
        Patrol();


        // 3) 玩家还没设 → 只巡逻
        if (player == null)
            return;


        // 4) 计算玩家距离
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        // 5) 玩家进入追逐范围 → 切换到 EnemyChase
        if (distanceToPlayer <= chaseTriggerRange)
        {
            Debug.Log($"🔴 Player entered chase range ({distanceToPlayer:F2}) → Switch to Chase");


            EnemyChase chase = GetComponent<EnemyChase>();
            if (chase != null)
            {
                if (chase.player == null)
                    chase.player = player;


                chase.enabled = true;
            }


            enabled = false;   // 停止巡逻逻辑
        }
    }


    void Patrol()
    {
        // 当前巡逻目标点
        Transform target = patrolPoints[currentPoint];


        // 移动到目标点
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );


        // ⭐⭐ 使用 rotation 翻转敌人朝向（不会影响移动）
        Vector3 dir = target.position - transform.position;
        HandleRotation(dir.x);


        // 到达巡逻点 → 切换到下一个
        float distToTarget = Vector2.Distance(transform.position, target.position);
        if (distToTarget < 0.1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }


    // 🔁 使用 rotation 翻转面朝方向（不会破坏移动）
    void HandleRotation(float xDir)
    {
        if (xDir > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);   // 面向右
        }
        else if (xDir < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // 面向左
        }
    }
}
