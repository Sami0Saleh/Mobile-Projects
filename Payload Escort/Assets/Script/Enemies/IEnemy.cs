using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    static public List<IEnemy> EnemyList = new List<IEnemy>();
    Transform transform { get; }
    GameObject gameObject { get; }
    void DetectTarget();
    void MoveTowardsPlayer();
    void Die();
    void SetPlayer(PlayerController player);
    void SetPlayerStats(PlayerStats playerStats);
    void SetPlayerTransform(Transform playerTransform);
    void SetPayloadTarget(Transform payloadTransform);
}
