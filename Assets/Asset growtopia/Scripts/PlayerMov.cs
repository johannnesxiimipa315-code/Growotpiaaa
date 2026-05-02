using UnityEngine;
using UnityEngine.InputSystem; // INPUT SYSTEM BARU

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
        // Mouse position pakai Input System baru
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 snappedMouse = new Vector2(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y)
        );

        UpdateHighlight(snappedMouse);

        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("hit", hit);
        anim.SetBool("place", place);
        anim.SetFloat("vertical", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
       
        horizontal = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;

        
        float jump = Keyboard.current.spaceKey.isPressed ? 1f : 0f;

        FlipPlayer();

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        Vector2 snappedMouse = new Vector2(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y)
        );

        mineTimer -= Time.deltaTime;

        // MINING (klik kiri)
        hit = Mouse.current.leftButton.isPressed;

        if (hit && InRange(snappedMouse) && mineTimer <= 0f)
        {
            terrainGeneration.RemoveTile(snappedMouse);
            mineTimer = mineRate;
        }

        // PLACE BLOCK (klik kanan)
        placeTimer -= Time.deltaTime;
        place = Mouse.current.rightButton.isPressed;

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

        // JUMP
        if (jump > 0.1f && onGround)
        {
            movement.y = jumpForce;
        }

        rb.linearVelocity = movement;
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

        RaycastHit2D hit = Physics2D.Raycast(snappedMouse, Vector2.zero);

        bool inRange = InRange(snappedMouse);

        if (!inRange)
        {
            sr.color = new Color(1, 0, 0, 0.3f);
        }
        else if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            sr.color = new Color(1, 1, 0, 0.3f);
        }
        else
        {
            sr.color = new Color(0, 1, 0, 0.3f);
        }

        gridHighlight.transform.localScale =
            Vector3.one * (1 + Mathf.Sin(Time.time * 10f) * 0.05f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                    onGround = true;
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
                    onGround = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
            onGround = false;
    }

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