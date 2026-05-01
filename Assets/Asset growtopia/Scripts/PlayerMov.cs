using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;

    public float horizontal;
    private Rigidbody2D rb;

    public TerrainGeneration terrainGeneration;

    private Vector2 mousePos;

    // 🔹 Range kotak
    public float rangeX = 5f;
    public float rangeY = 5f;

    // 🔹 Highlight
    public GameObject gridHighlight;

    // 🔹 Mining delay
    public float mineRate = 0.3f;
    private float mineTimer;

    public float placeRate = 0.15f;
    private float placeTimer;

    public HotbarUI hotbarUI;

    private Animator anim;
    public bool hit;
    public bool place;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 snappedMouse = new Vector2(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y)
        );

        UpdateHighlight(snappedMouse);

        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("hit", hit);
        anim.SetBool("place", place);

        
    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        FlipPlayer();

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        Vector2 snappedMouse = new Vector2(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y)
        );

         
        mineTimer -= Time.deltaTime;

        
        hit = Input.GetMouseButton(0);
        if (hit && InRange(snappedMouse) && mineTimer <= 0f)
        {
            terrainGeneration.RemoveTile(snappedMouse);
            mineTimer = mineRate;
        }

        //   PLACE BLOCK
        placeTimer -= Time.deltaTime;
        place = Input.GetMouseButton(1);
        if (place && InRange(snappedMouse))
        {
            string itemName = hotbarUI.GetSelectedItem();

            if (itemName != "")
            {
                Sprite selectedSprite =
                    ItemIconDatabase.instance.GetIcon(itemName);

                bool placed =
                    terrainGeneration.PlaceTile(itemName, snappedMouse);
                if (placed)
                {
                    Inventory.instance.RemoveItem(itemName);
                }
            }
        }
        // 🔹 JUMP (logic asli lu)
        if (jump > 0.1f && onGround)
        {
            movement.y = jumpForce;
        }

        rb.linearVelocity = movement;
    }

    //   CEK RANGE
    bool InRange(Vector2 targetPos)
    {
        Vector2 playerPos = transform.position;

        return Mathf.Abs(targetPos.x - playerPos.x) <= rangeX &&
               Mathf.Abs(targetPos.y - playerPos.y) <= rangeY;
    }

    // 🔹 HIGHLIGHT SYSTEM
    void UpdateHighlight(Vector2 snappedMouse)
    {
        if (gridHighlight == null) return;

        gridHighlight.transform.position = snappedMouse;

        SpriteRenderer sr = gridHighlight.GetComponent<SpriteRenderer>();

        RaycastHit2D hit = Physics2D.Raycast(snappedMouse, Vector2.zero);

        bool inRange = InRange(snappedMouse);

        if (!inRange)
        {
            sr.color = new Color(1, 0, 0, 0.3f); // merah
        }
        else if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            sr.color = new Color(1, 1, 0, 0.3f); // kuning
        }
        else
        {
            sr.color = new Color(0, 1, 0, 0.3f); // hijau
        }

        // animasi biar hidup
        gridHighlight.transform.localScale =
            Vector3.one * (1 + Mathf.Sin(Time.time * 10f) * 0.05f);
    }

    //   GROUND DETECTION (VERSI LU — STABIL)
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    onGround = true;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    onGround = true;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    //   DEBUG RANGE
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(rangeX * 2, rangeY * 2, 0));
    }

   void FlipPlayer()
    {
        if (horizontal > 0.1f)
        {
           transform.localScale = new Vector3(-4, 4, 4);
        }
        else if (horizontal < -0.1f)
        {
            
            transform.localScale = new Vector3(4, 4, 4);
        }
    }
}