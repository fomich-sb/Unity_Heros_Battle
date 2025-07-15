using UnityEngine;

public class InputHeroAuto : IInputSystem
{
    private HeroControllerBase enemyController;
    private GameController gameController;

    public InputHeroAuto(GameController gameController, HeroControllerBase enemyController)
    {
        this.enemyController = enemyController;
        this.gameController = gameController;
    }

    public InputFlags GetInputValues()
    {
        return new InputFlags
        {
            MoveForward = gameController.HerosDistance > 2 ? true : false,
            MoveBack = gameController.HerosDistance < 3 ? true : false,
            Jump = Random.value < 0.5 ? true : false,
            SeatDown = Random.value < 0.5 ? true : false,
            Block = Random.value < 0.5 || enemyController.HasAttack() ? true : false,
            Attack = Random.value < 0.5 && !enemyController.HasBlock() && gameController.HerosDistance < 2.5f ? true : false,
            SuperAttack1 = Random.value < 0.5 ? true : false,
            SuperAttack2 = Random.value < 0.5 ? true : false,
        };
    }
}
