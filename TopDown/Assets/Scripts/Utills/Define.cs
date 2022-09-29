using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject { Unknown,Player, Monster}
    public enum State { Idle, Move, Die, Skill, }
    public enum Layer { Monster = 6, Wall = 7, Player = 8,Ground = 9, Block = 10, Enabled = 11, Item = 12, ItemBox = 13}
    public enum Scene { Unknown, Login, Lobby, Game,}
    public enum Sound {  BGM, Effect, MaxCount,}
    public enum UIEvent { Click, Drag,}
    public enum CameraMode { QuarterView}    
    public enum MouseEvent { LPress, LPointerDown, LPointerUp, LClick, RPress, RPointerDown, RPointerUp, RClick }
    public enum KeyEvent { Down, Press, None}
    public enum AttackType { Default, Skill_Target, Skill_NoneTarget}
    public enum PlayerAttackIndex { Default = 0, Q = 1, W = 2, E = 3, R = 4}
    public enum MonsterAttackPattern { Attack1 = 0, Attack2 =1 , Projectile = 2 }
    public enum AttackPrioty { Firts = 5, Second = 4, Third = 3, Forth = 2, Fifth = 1, }
    public enum ItemType { Weapon = 0, Helmet = 1, Chest = 2, Gloves = 3, Pants = 4, Boots = 5, Accessories = 6, Consumable, Food, Default }
    public enum UnitAttribute { HP, Mana, Attack, AttackSpeed, Defence, MoveSpeed, }
    public enum MonsterInterface {FOV,Patrol,}
    public enum Tag { Bear, Palet,ItemBox}
}
