using UnityEngine;


public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2.5f;
    public float attackRange = 1.5f;


    private Animator animator;
    private EnemyAttack attackScript;


    void Start()
    {
        animator = GetComponent<Animator>();
        attackScript = GetComponent<EnemyAttack>();
    }


    void Update()
    {
        if (player == null) return;


        float distance = Vector3.Distance(transform.position, player.position);


        // 🟢 还没到攻击范围 → 一直跑（Run）
        if (distance > attackRange)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            dir.z = 0;
            transform.position += dir * chaseSpeed * Time.deltaTime;


            if (animator != null)
                animator.SetBool("isMoving", true);   // 让 Animator 切到 Run
        }
        else
        {
            // 🔴 进入攻击范围 → 停止跑动，交给攻击脚本
            if (animator != null)
                animator.SetBool("isMoving", false);  // 切回 Idle，作为攻击基底


            attackScript.enabled = true;   // 开 EnemyAttack
            enabled = false;               // 关 Chase
        }
    }
}




