using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour {

    public CanvasGroup canvasGroup;

    public OrbController currentOrb;

    public Dropdown tokenDropdown;

    public GameObject cursor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showDialog()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for(int i = 0; i < GameManager.instance.playerTeam.Count; i++)
        {
            int pos = i + 1;
            options.Add(new Dropdown.OptionData("World " + pos));
        }
        tokenDropdown.options.Clear();
        tokenDropdown.AddOptions(options);

        changeOrb();

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void changeOrb()
    {
        currentOrb = GameManager.instance.playerTeam[tokenDropdown.value];
        showCursor();
    }

    public void showCursor(){
        cursor.transform.localScale = new Vector3(currentOrb.transform.localScale.x + 1.5f,
            currentOrb.transform.localScale.y + 1.5f, 1);
        cursor.transform.position = currentOrb.gameObject.transform.position;
    }

    public void hideCursor()
    {
        cursor.transform.position = new Vector3(0, 0, 1);
    }

    public void LevelUpOrb(){
        currentOrb.LevelUp();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        hideCursor();

        GameManager.instance.executeManager.showDialog();
    }

    public void AddOrb(){
        GameManager.instance.addToPlayerTeam();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        hideCursor();

        GameManager.instance.executeManager.showDialog();

    }
}
