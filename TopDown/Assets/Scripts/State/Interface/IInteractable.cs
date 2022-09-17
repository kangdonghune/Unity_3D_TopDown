using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable //접근 시 사용할 함수에 대한 인터페이스
{
    float Distance { get; }

    bool Interact(GameObject other);
    void StopInteract(GameObject other);
}
