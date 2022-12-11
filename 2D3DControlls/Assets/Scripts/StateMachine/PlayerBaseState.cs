using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PlayerBaseState : State
{
    protected int airJumps = 3;
    protected int jumpCount;

    

    private NewPlayer3DController player;
    public NewPlayer3DController Player => player = player ?? (NewPlayer3DController)owner;

    
}
