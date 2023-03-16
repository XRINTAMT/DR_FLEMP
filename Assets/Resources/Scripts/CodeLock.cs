using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeLock : MonoBehaviour
{
    private string code;
    PhotonManager photonManager;

    public Text showCode;
    [SerializeField] private int maxLength;
    private JoinRoomController MultiplayerController;

    private void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
        MultiplayerController = GetComponent<JoinRoomController>();
    }
    public void AddDigit(string digit)
    {
        code += digit;
        if (code.Length > maxLength)
        {
            code = code.Substring(1, code.Length - 1);
        }
        showCode.text = code;
    }

    public void Backspace()
    {
        code = code.Substring(0, code.Length - 1);
        showCode.text = code;
    }

    public void Clear()
    {
        code = "";
        showCode.text = code;
    }

    public void Submit()
    {
        if (photonManager == null)
            return;

        if (code == "")
        {
            MultiplayerController.CreateRoom();
        }
        else
        {
            MultiplayerController.CheckPassword();
        }
    }
}
