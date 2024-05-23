using Spine.Unity;
using UnityEngine;

public class PlayerCharacterView : MonoBehaviour
{
    #region variables
    public PlayerCharacterModel model;
    public SkeletonAnimation skeletonAnimation;

    public AnimationReferenceAsset run, idle, jump, attack, shield;

    PlayerCharacterModel.PlayerState previousState;
    #endregion

    void Start()
    {
        model.view = this; // Assign this view to the model
    }

    void Update()
    {
        if (skeletonAnimation.Skeleton.ScaleX < 0 != model.facingLeft)
        {
            Turn(model.facingLeft);
        }

        PlayerCharacterModel.PlayerState currentState = model.state;

        if (previousState != currentState)
        {
            PlayNewAnimation();
        }

        previousState = currentState;
    }

    void PlayNewAnimation()
    {
        PlayerCharacterModel.PlayerState newstate = model.state;
        Spine.Animation nextAnimation;
        bool shouldLoop;

        switch (newstate)
        {
            case PlayerCharacterModel.PlayerState.Jumping:
                nextAnimation = jump;
                shouldLoop = false;
                break;
            case PlayerCharacterModel.PlayerState.Running:
                nextAnimation = run;
                shouldLoop = true;
                break;
            case PlayerCharacterModel.PlayerState.Shielding:
                nextAnimation = shield;
                shouldLoop = false;
                break;
            case PlayerCharacterModel.PlayerState.Attacking:
                nextAnimation = attack;
                shouldLoop = false;
                break;
            default:
                nextAnimation = idle;
                shouldLoop = true;
                break;
        }

        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, shouldLoop);
    }

    public void Turn(bool facingLeft)
    {
        skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1 : 1f;
    }
}
