using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(CharacterScript character);

public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    private Camera mainCamera;

    private static GameManager instance;

    [SerializeField]
    private Player player;

    private Enemy currentTarget;
    private int targetIndex;

    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    public static GameManager MyInstance 
    { 
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public HashSet<Vector3Int> Blocked { get => blocked; set => blocked = value; }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        NextTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,512);

            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                DeSelectTarget();

                SelectTarget(hit.collider.GetComponent<Enemy>());
            }
            else
            {
                UIManager.m_instance.HideTargetFrame();

                DeSelectTarget();

                currentTarget = null;
                player.MyTarget = null;
            }
        }

        else if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null)
            {
                IInteractible entity = hit.collider.gameObject.GetComponent<IInteractible>();
                if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractibles.Contains(entity))
                {
                    entity.Interact();
                }
            }
            
        }

    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeSelectTarget();
            if (Player.m_instance.MyEnemies.Count > 0)
            {
                if (targetIndex < Player.m_instance.MyEnemies.Count)
                {
                    SelectTarget(Player.m_instance.MyEnemies[targetIndex]);
                    targetIndex++;
                    if (targetIndex >= Player.m_instance.MyEnemies.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0;
                }
                
            }
        }
    }

    private void DeSelectTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }

    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UIManager.m_instance.ShowTargetFrame(currentTarget);
    }

    public void OnKillConfirmed(CharacterScript character)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }
}
