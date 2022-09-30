using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillText: MonoBehaviour
{
    [TextArea(15, 20)]
    public string Text;
}
