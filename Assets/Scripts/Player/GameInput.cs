using UnityEngine;

public class GameInput
{
    public bool GetInputForMove() 
    {
        return Input.GetKey(KeyCode.W);
    }

    public float GetRotation() 
    {
        return Input.GetAxis("Horizontal");
    }

    public bool GetInputForAttack() 
    {
        return Input.GetKey(KeyCode.Space);
    }
}
