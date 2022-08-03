using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class GameManager : SingletonPersistent<GameManager>
{
    [HideInInspector]
    public User user;

    public List<Sprite> avatars;
    
    public void StartGame()
    {
        LoadingManager.Instance.NextLevel(EnumScenes.Game);
    }
}
