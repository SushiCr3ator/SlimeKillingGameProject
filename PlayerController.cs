using System.Collections;
using UnityEngine;
 
public class PlayerController : MonoBehaviour
{

    public bool FacingLeft { get; private set; } = false;

    public static PlayerController Instance;
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer trailRenderer;
    
    private PlayerControls _playerControls;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Knockback _knockback;
    private float _startingMovementSpeed;

    private bool _isDashing = false;

    private void Awake()
    {
        Instance = this;
        this._playerControls = new PlayerControls();
        this._rb = GetComponent<Rigidbody2D>();
        this._animator = GetComponent<Animator>();
        this._spriteRenderer = GetComponent<SpriteRenderer>();
        this._knockback = GetComponent<Knockback>();

    }

    private void Start()
    {
        this._playerControls.Combat.Dash.performed += _ => Dash();

        this._startingMovementSpeed = this.moveSpeed;
    }

    private void OnEnable()
    {
        this._playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        this._movement = this._playerControls.Movement.Move.ReadValue<Vector2>();
        
        // ReSharper disable Unity.PreferAddressByIdToGraphicsParams
        this._animator.SetFloat("moveX", this._movement.x);
        this._animator.SetFloat("moveY", this._movement.y);
        // ReSharper restore Unity.PreferAddressByIdToGraphicsParams
    }

    private void Move()
    {
        if (this._knockback.GettingKnockedBack) { return;}
        this._rb.MovePosition(this._rb.position + this._movement * (this.moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        // ReSharper disable once PossibleNullReferenceException
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            this._spriteRenderer.flipX = true;
            this.FacingLeft = true;
        }
        else
        {
            this._spriteRenderer.flipX = false;
            this.FacingLeft = false;
        }

    }

    private void Dash()
    {
        if (!this._isDashing)
        {
            this._isDashing = true;
            this.moveSpeed *= this.dashSpeed;
            this.trailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCd = .25f;
        yield return new WaitForSeconds(dashTime);
        this.moveSpeed = this._startingMovementSpeed;
        this.trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCd);
        this._isDashing = false;
    }
    
}
    
    
