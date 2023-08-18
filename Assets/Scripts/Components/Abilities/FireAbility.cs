using System.Collections.Generic;
using UnityEngine;

public class FireAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    private float fireCoolDown = 0.52f;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private int maxProjectileAmount = 20;
    [SerializeField]
    private Transform firePort;
    [SerializeField]
    private bool fireInstatiate = false;
    private List<Projectile> projectiles;
    private Animator animator;

    public float Duration { get { return fireCoolDown; } set { fireCoolDown = value; } }
    private AbilityType abilityType;
    public AbilityType AbilityType { get { return abilityType; } set { abilityType = value; } }

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
        if (fireInstatiate)
        {
            SendProjectie();
        }
        else
        {
            CirculateProjectilesSend();
        }
    }

    private void StashProjectiles()
    {
        for (int i = 0; i < maxProjectileAmount; i++)
        {
            _sampleProjectile = GameObject.Instantiate(projectilePrefab, firePort.position, firePort.rotation).GetComponent<Projectile>();
            projectiles.Add(_sampleProjectile);
        }
    }

    private void CirculateProjectilesSend()
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
        _sampleProjectile.Enable(firePort.transform);
        animator.SetTrigger(name: "Fire");
    }

    private void SendProjectie()
    {
        var _go = GameObject.Instantiate(projectilePrefab, firePort.position, firePort.rotation);
        _sampleProjectile = _go.GetComponent<Projectile>();
        if (_sampleProjectile != null)
        {
            _sampleProjectile.Enable(firePort);
        }
        animator.SetTrigger(name: "Fire");
        _sampleProjectile = null;
    }
}
