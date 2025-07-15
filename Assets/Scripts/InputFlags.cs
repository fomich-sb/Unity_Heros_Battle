public struct InputFlags
{
    public bool MoveForward;
    public bool MoveBack;
    public bool Jump;
    public bool SeatDown;
    public bool Attack;
    public bool Block;
    public bool SuperAttack1;
    public bool SuperAttack2;

    public bool SuperAttack;

    /* public int Move;
     public int JumpSeatDown;
     public int AttackBlock;*/

    public static InputFlags operator |(InputFlags a, InputFlags b)
    {
        return new InputFlags
        {
            MoveForward = a.MoveForward || b.MoveForward,
            MoveBack = a.MoveBack || b.MoveBack,
            Jump = a.Jump || b.Jump,
            SeatDown = a.SeatDown || b.SeatDown,
            Block = a.Block || b.Block,
            Attack = a.Attack || b.Attack,
            SuperAttack1 = a.SuperAttack1 || b.SuperAttack1,
            SuperAttack2 = a.SuperAttack2 || b.SuperAttack2
        };
    }

    public bool Equals(InputFlags other)
    {
        return MoveForward == other.MoveForward &&
               MoveBack == other.MoveBack &&
               Jump == other.Jump &&
               SeatDown == other.SeatDown &&
               Block == other.Block &&
               Attack == other.Attack &&
               SuperAttack1 == other.SuperAttack1 &&
               SuperAttack2 == other.SuperAttack2;
    }

    public bool CorrespondsToInput(InputFlags other)
    {
        return     (MoveForward == false || other.MoveForward == true)
                && (MoveBack == false || other.MoveBack == true)
                && (Jump == false || other.Jump == true)
                && (SeatDown == false || other.SeatDown == true)
                && (Block == false || other.Block == true)
                && (Attack == false || other.Attack == true)
                && (SuperAttack1 == false || other.SuperAttack1 == true)
                && (SuperAttack2 == false || other.SuperAttack2 == true);
    }

    public int GetScore()
    {
        return (MoveForward ? 1 : 0) + (MoveBack ? 1 : 0) + (Jump ? 1 : 0) + (SeatDown ? 1 : 0) + (Block ? 1 : 0) + (Attack ? 1 : 0);
    }
}