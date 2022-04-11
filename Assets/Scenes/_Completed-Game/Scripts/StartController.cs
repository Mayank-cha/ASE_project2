using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine.Networking;


public class StartController : MonoBehaviour
{
    public Button startButton;
    public TMP_InputField userNameField;
    public TMP_Dropdown modeDropdown;
    public string addScoreURL = "http://localhost/db_scripts/addscores.php?";

    private string secretKey = "mySecretKey";


    void Start()
    {
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(AddUserToDB);

    }

    void AddUserToDB()
    {

        Globals.userName = userNameField.text;
        Globals.gameMode = modeDropdown.value;
        if (userNameField.text == "")
        {
            return;
        }
        
        addDetailsToDB();


    }
    public void addDetailsToDB()
    {
        StartCoroutine(addDetails(userNameField.text, modeDropdown.value));

    }

    IEnumerator addDetails(string name, int mode)
    {
        string hash = HashInput(name + mode + secretKey);

        string post_url = addScoreURL + "name=" +
               UnityWebRequest.EscapeURL(name) + "&mode="
               + mode + "&hash=" + hash;

        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, hash);
        yield return hs_post.SendWebRequest();
        if (hs_post.error != null)
            Debug.Log("There was an error adding user details : "
                    + hs_post.error);

    }

    public string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue =
                hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert =
                 BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }
}