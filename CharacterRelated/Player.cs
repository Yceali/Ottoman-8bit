
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : CharacterScript
{
    private static Player instance;

    private List<IInteractible> interactibles = new List<IInteractible>();

    public int MyGold { get; set; }
    [SerializeField]
    private Text goldText;

    [SerializeField]
    private Transform mapIndicator;

    public static Player m_instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;

            
        }
    }

    private List<Enemy> enemies = new List<Enemy>();

    public List<IInteractible> MyInteractibles { get => interactibles; set => interactibles = value; }
    public Stat MyXp { get => xp; set => xp = value; }
    public Stat MyStamina { get => stamina; set => stamina = value; }
    public List<Enemy> MyEnemies { get => enemies; set => enemies = value; }

    [SerializeField]
    private Stat stamina;
    private float initialStamina = 12;
    private Vector2 initialPos;

    [SerializeField]
    private Stat xp;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Animator lvlUp;


    [SerializeField]
    public SightBlocker[] blocks;
    private int directionIndex;
    private int throwableIndex;

    private Vector3 min, max;
    [SerializeField]
    private int playerAttackRange;
    private bool canDamage;
    [SerializeField]
    private Profession profession;
    public Coroutine MyInitRoutine { get; set; }

    // Update is called once per frame
    protected override void Update()
    {        
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
            Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        base.Update();

        if (MyTarget != null)
        {
            float distance = Vector2.Distance(MyTarget.position, transform.position);

            if (distance < playerAttackRange && !IsAttacking)
            {
                canDamage = true;
            }
            else
            {
                canDamage = false;
                
            }
        }
    }

   
    public IEnumerator ReSpawn()
    {
        MySpriteRenderer.enabled = false;
        yield return new WaitForSeconds(1.0f);
        health.Initialize(initialHealth, initialHealth);
        MyStamina.Initialize(initialStamina, initialStamina);
        transform.parent.position = initialPos;
        MySpriteRenderer.enabled = true;
        MyAnimator.SetTrigger("respawn");
    }

    public void SetDefaults()
    {
        health.Initialize(initialHealth, initialHealth);
        MyStamina.Initialize(initialStamina, initialStamina);
        MyGold = 1000;
        UpdateGoldText();
        MyXp.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
        initialPos = transform.parent.position;
    }
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue -= 4;
            MyStamina.MyCurrentValue -= 2;
            GainXp(16);
        }

        Direction = Vector2.zero;
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"]))
        {
            directionIndex = 0;
            Direction += Vector2.up;
            mapIndicator.eulerAngles = new Vector3(0, 0, 90);
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"]))
        {
            directionIndex = 2;
            Direction += Vector2.down;            
            mapIndicator.eulerAngles = new Vector3(0, 0, -90);
            
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"]))
        {
            directionIndex = 3;
            Direction += Vector2.left;
            if (Direction.y == 0)
            {
                mapIndicator.eulerAngles = new Vector3(0, 0, 180);
            }
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"]))
        {
            directionIndex = 1;
            Direction += Vector2.right;
            if (Direction.y == 0)
            {
                mapIndicator.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Block();
            if (MyTarget != null && !IsAttacking && !IsMoving && !IsThrowing && InlineOfSight()&& canDamage)
            {
                CharacterScript c = MyTarget.GetComponentInParent<CharacterScript>();
                c.TakeDamage(10,this.transform);
                actionRoutine = StartCoroutine(Attack());
            }
        }

        if (IsMoving)
        {
            StopAction();
            StopInit();
        }

        foreach (string action in KeybindManager.MyInstance.Actionbinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.Actionbinds[action]))
            {
                UIManager.m_instance.ClickActionButton(action);
            }
        }
    }

    public  void UpdateGoldText()
    {
        goldText.text = MyGold.ToString();
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
    public void AttackButton()
    {
        Block();
        if (MyTarget != null && !IsAttacking && !IsMoving && !IsThrowing && InlineOfSight())
        {
            actionRoutine = StartCoroutine(Attack());
        }
    }

    private IEnumerator ActionRoutine(ICastable castable)
    {
        SkillBook.m_instance.Cast(castable);

        IsThrowing = true;
        MyAnimator.SetBool("throwing", IsThrowing);

        yield return new WaitForSeconds(castable.MyCastTime);

        StopAction();
    }

    public IEnumerator Attack()
    {        
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking);
        yield return new WaitForSeconds(0.5f);
        StopAction();
    }

    public IEnumerator AttacRoutine(ICastable castable)
    {
        Transform currentTraget = MyTarget;


        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if(currentTraget != null && InlineOfSight())
        {
            Throwables newThrowable = SkillBook.m_instance.GetThrowable(castable.MyTitle);
            ThrowableSript t = Instantiate(newThrowable.MySpellPrefab, transform.position, Quaternion.identity).GetComponent<ThrowableSript>();
            t.Initialize(currentTraget,newThrowable.MyDamage, transform);
        }
        
        StopAction();
    }

    private IEnumerator GatherRoutine(ICastable castable , List<Drop> items)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));
        LootWindow.MyInstance.CreatePages(items);
    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));
        profession.AddItemsToInventory();
    }

    public void Throwables(ICastable castable)
    {
        Block();
        if (MyTarget != null && MyTarget.GetComponentInParent<Enemy>().IsAlive && !IsThrowing && !IsMoving && !IsThrowing && InlineOfSight())
        {
            MyInitRoutine = StartCoroutine(AttacRoutine(castable));
        }
    }

    public void Gather(ICastable castable, List<Drop> items)
    {
        if (!IsAttacking)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(castable, items));
        }
    }


    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
        }
    }
    private void Block()
    {
        foreach (SightBlocker b in blocks)
        {
            b.Deacvtivate();
        }

        blocks[directionIndex].Acvtivate();
    }

    private bool InlineOfSight()
    {

        if(MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);
            if (hit.collider == null)
            {
                return true;
            }
        }
             

        return false;
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!MyEnemies.Contains(enemy))
        {
            MyEnemies.Add(enemy);
        }
    }

    public void StopAction()
    {
        SkillBook.m_instance.StopCast();

        IsAttacking = false;
        MyAnimator.SetBool("attack", IsAttacking);
        IsThrowing = false;
        MyAnimator.SetBool("throwing", IsThrowing);

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
        }
    }


    public void GainXp(int x)
    {
        MyXp.MyCurrentValue += x;
        CombatTextManager.MyInstance.CreateText(transform.position, x.ToString(), SCTType.xp, false);

        if (MyXp.MyCurrentValue>= MyXp.MyMaxValue)
        {
            StartCoroutine(LvlUp());
        }
    }

    private IEnumerator LvlUp()
    {
        while (!MyXp.IsFull)
        {
            yield return null;
        }

        MyLevel++;
        lvlUp.SetTrigger("lvlup");
        levelText.text = MyLevel.ToString();
        MyXp.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyXp.MyMaxValue = Mathf.Floor(MyXp.MyMaxValue);
        MyXp.MyCurrentValue = MyXp.MyOverFlow;
        MyXp.Reset();

        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(LvlUp());
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            IInteractible interactible = collision.GetComponent<IInteractible>();

            if (!MyInteractibles.Contains(interactible))
            {
                MyInteractibles.Add(interactible);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (MyInteractibles.Count > 0)
            {
                IInteractible interactible = MyInteractibles.Find(x => x == collision.GetComponent<IInteractible>());
                if (interactible != null)
                {
                    interactible.StopInteract();
                }

                MyInteractibles.Remove(interactible);
            }            
        }
    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }
}
