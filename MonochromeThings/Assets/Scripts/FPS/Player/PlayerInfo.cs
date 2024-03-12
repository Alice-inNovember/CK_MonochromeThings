using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInfo 
{
    //체력
    int HP;
    
    //방어력
    int Physic_Armour;
    int Magic_Armour;

    //공격력
    int Physic_ATK;
    int Magic_ATK;

    //속도
    float ATK_Speed;
    float WalkSpeed;
    float Movement_Speed;

    //체력 회복
    int HP_Regen;
    float HP_RegenAmount;

    //점프
    float JumpHeight;
    float JumpSpeed;

    //중력
    float Gravity;

    //흡혈 관련 능력치
    float HP_Steal;
    float HP_StealDelay;

    


}
