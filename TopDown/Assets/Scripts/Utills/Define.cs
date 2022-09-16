using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject { Unknown,Player, Monster}
    public enum State { Idle, Move, Die, Skill, }
    public enum Layer { Monster = 6, Wall = 7, Player = 8,Ground = 9, Block = 10, }
    public enum Scene { Unknown, Login, Lobby, Game,}
    public enum Sound {  BGM, Effect, MaxCount,}
    public enum UIEvent { Click, Drag,}
    public enum CameraMode { TopView, QuarterView, None,End}    
    public enum MouseEvent { LPress, LPointerDown, LPointerUp, LClick, RPress, RPointerDown, RPointerUp, RClick }
    public enum MonsterAttackPattern { Attack1 = 0, Attack2 =1 , Projectile = 2 }
    public enum AttackPrioty { Firts = 5, Second = 4, Third = 3, Forth = 2, Fifth = 1, }
    public enum EquipType { };
}
