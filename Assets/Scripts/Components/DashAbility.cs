using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DashAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    private float dashSpeed = 3f;
    [SerializeField]
    private float dashCoolDown = 1f;
    private float dashCoolDownTimer = 0;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
