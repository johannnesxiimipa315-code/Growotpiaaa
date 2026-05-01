using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public bool onGround;

    public float horizontal;
    private float jumpInput;

    private Rigidbody2D rb;
    private Animator anim;

    public TerrainGeneration terrainGeneration;
    public HotbarUI hotbarUI;

    private Vector2 mousePos;

    // 🔹 Range
    public float rangeX = 5f;
    public float rangeY = 5f;

    // 🔹 Highlight
    public GameObject gridHighlight;

    // 🔹 Mining
    public float mineRate = 0.3f;
    private float mineTimer;

    // 🔹 Place
    public float placeRate = 0.15f;
    private float placeTimer;

    public bool hit;
    public bool place;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetAxisRaw("Jump");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 snappedMouse = new Vector2(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y)
        );

        UpdateHighlight(snappedMouse);

        // 🔹 Mining input
        hit = Input.GetMouseButton(0);

        // 🔹 Place input
        place = Input.GetMouseButton(1);

        // 🔹 Animator
        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetBool("hit", hit);
        anim.SetBool("place", place);
    }

    void FixedUpdate()
    {
        FlipPlayer();

        Vector2 movement = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );

         
        if (jumpInput > 0.1f && onGround)
        {
            movement.y = jumpForce;
            jumpInput = 0f;
        }

        rb.linearVelocity = movement;

        Vector2 snappedMouse = new Vector2(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y)
        );

        // 🔹 Mining Timer
        mineTimer -= Time.fixedDeltaTime;

        if (hit && InRange(snappedMouse) && mineTimer <= 0f)
        {
            terrainGeneration.RemoveTile(snappedMouse);
            mineTimer = mineRate;
        }

        // 🔹 Place Timer
        placeTimer -= Time.fixedDeltaTime;

        if (place && InRange(snappedMouse) && placeTimer <= 0f)
        {
            string itemName = hotbarUI.GetSelectedItem();

            if (itemName != "")
            {
                bool placed =
                    terrainGeneration.PlaceTile(itemName, snappedMouse);

                if (placed)
                {
                    Inventory.instance.RemoveItem(itemName);
                    placeTimer = placeRate;
                }
            }
        }
    }

    bool InRange(Vector2 targetPos)
    {
        Vector2 playerPos = transform.position;

        return Mathf.Abs(targetPos.x - playerPos.x) <= rangeX &&
               Mathf.Abs(targetPos.y - playerPos.y) <= rangeY;
    }

    void UpdateHighlight(Vector2 snappedMouse)
    {
        if (gridHighlight == null) return;

        gridHighlight.transform.position = snappedMouse;

        SpriteRenderer sr = gridHighlight.GetComponent<SpriteRenderer>();

        RaycastHit2D hitCheck =
            Physics2D.Raycast(snappedMouse, Vector2.zero);

        bool inRange = InRange(snappedMouse);

        if (!inRange)
            sr.color = new Color(1, 0, 0, 0.3f);
        else if (hitCheck.collider != null &&
                 hitCheck.collider.CompareTag("Ground"))
            sr.color = new Color(1, 1, 0, 0.3f);
        else
            sr.color = new Color(0, 1, 0, 0.3f);

        gridHighlight.transform.localScale =
            Vector3.one *
            (1 + Mathf.Sin(Time.time * 10f) * 0.05f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        CheckGround(col);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        CheckGround(col);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
            onGround = false;
    }

    void CheckGround(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    onGround = true;
                    return;
                }
            }
        }
    }

    void FlipPlayer()
    {
        if (horizontal > 0.1f && transform.localScale.x > 0)
        {
            transform.localScale =
                new Vector3(-4, 4, 4);
        }
        else if (horizontal < -0.1f &&
                 transform.localScale.x < 0)
        {
            transform.localScale =
                new Vector3(4, 4, 4);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(rangeX * 2, rangeY * 2, 0)
        );
    }
}