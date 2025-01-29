using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableStartGameButton : MonoBehaviour
{   
    public Button gameButton;

    // Start is called before the first frame update
    void Start()
    {

        // Start the coroutine
        StartCoroutine(WaitAndCallFunction());
    }

    private IEnumerator WaitAndCallFunction()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        gameButton.gameObject.SetActive(true);

        
    }


}
