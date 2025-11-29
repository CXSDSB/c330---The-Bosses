using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum PlayerStyle { Aggressive, Defensive, Mobile }
    public PlayerStyle detectedStyle;

    public EnemyChase chase;
    public EnemyAttack attack;

    private float playerAggression;
    private float playerDistance;
    private float movementVariance;

    public Transform player;

    void Update()
    {
        if (player == null) return;

        // 收集数据
        float distance = Vector2.Distance(transform.position, player.position);
        movementVariance = Mathf.Lerp(movementVariance, Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.magnitude), 0.1f);

        playerDistance = Mathf.Lerp(playerDistance, distance, 0.1f);

        // 模拟攻击频率（你可以接你的player攻击事件）
        if (player.GetComponent<PlayerAttack>().IsAttacking)
            playerAggression += 0.1f;
        else
            playerAggression *= 0.98f; 

        ClassifyPlayer();
        ApplyBehavior();
    }

    void ClassifyPlayer()
    {
        if (playerAggression > 1.5f)
            detectedStyle = PlayerStyle.Aggressive;

        else if (playerDistance > 5f)
            detectedStyle = PlayerStyle.Defensive;

        else
            detectedStyle = PlayerStyle.Mobile;
    }

    void ApplyBehavior()
    {
        switch (detectedStyle)
        {
            case PlayerStyle.Aggressive:
                chase.chaseSpeed = 3.5f;
                attack.attackCooldown = 0.8f;
                break;

            case PlayerStyle.Defensive:
                chase.chaseSpeed = 2f;
                attack.attackCooldown = 1.5f;
                break;

            case PlayerStyle.Mobile:
                chase.chaseSpeed = 4f;
                break;
        }
    }
}
