using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{

    private Enemy parent;

    private float attackCoolDown = 3;

    private float extraAttackRange = .1f;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.MyRigidbody.velocity = Vector2.zero;
        parent.Direction = Vector2.zero;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (parent.MyAttactime >= attackCoolDown && !parent.IsAttacking)
        {
            parent.MyAttactime = 0;
            parent.StartCoroutine(Attack());
        }

        if(parent.MyTarget != null)
        {
            float distance = Vector2.Distance(parent.MyTarget.parent.position, parent.transform.parent.position);

            if(distance >= parent.MyAttackRange + extraAttackRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }
}
