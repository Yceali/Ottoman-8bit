using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public delegate void HealthChanged(float health);
public delegate void CharacterRemoved();
public class Enemy : CharacterScript, IInteractible
{
    public event HealthChanged healthChanged;
    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private CanvasGroup healthGroup;


    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    public float MyAttackRange { get; set; }

    public float MyAttactime { get; set; }

    [SerializeField]
    private float initalAggroRange;
    public float MyAggroRange { get; set; }

    [SerializeField]
    private LayerMask losMask;

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait { get => portrait; }

    [SerializeField]
    private AStar astar;

    [SerializeField]
    private int damage; //temp

    private bool canDoDamage = true;

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    public AStar MyAstar { get => astar;}

    protected void Awake()
    {
        health.Initialize(initialHealth, initialHealth);
        MyStartPosition = transform.parent.position;
        MyAttackRange = 1;
        ChangeState(new IdleState());
    }
    protected override void Update()
    {
        //Debug.Log(InRange);
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttactime += Time.deltaTime;
            }

            currentState.Update();

            if (MyTarget != null && !Player.m_instance.IsAlive)
            {
                ChangeState(new EvadeState());

            }
        }
        base.Update();
        
    }

    public Transform Select()
    {
        healthGroup.alpha = 1;
        return hitBox; 
    }

    public void DeSelect()
    {
        healthGroup.alpha = 0;
        healthChanged -= new HealthChanged(UIManager.m_instance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.m_instance.HideTargetFrame);
    }


    public override void TakeDamage(float damage, Transform source)
    {
        if(!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);
                base.TakeDamage(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (!IsAlive)
                {
                    Player.m_instance.MyEnemies.Remove(this);
                    Player.m_instance.GainXp(XpManager.CalculateXp(this as Enemy));                    
                }
            }
            
        }
    }

    public void DoDamage()
    {
        if (canDoDamage)
        {
            Player.m_instance.TakeDamage(damage, transform);
            canDoDamage = false;
        }
        
    }

    public void CanDoDamage()
    {
        canDoDamage = true;
    }
    public void ChangeState(IState newState)
    {
        
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
        Debug.Log(currentState);
    }

    public void SetTarget(Transform target)
    {
        if(MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initalAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void ResetEnemy()
    {
        this.MyTarget = null;
        this.MyAggroRange = initalAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();

            foreach (IInteractible interactible in Player.m_instance.MyInteractibles)
            {
                if (interactible is Enemy && !(interactible as Enemy).IsAlive)
                {
                    drops.AddRange((interactible as Enemy).lootTable.GetLoot());
                }
            }

            LootWindow.MyInstance.CreatePages(drops);
        }
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.Close();      
    }


    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }
        Destroy(gameObject);
    }

    public bool CanSeePlayer()
    {
        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), losMask);
        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }
}
