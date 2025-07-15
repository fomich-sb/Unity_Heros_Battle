public class InputHeroOnline : IInputSystem
{
    private bool _moveForward = false;
    private bool _moveBack = false;
    private bool _jump = false;
    private bool _seatDown = false;
    private bool _block = false;
    private bool _attack = false;
    private bool _superAttack1 = false;
    private bool _superAttack2 = false;


    public InputFlags GetInputValues()
    {
        return new InputFlags
        {
            MoveForward = _moveForward,
            MoveBack = _moveBack,
            Jump = _jump,
            SeatDown = _seatDown,
            Block = _block,
            Attack = _attack,
            SuperAttack1 = _superAttack1,
            SuperAttack2 = _superAttack2,
        };
    }

    public void UpdateHeroInputOnline(InputFlags heroInputData)
    {
        _moveForward = heroInputData.MoveForward;
        _moveBack = heroInputData.MoveBack;
        _jump = heroInputData.Jump;
        _seatDown = heroInputData.SeatDown;
        _block = heroInputData.Block;
        _attack = heroInputData.Attack;
        _superAttack1 = heroInputData.SuperAttack1 || heroInputData.SuperAttack;
        _superAttack2 = heroInputData.SuperAttack2 || heroInputData.SuperAttack;

    }
}
