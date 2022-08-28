using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject { Unknown,Player, Monster}
    public enum State { Idle, Move, Die, Skill, }
    public enum Layer { Monster = 6, Plane = 9, Block = 10, }
    public enum Scene { Unknown, Login, Lobby, Game,}
    public enum Sound {  BGM, Effect, MaxCount,}
    public enum UIEvent { Click, Drag,}
    public enum CameraMode { QuarterView, }    
    public enum MouseEvent { Press, PointerDown, PointerUp, Click,}


}
