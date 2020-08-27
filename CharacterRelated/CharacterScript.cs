using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class CharacterScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private string type;

    private Vector2 direction;
    [SerializeField]
    private Rigidbody2D rigidBody;

    protected Coroutine actionRoutine;
    public Animator MyAnimator { get; set; }

    public Transform MyCurrentTile { get; set; }
    public bool IsAttacking { get; set; }
    public bool IsThrowing { get; set; }
    public Stack<Vector3> MyPath { get; set; }

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    [SerializeField]
    private int level;

    public Transform MyTarget { get; set; }
    public Stat MyHealth
    {
        get { return health; }
    }
    [SerializeField]
    protected float initialHealth;
    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    public string MyType { get => type; }
    public int MyLevel { get => level; set => level = value; }

    public Rigidbody2D MyRigidbody { get => rigidBody; }

    public SpriteRenderer MySpriteRenderer { get; set; }

    protected virtual void Start()
    {
        MyAnimator = GetComponent<Animator>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    public void FixedUpdate()
    {
        Move();
    }


    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else if (IsThrowing)
            {
                ActivateLayer("ThrowLayer");
            }
            else
            {
                ActivateLayer("IdleLayer");
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
        }
        
    }

    public void Move()
    {
        if (MyPath == null)
        {
            if (IsAlive)
            {
                MyRigidbody.velocity = Direction.normalized * Speed;
            }
        }
        
    }


    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }


    public virtual void TakeDamage(float damage, Transform source)
    {        
        health.MyCurrentValue -= damage;
        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTType.damage, false);

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            MyRigidbody.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
        }                
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTType.heal,true);
    }
}
