using System;
using UnityEngine;

[Serializable]
public class PlayerInfo :MonoBehaviour
{
    public string name = "bob";
    public int lives = 2 ;
    public float health = 3 ;
    public string jsonString;
    public void Start()
    {
        jsonString = JsonUtility.ToJson(this, true);
        CreateFromJSON(jsonString);
        Debug.Log(name);
    }
    public static PlayerInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.

}