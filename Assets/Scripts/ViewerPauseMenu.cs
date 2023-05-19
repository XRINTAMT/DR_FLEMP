
using System.Collections.Generic;
using Autohand;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewerPauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Text roomNumber;
    PhotonView photonView;
    // Start is called before the first frame update
    bool menuActive;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            EventSystem evenSystem = FindObjectOfType<EventSystem>();
            evenSystem.gameObject.AddComponent<StandaloneInputModule>();
            Destroy(evenSystem.gameObject.GetComponent<AutoInputModule>());
        }
    }
    void Start()
    {
        menu.SetActive(false);
        roomNumber.text = JoinRoomController.nameRoom;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && photonView.IsMine) 
        {
            Cursor.visible = !menu.activeSelf;
            Cursor.lockState = CursorLockMode.None;
            menu.SetActive(!menu.activeSelf);

        }
           

    }

    public void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
    }
}
