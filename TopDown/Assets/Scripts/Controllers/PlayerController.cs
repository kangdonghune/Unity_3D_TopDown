using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : BaseController, IAttackable, IDamageable
{
    #region Variable
    public LayerMask groundLayerMask;

    private int _mask = (1 << (int)Define.Layer.Ground) |
                        (1 << (int)Define.Layer.Monster) |
                        (1 << (int)Define.Layer.Item) |
                        (1 << (int)Define.Layer.ItemBox);

    public float groundCheckDistance = 0.3f;

    public virtual Transform Target { get; protected set; }
    public Transform projectileTransform;
    public Transform hitTransform;
    private CharacterController _characterController;
    private NavMeshAgent _navAgent;
    private Camera _camera;
    private Animator _animator;

    [HideInInspector]
    public GameObject Inventory;

    private DynamicInventoryUI _dynamicInven;
    private StaticInventoryUI _equipInven;

    private UI_UnitDefault _unitUI;
    private int _maxHp = 100;
    private int _hp = 100;

    private bool _isOnUI;

    readonly int moveHash = Animator.StringToHash("Move");



    #endregion

    #region UnityRotationFunc
    protected override void AwakeInit()
    {
        _unitUI = Managers.UI.CreateUnitUI<UI_UnitDefault>(null, transform);
        Inventory = Managers.Resource.Instantiate("UI/Inventory/Inventory");//유저 인벤토리 생성
        if (Inventory != null)
        {
            _dynamicInven = Inventory.FindChild<DynamicInventoryUI>();
            _dynamicInven.Owner = gameObject;
            _equipInven = Inventory.FindChild<StaticInventoryUI>();
            _equipInven.Owner = gameObject;
            _dynamicInven.inventoryObject.OnUseItem -= OnUseItem;
            _equipInven.inventoryObject.OnUseItem -= OnUseItem;
            _dynamicInven.inventoryObject.OnUseItem += OnUseItem;
            _equipInven.inventoryObject.OnUseItem += OnUseItem;
        }
        PlayerEquipment equip = GetComponent<PlayerEquipment>();
        if (equip != null)
        {
            equip.equipment = _equipInven.inventoryObject;
        }
    }

    protected override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _characterController = GetComponent<CharacterController>();
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.updatePosition = false; //이동은 컨트롤러가 시행
        _navAgent.updateRotation = true; // 회전은 네비가 하도록
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        _unitUI.MaximumValue = _maxHp;
        _unitUI.Value = _hp;

   

    }

    void Update()
    {
        _isOnUI = EventSystem.current.IsPointerOverGameObject();

        PlayerMove();
        _unitUI.Value = _hp;
    }

    #endregion

    #region MouseFunc
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_isOnUI == true)
            return;
        switch (evt)
        {
            case Define.MouseEvent.LClick:
                OnMousePicking();
                break;
            case Define.MouseEvent.LPointerDown:
                break;
            case Define.MouseEvent.LPointerUp:
                break;
            case Define.MouseEvent.LPress:
                break;
            case Define.MouseEvent.RClick:
                OnMousePicking();
                break;
            case Define.MouseEvent.RPointerDown:
                break;
            case Define.MouseEvent.RPointerUp:
                break;
            case Define.MouseEvent.RPress:
                break;
        }
    }

    private void OnMousePicking()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;

        bool isStop = false;
        hits = Physics.SphereCastAll(Camera.main.transform.position, 0.5f, ray.direction, 100.0f, _mask);
        foreach (RaycastHit hit in hits)
        {
            switch (hit.transform.gameObject.layer)
            {
                case (int)Define.Layer.ItemBox:
                case (int)Define.Layer.Item:
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if(interactable != null)
                    {
                        SetTarget(hit.collider.transform, interactable.Distance);
                    }
                    _navAgent.SetDestination(hit.collider.transform.position);
                    isStop = true;
                    break;
                case (int)Define.Layer.Monster:
                    //ToDo 추후 스테이트패턴으로 변경
                    _navAgent.SetDestination(hit.point);
                    isStop = true;
                    break;
                case (int)Define.Layer.Ground:
                    RaycastHit Hit;
                    if (Physics.Raycast(ray, out Hit, 100, groundLayerMask))
                    {
                        _navAgent.SetDestination(Hit.point);
                        RemoveTarget();
                    }
                    break;
            }

            if (isStop == true)
                break;
        }

    }

    #endregion

    #region MoveFunc
    private void SetTarget(Transform target, float distance)
    {
        Target = target;

        _navAgent.stoppingDistance = distance;
        _navAgent.updateRotation = true;
        _navAgent.SetDestination(target.transform.position);
    }

    private void RemoveTarget()
    {
        CallStopInteract();
        Target = null;
        _navAgent.stoppingDistance = 0.01f;
    }

    private void CallStopInteract()
    {
        if (Target != null)
        {
            if (Target.GetComponent<IInteractable>() != null)
            {
                IInteractable interactable = Target.GetComponent<IInteractable>();
                interactable.StopInteract(this.gameObject);
            }
        }
    }

    private void PlayerMove()
    {

        if (_navAgent.remainingDistance > _navAgent.stoppingDistance)
        {
            _characterController.Move(_navAgent.velocity * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, _navAgent.nextPosition.y, transform.position.z);
            _animator.SetBool(moveHash, true);
        }
        else
        {
            if (Target != null)
            {
                if (Target.GetComponent<IInteractable>() != null)
                {
                    IInteractable interactable = Target.GetComponent<IInteractable>();
                    interactable.Interact(this.gameObject);
                }
            }
            transform.position = _navAgent.nextPosition;
            _navAgent.velocity = Vector3.zero;
            _animator.SetBool(moveHash, false);
        }     
    }

    #endregion

    #region Inventory & Item

    public bool PickUpItem(GroundItem groundItem)
    {
        if (groundItem != null)
        {
            //습득 시 1순위는 장비칸 인벤토리
            //2순위는 보관 인벤토리
            if (_equipInven.AddItemPossible(groundItem.ItemObject, groundItem.ItemObject.itemAmount))
            {
                Managers.Resource.Destroy(groundItem.gameObject);
                return true;
            }
            if (_dynamicInven.AddItemPossible(groundItem.ItemObject, groundItem.ItemObject.itemAmount))
            {
                Managers.Resource.Destroy(groundItem.gameObject);
                return true;
            }
            return false;
        }
        return false;
    }
    public bool ConnectBox(ItemBox box)
    {
        return box.itemBoxInvenUI.ConnectInven(Inventory);
    }
    private void OnUseItem(ItemObject itemObject)
    {
        foreach (ItemBuff buff in itemObject.data.buffs)
        {
            switch(buff.stat)
            {
                case CharacterAttribute.HP:
                    _hp += (int)buff.value;
                    break;
                case CharacterAttribute.Mana:
                    break;
                case CharacterAttribute.Attack:
                    break;
                case CharacterAttribute.AttackSpeed:
                    break;
                case CharacterAttribute.Defence:
                    break;
                case CharacterAttribute.MoveSpeed:
                    break;
                default:
                    Debug.LogWarning($"정의되지 않는 버프 스텟이 존재합니다.{buff.stat}");
                    break;
            }
        }
    }
    #endregion
    #region interface
    public AttackBehavior CurrentAttackBehavior { get; private set; }

    public void OnExecuteAttack(int attackIndex)
    {
        if (CurrentAttackBehavior != null && Target != null)
        {
            CurrentAttackBehavior.ExecuteAttack(Target.gameObject, projectileTransform);
        }
    }

    public bool IsAlive => true;

    public void TakeDamage(int damage, GameObject hitEffectPrefabs)
    {
        if (!IsAlive)
            return;

        if (hitEffectPrefabs)
        {
            Managers.Effect.Instantiate(hitEffectPrefabs, hitTransform.position, Quaternion.identity);
        }

        if (IsAlive)
        {
            _hp -= damage;
            _unitUI.CreateDamageText(damage);
        }
        else
        {
            //  StateMachine.ChangeState<DeadState>();
        }
    }
    #endregion

    #region Corutine

    #endregion
}
