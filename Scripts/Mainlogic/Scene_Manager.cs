using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void Go_to_Title()
    {
        SceneManager.LoadScene("1_TitleScene");
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
