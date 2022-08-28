using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    private Animator Ani { get { return GetComponent<Animator>(); } }

    [SerializeField]
    protected Vector3 _destPos;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    [SerializeField]
    protected virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (_state)
            {
                case Define.State.Idle:
                    Ani.CrossFade("WAIT", 0.1f, -1, 0);
                    break;
                case Define.State.Move:
                    Ani.CrossFade("RUN", 0.1f, -1, 0);
                    break;
                case Define.State.Skill:
                    Ani.CrossFade("ATTACK1", 0.1f, -1, 0);
                    break;
                case Define.State.Die:
                    break;
            }
        }
    }

    protected GameObject _lockTarget = null;


    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public void Start()
    {
        Init();
    }

    protected abstract void Init();

    protected void UpdateState()
    {
        switch (_state)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;

            case Define.State.Move:
                UpdateMove();
                break;

            case Define.State.Die:
                UpdateDie();
                break;

            case Define.State.Skill:
                UpdateSkill();
                break;

            default:
                break;
        }

        return;
    }


    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateSkill() { }

    void Update()
    {
        
    }
}
