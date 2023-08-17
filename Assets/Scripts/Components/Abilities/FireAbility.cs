using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FireAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    private float fireCoolDown = 0.52f;
    [SerializeField]
    private Transform projectilePrefab;
    [SerializeField]
    private Transform firePort;
    private Animator animator;

    public float Duration { get { return fireCoolDown; } set { fireCoolDown = value; } }

    private AbilityType abilityType = AbilityType.CharacterAbility;
    public AbilityType AbilityType { get { return abilityType; } set { abilityType = value; } }

    private UserInputData userInputData;
    private Projectile _sampleProjectile;
    private IEnumerator<Projectile> _em;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        userInputData = GetComponentInParent<UserInputData>();
        if (userInputData != null)
        {
            userInputData.Abilities.Add(this);
        }
    }

    public void Execute()
    {
        SendProjectie();
    }

    private void SendProjectie()
    {
        _sampleProjectile = GameObject.Instantiate(projectilePrefab, firePort.position, Quaternion.identity).GetComponent<Projectile>();
        _sampleProjectile.Enable(firePort);
        animator.SetTrigger(name: "Fire");
        _sampleProjectile = null;
    }
}