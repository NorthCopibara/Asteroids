using UnityEngine;

public class GameInput
{
    public MoveDirection GetInputForMove() 
    {
        if (Input.GetKey(KeyCode.D)) 
        {
            return MoveDirection.Right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            return MoveDirection.Left;
        }

        if (Input.GetKey(KeyCode.W))
        {
            return MoveDirection.Forward;
        }

        return MoveDirection.Idle;
    }

    public bool GetInputForAttack() 
    {
        return Input.GetKey(KeyCode.Space);
    }
}

public enum MoveDirection 
{
    Idle,
    Forward,
    Left,
    Right
}
