using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeLock : MonoBehaviour
{
    private string code;
    PhotonManager photonManager;

    public Text showCode;
    [SerializeField] int maxLength;

    private void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
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

        photonManager.ConnectToRandomRoom();
        //Joining the room logic here

    }
}
