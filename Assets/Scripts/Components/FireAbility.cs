using System.Collections.Generic;
using UnityEngine;

public class FireAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    private float fireCoolDown = 0.52f;
    [SerializeField]
    private Transform projectilePrefab;
    [SerializeField]
    private int maxProjectileAmount = 20;
    private List<Projectile> projectiles;
    private Animator animator;

    public float Duration { get { return fireCoolDown; } set { fireCoolDown = value; } }

    private UserInputData userInputData;
    private Projectile _sampleProjectile;
    private IEnumerator<Projectile> _em;

    private void Awake()
    {
        projectiles = new List<Projectile>();
        StashProjectiles();
        _em = projectiles.GetEnumerator();
        animator = GetComponentInParent<Animator>();
        userInputData = GetComponentInParent<UserInputData>();
        if (userInputData != null)
        {
            userInputData.Abilities.Add(this);
        }
    }

    public void Execute()
    {
        if (_em.MoveNext())
        {
            _sampleProjectile = _em.Current;
        }
        else
        {
            _em.Reset();
            _em.MoveNext();
            _sampleProjectile = _em.Current;
        }
        _sampleProjectile.Enable(transform);
        animator.SetTrigger(name: "Fire");
    }

    private void StashProjectiles()
    {
        for (int i = 0; i < maxProjectileAmount; i++)
        {
            _sampleProjectile = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectiles.Add(_sampleProjectile);
        }
    }
}
