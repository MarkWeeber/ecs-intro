using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FireAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    private float fireCoolDown = 0.52f;
    [SerializeField]
    private Transform projectilePrefab;

    public float Duration { get { return fireCoolDown; } set { fireCoolDown = value; } }

    private Transform _sampleProjectile;
    private UserInputData userInputData;

    private void Awake()
    {
        userInputData = GetComponent<UserInputData>();
        if (userInputData != null)
        {
            userInputData.Abilities.Add(this);
        }
    }

    public void Execute()
    {
        _sampleProjectile = GameObject.Instantiate(projectilePrefab);
        _sampleProjectile.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(transform.forward));
    }
}
