using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable //���� �� ����� �Լ��� ���� �������̽�
{
    float Distance { get; }

    bool Interact(GameObject other);
    void StopInteract(GameObject other);
}
