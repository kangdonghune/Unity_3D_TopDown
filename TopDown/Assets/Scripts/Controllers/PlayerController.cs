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

    public virtual Transform Target { get; set; }
    public Transform projectileTransform;
    public Transform hitTransform;
    private CharacterController _characterController;
    private NavMeshAgent _navAgent;
    private Camera _camera;
    private Animator _animator;

    protected StateMachine<PlayerController> _stateMachine;
    public StateMachine<PlayerController> StateMachine { get { return _stateMachine; } }


    //UI
    [HideInInspector]
    public GameObject Inventory;

    private DynamicInventoryUI _dynamicInven;
    private StaticInventoryUI _equipInven;
    private UI_UnitDefault _defalutUI;
    private PlayerInGameUI _playerInGameUI;
    private PlayerStatUI _statUI;
    [HideInInspector]
    public bool _isOnUI;

 
    [HideInInspector]
    public bool isMove = false;

    public Collider attackCollider; 


    #endregion

    #region UnityRotationFunc
    protected override void AwakeInit()
    {
        StatsObject origin = Resources.Load<StatsObject>("Prefab/UI/Stat/PlayerStats");
        Stats = Instantiate<StatsObject>(origin);
        Stats.InitializeAttribute();
        UI_UnitDefault uI_origin = Resources.Load<UI_UnitDefault>("Prefab/UI/Unit/UI_UnitDefault");
        _defalutUI = Instantiate<UI_UnitDefault>(uI_origin, gameObject.transform);
        Inventory = Managers.Resource.Instantiate("UI/Inventory/PlayerUI");//유저 인벤토리 생성
        if (Inventory != null)
        {
            //인벤토리
            _dynamicInven = Inventory.FindChild<DynamicInventoryUI>();
            _dynamicInven.Owner = gameObject;
            _equipInven = Inventory.FindChild<StaticInventoryUI>();
            _equipInven.Owner = gameObject;
            _dynamicInven.inventoryObject.OnUseItem -= OnUseItem;
            _equipInven.inventoryObject.OnUseItem -= OnUseItem;
            _dynamicInven.inventoryObject.OnUseItem += OnUseItem;
            _equipInven.inventoryObject.OnUseItem += OnUseItem;
            //게임UI
            _playerInGameUI = Inventory.FindChild<PlayerInGameUI>();
            _playerInGameUI.playerStats = Stats;
            _playerInGameUI.AddEvent();
            _statUI = Inventory.FindChild<PlayerStatUI>();
            _statUI.equipment = _equipInven.inventoryObject;
            _statUI.playerStats = Stats;
            _statUI.SetRendering(false);
            _statUI.enabled = true;
        } //UI 별 이벤트 추가
        PlayerEquipment equip = GetComponent<PlayerEquipment>(); //아이템 장착 시 해당 아이템의 prefab생성하여 착용하기 위한 컴퍼넌트
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
        _navAgent.updatePosition = true; //이동은 컨트롤러가 시행
        _navAgent.updateRotation = true; // 회전은 컨트롤러가 시행
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        Managers.Input.KeyAction -= OnKeyEvent;
        Managers.Input.KeyAction += OnKeyEvent;
        _defalutUI._hpSlider.gameObject.SetActive(false);
       
    }
    #endregion

    protected virtual void Update()
    {
        _isOnUI = EventSystem.current.IsPointerOverGameObject();
        CheckAttackBehavior();
    }

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
        if (_isOnUI == true)
            return;

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
                    isMove = true;
                    isStop = true;
                    break;
                case (int)Define.Layer.Monster:
                    //ToDo 추후 스테이트패턴으로 변경
                    SetTarget(hit.collider.transform, CurrentAttackBehavior.Range);
                    isMove = true;
                    isStop = true;
                    break;
                case (int)Define.Layer.Ground:
                    RaycastHit Hit;
                    if (Physics.Raycast(ray, out Hit, 100, groundLayerMask))
                    {
                        RemoveTarget();
                        _navAgent.SetDestination(Hit.point);
                        isMove = true;
                    }
                    break;
            }

            if (isStop == true)
                break;
        }

    }
    #endregion

    #region KeyBoardFunc
    private void OnKeyEvent(Define.KeyEvent evt)
    {
        switch(evt)
        {
            case Define.KeyEvent.Down:
                OnSkillReady();
                break;
            case Define.KeyEvent.Press:
                OpenStatUI();
                break;
            case Define.KeyEvent.None:
                CloseStatUI();
                break;
        }
    }

    private void OnSkillReady()
    {
        //이미 예약된 스킬이 있다면 스킵
        foreach (AttackBehavior behavior in SkillBehaviors)
        {
            if (behavior.Ready)
                return;
        }
                
        //예약된 스킬이 없다면 예약

        KeyCode key = KeyCode.None;
        if (Input.GetKeyDown(KeyCode.Q))
            key = KeyCode.Q;
        if (Input.GetKeyDown(KeyCode.W))
            key = KeyCode.W;
        if (Input.GetKeyDown(KeyCode.E))
            key = KeyCode.E;
        if (Input.GetKeyDown(KeyCode.R))
            key = KeyCode.R;

        foreach (AttackBehavior behavior in SkillBehaviors)
        {
            if (behavior.Key == key && behavior.isAvailable)
            {
                behavior.Ready = true;
                if (behavior.type == Define.AttackType.Skill_Target && Target != null) //스킬이 타겟팅이면 재타겟팅
                    SetTarget(Target, behavior.Range);    
                break;
            }
        }
    
    }

    private void OpenStatUI()
    {
        if(Input.GetKey(KeyCode.C))
            _statUI.SetRendering(true);
    }
    private void CloseStatUI()
    {
        _statUI.SetRendering(false);
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

    public void RemoveTarget()
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
                case Define.UnitAttribute.HP:
                    Stats.AddHP(buff.value);
                    _defalutUI.CreateDamageText((int)buff.value);
                    break;
                case Define.UnitAttribute.Mana:
                    break;
                case Define.UnitAttribute.Attack:
                    break;
                case Define.UnitAttribute.AttackSpeed:
                    break;
                case Define.UnitAttribute.Defence:
                    break;
                case Define.UnitAttribute.MoveSpeed:
                    break;
                default:
                    Debug.LogWarning($"정의되지 않는 버프 스텟이 존재합니다.{buff.stat}");
                    break;
            }
        }
    }
    #endregion

    #region Attack & Damage  

    public void CheckAttackBehavior()
    {
        if (CurrentAttackBehavior == null || !CurrentAttackBehavior.isAvailable || !CurrentAttackBehavior.Ready)
            CurrentAttackBehavior = null;

        //어택 behavior를 순회하며 isAvailable이 참인 것 중 우선도가 가장 높은 것은 현재 실행할 behavior로 지정 
        foreach (AttackBehavior behavior in attackBehaviors)
        {
            if (behavior.isAvailable && behavior.Ready)
            {
                if (CurrentAttackBehavior == null || CurrentAttackBehavior.Priority < behavior.Priority)
                        CurrentAttackBehavior = behavior;
            }
        }
    }

    public AttackBehavior CurrentAttackBehavior { get; private set; }

    public void OnExecuteAttack(int attackIndex)
    {
        if (CurrentAttackBehavior != null && Target != null)
        {
            CurrentAttackBehavior.ExecuteAttack(Target.gameObject, projectileTransform);
        }
    }

    public bool IsAlive => Stats.HP > 0;

    public void TakeDamage(int damage, GameObject hitEffectPrefabs, GameObject attacker)
    {
        if (!IsAlive)
            return;

        if (hitEffectPrefabs)
        {
            Managers.Effect.Instantiate(hitEffectPrefabs, hitTransform.position, Quaternion.identity);
        }

        if (IsAlive)
        {
            Stats.AddHP(-damage);
            _defalutUI.CreateDamageText(damage);
        }
        else
        {
            //  StateMachine.ChangeState<DeadState>();
        }
    }

    public void OnAttackStart()
    {
        throw new NotImplementedException();
    }

    public void OnAttackEnd()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Corutine

    #endregion
}
