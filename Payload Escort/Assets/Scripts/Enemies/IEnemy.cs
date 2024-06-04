using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void DetectTarget();
    void MoveTowardsPlayer();
    void GotHit(int damage);
    void Die();
    void SetPlayer(PlayerController player);
    void SetPlayerTransform(Transform playerTransform);
    void SetPayloadTarget(Transform payloadTransform);
}
