using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable
{
    public void TakeDamage(int damage);

    public bool hasTakenDamage { get; set; }

    public void Die();

}
