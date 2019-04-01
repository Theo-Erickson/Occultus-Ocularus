using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplay : MonoBehaviour
{
    GameObject minCtrls;
    GameObject detailedCtrls;

    // Start is called before the first frame update
    void Start()
    {
        minCtrls = gameObject.transform.GetChild(0).gameObject;
        detailedCtrls = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch between minimalist and detailed controls UI when Tab is pressed
        if (Input.GetKeyDown(KeyCode.Tab)) {

            if (!minCtrls.activeSelf && !detailedCtrls.activeSelf || minCtrls.activeSelf && detailedCtrls.activeSelf) {
                minCtrls.SetActive(true);
                detailedCtrls.gameObject.SetActive(false);
            }
            else if (minCtrls.activeSelf && !detailedCtrls.activeSelf) {
                minCtrls.SetActive(false);
                detailedCtrls.gameObject.SetActive(true);
            }
            else if (!minCtrls.activeSelf && detailedCtrls.activeSelf) {
                minCtrls.SetActive(false);
                detailedCtrls.gameObject.SetActive(false);
            }
        }
    }
}
