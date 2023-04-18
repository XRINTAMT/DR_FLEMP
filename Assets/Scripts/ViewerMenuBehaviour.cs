using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ViewerMenuBehaviour : MonoBehaviour
{
    [SerializeField] GameObject RoomListEntry;
    [SerializeField] GameObject Container;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddRoom(string _roomName)
    {
        GameObject _newEntry = Instantiate(RoomListEntry);
        _newEntry.transform.SetParent(Container.transform);
        _newEntry.GetComponent<RoomListEntryBehaviour>().SetUp(_roomName);
    }

    public void Refresh()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
