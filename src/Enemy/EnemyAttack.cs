using UnityEngine;


public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    private float lastAttackTime = 0f;
    private Animator animator;
    public Transform player;


    void Start()
    {
        animator = GetComponent<Animator>();
        this.enabled = false; // 默认不攻击，由 Chase 打开
    }


    void Update()
    {
        if (player == null) return;


        float distance = Vector3.Distance(transform.position, player.position);


        // 🟢 玩家离开攻击范围 → 回到追击（Run）
        if (distance > 1.6f)
        {
            var chase = GetComponent<EnemyChase>();
            if (chase != null)
                chase.enabled = true;


            if (animator != null)
                animator.SetBool("isMoving", true);  // 回到 Run


            this.enabled = false; // 关闭攻击脚本
            return;
        }


        // ⏱ 冷却时间没到，不攻击，只保持 Idle
        if (Time.time - lastAttackTime < attackCooldown)
            return;


        // 💥 触发一次攻击
        Attack();
        lastAttackTime = Time.time;
    }


    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");   // 播 Attack → 回 Idle


        Debug.Log($"💥 Enemy attacked player for {attackDamage} damage!");
    }
}
