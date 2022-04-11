using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjController : MonoBehaviour
{
    private Dictionary<int, string> gameModeDec = new Dictionary<int, string>
    {
        { 0, "PickupContainer" },
        { 1, "ChildrenContainer"},
        { 2, "GemstonesContainer"}

    };

    public GameObject[] objs;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {


        if (Globals.hasRunModeSelection == true) {
            return;
        }

        Dictionary<int, string> modes = new Dictionary<int, string> (gameModeDec);
        modes.Remove(Globals.gameMode);

        foreach (string modeName in modes.Values) {

            Debug.Log("modes ------->" + modeName);
            objs = GameObject.FindGameObjectsWithTag(modeName);
            foreach (GameObject obj in objs)
            {
                
                obj.SetActive(false);
            }
        }

        

    }
}
 