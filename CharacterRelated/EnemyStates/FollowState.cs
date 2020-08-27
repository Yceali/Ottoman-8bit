 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        Player.m_instance.AddAttacker(parent);
        this.parent = parent;
        parent.MyPath = null; 
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.MyRigidbody.velocity = Vector2.zero;
    }

    public void Update()
    {
        if(parent.MyTarget != null)
        {
            parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;

            float distance = Vector2.Distance(parent.MyTarget.position , parent.transform.position);

            if(distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        }
        else if (!parent.CanSeePlayer())
        {
            parent.ChangeState(new PathState());
        }
    }
}
