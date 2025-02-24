using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shap_NPC : MonoBehaviour
{
    public GameObject I_Shap;
    GameObject B_Pause;
    Transform Player;

    Vector3 Current_Pos;

    float open_distance;

    private void Awake()
    {
        Init_Data();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Open_Shap();
    }

    void Init_Data()
    {
        Player = GameObject.Find("Player").transform;
        B_Pause = GameObject.Find("B_Pause");
        Current_Pos = this.transform.position;
        open_distance = 3f;
    }

    void Open_Shap()
    {
        float search_dir = Vector3.Distance(Current_Pos, Player.position);

        if (search_dir <= open_distance && Input.GetKeyDown(KeyCode.E) == true)
        {
            GameObject.Find("Game_Manager").GetComponent<Game_Manager>().isShopOpen = true;
            GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Show_Data();
            I_Shap.gameObject.SetActive(true);
            B_Pause.gameObject.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0f;
        }
    }

    public void Close_Shap()
    {
        GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Save_Data();
        I_Shap.gameObject.SetActive(false);
        B_Pause.gameObject.SetActive(true);
        GameObject.Find("Game_Manager").GetComponent<Game_Manager>().isShopOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Show_Money();
        Time.timeScale = 1f;
    }
}
