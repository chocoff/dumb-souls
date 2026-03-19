using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance; //singleton 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } 
}
