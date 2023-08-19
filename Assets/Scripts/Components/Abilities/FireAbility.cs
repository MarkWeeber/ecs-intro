using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using System.Linq;

public class FireAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    private float fireCoolDown = 0.52f;
    [SerializeField]
    private GameObject simpleProjectilePrefab;
    [SerializeField]
    private GameObject specialprojectilePrefab;
    [SerializeField]
    private int maxProjectileAmount = 150;
    [SerializeField]
    private Transform firePort;
    public bool SpecialFire = false;
    private bool fireInstatiate = false;
    private List<Projectile> simpleProjectiles;
    private List<Projectile> specialProjectiles;
    private Animator animator;

    private float _fireCoolDown = 0.52f;
    public float Duration { get { return _fireCoolDown; } set { _fireCoolDown = value; } }
    private AbilityType abilityType;
    public AbilityType AbilityType { get { return abilityType; } set { abilityType = value; } }

    private UserInputData userInputData;
    private Projectile _sampleProjectile;
    private IEnumerator<Projectile> _simpleEm;
    private IEnumerator<Projectile> _specialEm;
    private int _simpleIndex = 0;
    private int _specialIndex = 0;
    private GameObject _simpleGameObject;
    private bool _simpleProjectilesDepleted = false;

    private void Awake()
    {
        _fireCoolDown = fireCoolDown;
        simpleProjectiles = new List<Projectile>();
        specialProjectiles = new List<Projectile>();
        StashProjectiles();
        _simpleEm = simpleProjectiles.GetEnumerator();
        _specialEm = specialProjectiles.GetEnumerator();
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
            if (SpecialFire)
            {
                CirculateProjectilesSend1(_specialEm);
            }
            else
            {
                CirculateProjectilesSend1(_simpleEm);
            }
        }
    }

    private void StashProjectiles()
    {
        for (int i = 0; i < maxProjectileAmount; i++)
        {
            _sampleProjectile = GameObject.Instantiate(simpleProjectilePrefab, firePort.position, firePort.rotation).GetComponent<Projectile>();
            simpleProjectiles.Add(_sampleProjectile);
            _sampleProjectile = GameObject.Instantiate(specialprojectilePrefab, firePort.position, firePort.rotation).GetComponent<Projectile>();
            specialProjectiles.Add(_sampleProjectile);
        }
    }

    private void CirculateProjectilesSend1(IEnumerator<Projectile> em)
    {
        if (em.MoveNext())
        {
            _sampleProjectile = em.Current;
        }
        else
        {
            em.Reset();
            em.MoveNext();
            _sampleProjectile = em.Current;
        }
        if (_sampleProjectile != null)
        {
            _sampleProjectile.Enable(firePort.transform);
            animator.SetTrigger(name: "Fire");
        }
        else
        {
            Debug.Log("c2");
        }        
    }

    private void CirculateProjectilesSend()
    {
        if (SpecialFire)
        {
            if (_simpleProjectilesDepleted)
            {
                _fireCoolDown = fireCoolDown;
            }
            if (_specialIndex >= maxProjectileAmount)
            {
                _specialIndex = 0;
            }
            _sampleProjectile = specialProjectiles.ElementAt(_specialIndex);
            if (_sampleProjectile != null)
            {
                ActivateProjectile(_sampleProjectile);
                _specialIndex++;
            }
        }
        else
        {
            if (_simpleProjectilesDepleted)
            {
                _fireCoolDown = 0;
                return;
            }
            if (_simpleIndex >= maxProjectileAmount)
            {
                _simpleIndex = 0;
            }
            _sampleProjectile = simpleProjectiles.ElementAt(_simpleIndex);
            if (_sampleProjectile != null)
            {
                ActivateProjectile(_sampleProjectile);
                _simpleIndex++;
            }
            else
            {
                _simpleProjectilesDepleted = true;
                for (int i = 0; i < maxProjectileAmount; i++)
                {
                    _sampleProjectile = simpleProjectiles.ElementAt(i);
                    if (_sampleProjectile != null)
                    {
                        ActivateProjectile(_sampleProjectile);
                        _simpleProjectilesDepleted = false;
                        _simpleIndex = i + 1;
                        break;
                    }
                }
            }
        }

    }

    private void ActivateProjectile(Projectile projectile)
    {
        projectile.Enable(firePort.transform);
        animator.SetTrigger(name: "Fire");
    }

    private void SendProjectie()
    {
        _simpleGameObject = GameObject.Instantiate(simpleProjectilePrefab, firePort.position, firePort.rotation);
        _sampleProjectile = _simpleGameObject.GetComponent<Projectile>();
        if (_sampleProjectile != null)
        {
            _sampleProjectile.Enable(firePort);
        }
        animator.SetTrigger(name: "Fire");
        _sampleProjectile = null;
    }

    private void _ConvertToEntity()
    {
        Entity prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(simpleProjectilePrefab, GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore()));
        Entity newEntity = World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(prefabEntity);
        World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(newEntity, new Translation{Value = firePort.position});
    }
}
