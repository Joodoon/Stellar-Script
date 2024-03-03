using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;

    protected float startTime;

    private string animBoolName;

    public bool isCasting;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        //player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;

        Debug.Log(animBoolName);

        isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        //player.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() { 
        isCasting = player.InputHandler.castInput;

        if(isCasting){
            Time.timeScale = 0.1f;

            foreach(Image star in player.StarGrid.stars){
                star.color = new Color(star.color.r, star.color.g, star.color.b, 1f);
            }
        }
        else{
            Time.timeScale = 1f;
            foreach(Image star in player.StarGrid.stars){
                star.color = new Color(star.color.r, star.color.g, star.color.b, 0f);
            }
        }
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
