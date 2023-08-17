using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DashAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    private float dashSpeed = 3f;
    [SerializeField]
    private float dashCoolDown = 1f;
    private AbilityType abilityType = AbilityType.OneTimeTrigger;
    public AbilityType AbilityType { get { return abilityType; } set { abilityType = value; } }
    public float Duration { get { return dashCoolDown; } set { dashCoolDown = value; } }

    private float dashCoolDownTimer = 0;
    private Animator animator;
    private UserInputData userInputData;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        userInputData = GetComponent<UserInputData>();
        if (userInputData != null)
        {
            userInputData.Abilities.Add(this);
        }
    }

    public void Execute()
    {
        if (dashCoolDownTimer <= 0)
        {
            animator.SetTrigger(name: "Dash");
            dashCoolDownTimer = dashCoolDown;
        }
    }

    public void Update()
    {
        if (dashCoolDownTimer > 0)
        {
            dashCoolDownTimer -= Time.deltaTime;
            transform.Translate(transform.forward * dashSpeed * Time.deltaTime, Space.World);
        }
    }
}
