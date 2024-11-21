using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimationSpawnPoint;
    [SerializeField] private Transform weaponCollidor;
    [SerializeField] private float swordAttackCd = .5f;
    
    private PlayerControls _playerControls;
    private Animator _animator;
    private PlayerController _playerController;
    private ActiveWeapon _activeWeapon;
    private bool _attackButtonCooldown, _isAttacking = false;

    private GameObject _slashAnimation;

    private void Awake()
    {
        this._playerController = GetComponentInParent<PlayerController>();
        this._activeWeapon = GetComponentInParent<ActiveWeapon>();
        
        this._animator = GetComponent<Animator>();
        this._playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        this._playerControls.Enable();
    }

    private void Start()
    {
        this._playerControls.Combat.Attack.started    += _ => StartAttacking();
        this._playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    private void StartAttacking()
    {
        this._attackButtonCooldown = true;
    }

    private void StopAttacking()
    {
        this._attackButtonCooldown = false;
    }

    private void Attack()
    {
        if (this._attackButtonCooldown && !this._isAttacking)
        {
            this._isAttacking = true;
            this._animator.SetTrigger("Attack");
            this.weaponCollidor.gameObject.SetActive(true);
            this._slashAnimation = Instantiate(this.slashAnimationPrefab,
                this.slashAnimationSpawnPoint.position,
                Quaternion.identity);
            this._slashAnimation.transform.parent = this.transform.parent;
            StartCoroutine(AttackCdRoutine());
        }
    }

    private IEnumerator AttackCdRoutine()
    {
        yield return new WaitForSeconds(this.swordAttackCd);
        this._isAttacking = false;
    }

    public void DoneAttackingAnimationEvent()
    {
        this.weaponCollidor.gameObject.SetActive(false);
    }
    
    public void SwingUpFlipAnimationEvent()
    {
        this._slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (this._playerController.FacingLeft)
        {
            this._slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }
    
    public void SwingDownFlipAnimationEvent()
    {
        this._slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (this._playerController.FacingLeft)
        {
            this._slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }
    
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint =
            Camera.main.WorldToScreenPoint(this._playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            this._activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            this.weaponCollidor.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            this._activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            this.weaponCollidor.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        

    }
    
}
