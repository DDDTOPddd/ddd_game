using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;
using Unity.Burst.CompilerServices;

enum SnakeState
{
    Move,Win,Fly,Dead
}
public class SnakeHead : MonoBehaviour
{
    public int number;
    private SnakeState snakeState;
    //有关蛇身
    public GameObject snakeBodyPrefab; // 之前准备好的蛇身Prefab
    public List<GameObject> snakeBodies = new List<GameObject>(); // 蛇身列表
    [SerializeField]
    private List<Vector2> bodyPositions = new List<Vector2>(); // 用来存储蛇身移动位置的队列

    private  AudioSource audioSource;
    public AudioClip MoveAudio;
    public AudioClip ToHole;
    public AudioClip Boom;
    public AudioClip Fly;
    public AudioClip DropAudio;

    public GameObject WholeSnake;
    public float moveDistance = 1.0f; // 移动的格子大小
    public float moveSpeed = 0.1f; // 两次移动之间的时间间隔
    private float moveTimer;
    private float WaitTimer = 0;
    // 蛇头各个方向的Sprite
    public Sprite upSprite_f;
    public Sprite downSprite_f;
    public Sprite leftSprite_f;
    public Sprite rightSprite_f;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    //蛇身的Sprite
    public Sprite tailr;
    public Sprite taild;
    public Sprite turn1;
    public Sprite taill;
    public Sprite straight_lr;
    public Sprite turn2;
    public Sprite tailu;
    public Sprite turn3;
    public Sprite straight_ud;
    public Sprite turn4;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Rigidbody2D q, n, h;

    public Tilemap groundTilemap;
    
    private int key = 0;
    bool IsDead ;

    public LayerMask stone;
    public LayerMask food;
    private LayerMask combinedLayerMask;
    Vector2 moveDirection;
    public  bool canMove;
    void Start()
    {
        
        audioSource = gameObject.GetComponent<AudioSource>();
        snakeState = SnakeState.Move;
        moveTimer = 0;
        combinedLayerMask = food | stone;
        IsDead = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = rightSprite;
        Vector2 initialPosition = new Vector2(0.5f, 0.5f);
        rb.position = initialPosition;
        canMove = true;
        for (int i = 0; i < number; i++)
        {
            Vector2 bodyPosition = initialPosition - new Vector2((i + 1) * moveDistance, 0);
            bodyPositions.Add(bodyPosition);
            Grow(bodyPosition);
        }
    }
    private void Update()
    {
        switch (snakeState)
        {
            case SnakeState.Move:
                MoveUpdate();
                break;
            case SnakeState.Win:
                WinUpdate();
                break;
            case SnakeState.Fly:
                FlyUpdate();
                break;
            case SnakeState.Dead:
                DeadUpdate();
                break;

        }
    }
    void MoveUpdate()
    {
        moveTimer += Time.deltaTime;
        change(key);
        if (moveTimer >= moveSpeed && canMove)
        {
            
            Vector2 oldPosition = rb.position;
            Vector2 newPosition = rb.position;
            Sprite newSprite = spriteRenderer.sprite;
            Sprite fnewSprite = spriteRenderer.sprite;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                newPosition.y += moveDistance;
                newSprite = upSprite;
                fnewSprite = upSprite_f;
                moveDirection = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                newPosition.y -= moveDistance;
                newSprite = downSprite;
                fnewSprite = downSprite_f;
                moveDirection = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                newPosition.x -= moveDistance;
                newSprite = leftSprite;
                fnewSprite = leftSprite_f;
                moveDirection = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                newPosition.x += moveDistance;
                newSprite = rightSprite;
                fnewSprite = rightSprite_f;
                moveDirection = Vector2.right;
            }
            
         


            if (newPosition != rb.position)
            {
                int a = CanPushTo(transform.position, moveDirection);
                if(a==1)
                {
                    audioSource.clip = MoveAudio;
                    audioSource.Play();
                    moveTimer = 0f;
                    MoveBody(oldPosition);
                    rb.MovePosition(newPosition); 
                    spriteRenderer.sprite = newSprite;
                    if (!HeadIsOnGround())
                    {
                        audioSource.clip = DropAudio;
                        audioSource.Play();
                        IsDead = true;
                        snakeState = SnakeState.Dead;
                        WaitTimer = 0;

                    }
                }
                else if (a == 2)
                {
                    audioSource.clip = MoveAudio;
                    audioSource.Play();
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1f, food);
                    Banana banana = hit.collider.GetComponent<Banana>();
                    if (banana != null)
                    {
                        int choose =hit.collider.GetComponent<Banana>().BeEat();
                        switch (choose)
                        {
                            //banana
                            case 1:
                                FoodManager.Instance.RemoveFood();
                                GameObject body = Instantiate(snakeBodyPrefab, rb.position, Quaternion.identity);
                                body.transform.SetParent(WholeSnake.transform);
                                snakeBodies.Insert(0, body);
                                bodyPositions.Insert(0, body.transform.position);
                                rb.MovePosition(newPosition);
                                spriteRenderer.sprite = newSprite;
                                break;
                            //辣椒
                            case 2:
                                canMove = false;
                                FoodManager.Instance.RemoveFood();
                                moveTimer = 0f;
                                MoveBody(oldPosition);
                                rb.MovePosition(newPosition);
                                
                                spriteRenderer.sprite = newSprite;
                                spriteRenderer.sprite = fnewSprite;
                                ToFly();
                                Vector2 FlyDir = -moveDirection;
                                StartCoroutine(FlyEffect(FlyDir,newSprite));
                                
                                //ToMove();
                                break;
                        }
                        

                    }
                    
                }
            }
            


        }
        if (!HeadIsOnGround())
        {
            audioSource.clip = DropAudio;
            audioSource.Play();
            IsDead = true;
            snakeState = SnakeState.Dead;
            WaitTimer = 0;

        }
        moveDirection = Vector2.zero;
    }
    void DeadUpdate()
    {
        change(key);
        if (IsDead)
        {
            WaitTimer += Time.deltaTime;
            if (WaitTimer > 0.5)
            {
                key = 1;
                WholeSnake.GetComponent<WholeSnake>().LinearTo();

                //UIController.Instance.Dead();
            }
            
        }
    }

    public void ToMove()
    {
        snakeState = SnakeState.Move;
        //canMove = true;
    }
    private void MoveBody(Vector2 headOldPosition)
    {
        bodyPositions.Insert(0, headOldPosition);

        if (bodyPositions.Count > snakeBodies.Count)
        {
            bodyPositions.RemoveAt(bodyPositions.Count - 1);
        }
        for (int i = 0; i < snakeBodies.Count; i++)
        {
            snakeBodies[i].transform.position = bodyPositions[i];
        }
    }
    public void Grow(Vector2 positionToGrow)
    {
        GameObject body = Instantiate(snakeBodyPrefab, positionToGrow, Quaternion.identity);
        body.transform.SetParent(WholeSnake.transform);
        snakeBodies.Add(body);
    }
    void change(int key)
    {
        if (key == 1) return;
        for (int i = 0; i < snakeBodies.Count; i++)
        {
            SpriteRenderer sr = snakeBodies[i].GetComponent<SpriteRenderer>();
            if (i == 0)//第一节
            {
                q = GetComponent<Rigidbody2D>();
                n = snakeBodies[0].GetComponent<Rigidbody2D>();
                h = snakeBodies[1].GetComponent<Rigidbody2D>();
                if (q.position.x == h.position.x)
                {
                    sr.sprite = straight_ud;
                }
                else if (q.position.y == h.position.y)
                {
                    sr.sprite = straight_lr;
                }
                else
                {
                    if (q.position.x > h.position.x && q.position.y > h.position.y && n.position.x < q.position.x)
                    {
                        sr.sprite = turn1;
                    }
                    else if (q.position.x < h.position.x && q.position.y < h.position.y && n.position.x < h.position.x)
                    {
                        sr.sprite = turn1;
                    }
                    else if (q.position.x < h.position.x && q.position.y > h.position.y && n.position.x > q.position.x)
                    {
                        sr.sprite = turn2;
                    }
                    else if (q.position.x > h.position.x && q.position.y < h.position.y && n.position.x > h.position.x)
                    {
                        sr.sprite = turn2;
                    }
                    else if (q.position.x > h.position.x && q.position.y < h.position.y && n.position.x < q.position.x)
                    {
                        sr.sprite = turn3;
                    }
                    else if (q.position.x < h.position.x && q.position.y > h.position.y && n.position.x < h.position.x)
                    {
                        sr.sprite = turn3;
                    }
                    else
                    {
                        sr.sprite = turn4;
                    }
                }
            }
            else if (i == snakeBodies.Count - 1)//尾
            {
                q = snakeBodies[i - 1].GetComponent<Rigidbody2D>();
                n = snakeBodies[i].GetComponent<Rigidbody2D>();
                if (q.position.x > n.position.x && q.position.y == n.position.y)
                {
                    sr.sprite = tailr;
                }
                else if (q.position.x < n.position.x && q.position.y == n.position.y)
                {
                    sr.sprite = taill;
                }
                else if (q.position.x == n.position.x && q.position.y > n.position.y)
                {
                    sr.sprite = tailu;
                }
                else
                {
                    sr.sprite = taild;
                }
            }
            else
            {
                q = snakeBodies[i - 1].GetComponent<Rigidbody2D>();
                n = snakeBodies[i].GetComponent<Rigidbody2D>();
                h = snakeBodies[i + 1].GetComponent<Rigidbody2D>();
                if (q.position.x == h.position.x)
                {
                    sr.sprite = straight_ud;
                }
                else if (q.position.y == h.position.y)
                {
                    sr.sprite = straight_lr;
                }
                else
                {
                    if (q.position.x > h.position.x && q.position.y > h.position.y && n.position.x < q.position.x)
                    {
                        sr.sprite = turn1;
                    }
                    else if (q.position.x < h.position.x && q.position.y < h.position.y && n.position.x < h.position.x)
                    {
                        sr.sprite = turn1;
                    }
                    else if (q.position.x < h.position.x && q.position.y > h.position.y && n.position.x > q.position.x)
                    {
                        sr.sprite = turn2;
                    }
                    else if (q.position.x > h.position.x && q.position.y < h.position.y && n.position.x > h.position.x)
                    {
                        sr.sprite = turn2;
                    }
                    else if (q.position.x > h.position.x && q.position.y < h.position.y && n.position.x < q.position.x)
                    {
                        sr.sprite = turn3;
                    }
                    else if (q.position.x < h.position.x && q.position.y > h.position.y && n.position.x < h.position.x)
                    {
                        sr.sprite = turn3;
                    }
                    else
                    {
                        sr.sprite = turn4;
                    }
                }
            }
        }
    }
    private bool HeadIsOnGround()
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(rb.position);
        if (groundTilemap.HasTile(gridPosition)) return true;
        for(int i = 0; i < bodyPositions.Count; i++)
        {
            gridPosition = groundTilemap.WorldToCell(bodyPositions[i]);
            if (groundTilemap.HasTile(gridPosition)) return true;
        }
        return false;
    }

    int CanPushTo(Vector2 position ,Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, moveDirection, 1f, combinedLayerMask);
        if (!hit)
        {
            return 1;
        }
        else
        {
            if (hit.collider.GetComponent<Tui>() != null)
            {
                return hit.collider.GetComponent<Tui>().CanMoveTo(dir);
            }      
        }
        print("no banana but!");
        return 0;

    }

    void WinUpdate()
    {
        change(key);
    }
    public void ToWin()
    {
        snakeState = SnakeState.Win;
    }
    //进洞
    void OnTriggerEnter2D(Collider2D other)
    {
        if (snakeState == SnakeState.Move)
        {
            if (other.CompareTag("hole"))
            {
                Hole hole = other.gameObject.GetComponent<Hole>();
                if (hole != null && hole.IsOpen())
                {
                    print("win");
                    ToWin();
                    audioSource.clip = ToHole;
                    audioSource.Play();
                    StartCoroutine(EnterHoleEffect(hole));
                }
            }
            if (other.CompareTag("sand"))
            {
                Sand sand = other.gameObject.GetComponent<Sand>();
                if (sand != null && snakeState == SnakeState.Move)
                {
                    print("Over");
                    sand.Tobig();
                    //********
                    ToWin();
                    audioSource.clip = ToHole;
                    audioSource.Play();
                    StartCoroutine(EnterHoleEffect(sand));

                }
            }
        }
        
    }
    IEnumerator EnterHoleEffect(Sand hole)
    {
        Vector3 holePosition = hole.transform.position;

        spriteRenderer.enabled = false;
        MoveBody(holePosition);
        for (int i = 0; i < snakeBodies.Count; i++)
        {
            snakeBodies[i].GetComponent<SpriteRenderer>().enabled = false;
            for (int j = (snakeBodies.Count) - 1; j > 0; j--)
            {
                snakeBodies[j].transform.position = snakeBodies[j - 1].transform.position;
            }
            yield return new WaitForSeconds(0.1f);
        }
        key = 1;
        yield return new WaitForSeconds(0.5f);
        UIController.Instance.Dead();
    }
    IEnumerator EnterHoleEffect(Hole hole)
    {
        Vector3 holePosition = hole.transform.position;
        
        spriteRenderer.enabled = false;
        MoveBody(holePosition);
        for(int i = 0; i < snakeBodies.Count; i++)
        {
            snakeBodies[i].GetComponent<SpriteRenderer>().enabled = false;
            for(int j = (snakeBodies.Count) - 1; j > 0; j--)
            {
                snakeBodies[j].transform.position = snakeBodies[j - 1].transform.position;
            }
            yield return new WaitForSeconds(0.1f);
        }
        key = 1;
        yield return new WaitForSeconds(0.5f);
        UIController.Instance.Win();
    }

    void FlyUpdate()
    {

    }
    public void ToFly()
    {
        snakeState = SnakeState.Fly;
    }

    IEnumerator FlyEffect(Vector2 flyDirection, Sprite t)
    {
        change(0);
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        bool blocked=false;

        List<Vector2> nextBodyPositions = new List<Vector2>();
        for (int i = 0; i < bodyPositions.Count; i++)
        {
            nextBodyPositions.Add(bodyPositions[i] );
        }
        RaycastHit2D hitf = Physics2D.Raycast(rb.position - flyDirection * 0.5f, -flyDirection, 0.5f, food);
        if (hitf.collider != null)
        {
            audioSource.clip = Boom;
            audioSource.Play();
            if (hitf.collider.GetComponent<Ice>() != null || hitf.collider.GetComponent<Wood>() != null)
            {
                Destroy(hitf.collider.gameObject);
            }
        }
        audioSource.clip = Fly;
        audioSource.Play();
        while (!blocked)
        {    
            for (int i = 0; i < nextBodyPositions.Count; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(nextBodyPositions[i] + flyDirection * 0.5f, flyDirection, 0.5f, stone);
                hitf = Physics2D.Raycast(nextBodyPositions[i] + flyDirection * 0.5f, flyDirection, 0.5f, food);
                if (hit.collider != null && hit.collider.GetComponent<SnakeBody>() == null)
                {
                    blocked = true;
                    canMove = true;
                    audioSource.Stop();
                    spriteRenderer.sprite = t;
                    ToMove();
                    yield break;
                }
                if (hitf )
                {
                    if(hitf.collider.GetComponent<Tui>() != null)
                    {
                        int s=hitf.collider.GetComponent<Tui>().CanMoveTo(flyDirection);
                        if (s == 2)
                        {
                            blocked = true;
                            canMove = true;
                            audioSource.Stop();
                            spriteRenderer.sprite = t;
                            ToMove();
                            yield break;
                        }
                    }
                }              
            }
            for (int i = 0; i < nextBodyPositions.Count; i++)
            {
                nextBodyPositions[i]+= flyDirection * moveDistance;
            }
            Vector2 nextHeadPosition = rb.position + flyDirection * 1;
            yield return new WaitForSeconds(0.1f);
            rb.position = nextHeadPosition;
            for (int i = 0; i < snakeBodies.Count; i++)
            {
                snakeBodies[i].transform.position = nextBodyPositions[i];

            }
            bodyPositions = new List<Vector2>(nextBodyPositions);
            
            
            

        }

        //audioSource.Stop();
        //canMove = true;
        //yield return new WaitForSeconds(1);
        //spriteRenderer.sprite = t;
        //yield return new WaitForSeconds(1);
        
       
    }
}