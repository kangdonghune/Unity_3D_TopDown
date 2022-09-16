using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Wall);

	Texture2D _attackIcon;
	Texture2D _handIcon;
	Texture2D _LootIcon;

	private GameObject Target = null;
	private UI_TargetPointer _pointer = null;

	enum CursorType
	{
		None,
		Attack,
		Hand,
	}

	CursorType _cursorType = CursorType.None;

	void Start()
	{
		_attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
		_handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
		_LootIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Loot");
		Managers.Input.MouseAction -= OnMouseEvent;
		Managers.Input.MouseAction += OnMouseEvent;
	}

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	
		RaycastHit[] hits;

		hits = Physics.SphereCastAll(Camera.main.transform.position, 0.5f, ray.direction, 100.0f, _mask);
		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
			{
				if (_cursorType != CursorType.Attack)
				{
					Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
					_cursorType = CursorType.Attack;
					Target = hit.collider.gameObject;
					break;
				}
			}
			else
			{
				if (_cursorType != CursorType.Hand)
				{
					Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
					_cursorType = CursorType.Hand;
					Target = null;
				}
			}
		}
	}

	private void OnMouseEvent(Define.MouseEvent evt)
	{
		switch (evt)
		{
			case Define.MouseEvent.LClick:
				OnClicked();
				break;
			case Define.MouseEvent.LPointerDown:
				break;
			case Define.MouseEvent.LPointerUp:
				break;
			case Define.MouseEvent.LPress:
				break;
			case Define.MouseEvent.RClick:
				OnClicked();
				break;
			case Define.MouseEvent.RPointerDown:
				break;
			case Define.MouseEvent.RPointerUp:
				break;
			case Define.MouseEvent.RPress:
				break;
		}
	}

	void OnClicked()
    {
		if (_pointer != null)
        {
			_pointer.DestroyPoint();
			_pointer = null;
        }
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		LayerMask mask = LayerMask.GetMask("Ground");
		if (Target == null)
        {
			if (Physics.Raycast(ray, out hit, 100.0f, mask))
			{
				_pointer = Managers.UI.CreateWorldUI<UI_TargetPointer>("MovePoint", transform);
				_pointer.SetPosition(hit);
			}
		}
		else
        {
			_pointer = Managers.UI.CreateWorldUI<UI_TargetPointer>("Targeting", transform);
			_pointer.SetTarget(Target.transform);

		}
		

	}
}
