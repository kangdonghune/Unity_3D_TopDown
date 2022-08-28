using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject { Unknown,Player, Monster}
    public enum State { Idle, Move, Die, Skill, }
    public enum Layer { Monster = 6, Wall = 7, Ground = 9, Block = 10, }
    public enum Scene { Unknown, Login, Lobby, Game,}
    public enum Sound {  BGM, Effect, MaxCount,}
    public enum UIEvent { Click, Drag,}
    public enum CameraMode { TopView, QuarterView, End}    
    public enum MouseEvent { LPress, LPointerDown, LPointerUp, LClick, RPress, RPointerDown, RPointerUp, RClick }


}
