using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteManager : MonoBehaviour {

    public CanvasGroup canvasGroup;

    public OrbController currentOrb;

    public GameObject cursor;
    public Dropdown tokenDropdown;
    public Slider powerSlider;
    public Slider angleSlider;
    public Text powerText;
    public Text angleText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        powerText.text = "Power\n" + (int)powerSlider.value + "/10";
        angleText.text = "Angle\n" + (int)angleSlider.value + "/360";
	}

    public void showDialog(){
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for(int i = 0; i < GameManager.instance.playerTeam.Count; i++){
            int pos = i + 1;
            options.Add(new Dropdown.OptionData("World " + pos));
        }
        tokenDropdown.options.Clear();
        tokenDropdown.AddOptions(options);
        powerSlider.value = 1;
        angleSlider.value = 0;

        changeOrb();

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void changeOrb(){
        currentOrb = GameManager.instance.playerTeam[tokenDropdown.value];
        showCursor();
    }

    public void showCursor(){
        cursor.transform.localScale = new Vector3(currentOrb.transform.localScale.x+1.5f,
            currentOrb.transform.localScale.y + 1.5f, 1);
        cursor.transform.position = currentOrb.gameObject.transform.position;
    }

    public void hideCursor(){
        cursor.transform.position = new Vector3(0,0,1);
    }

    public void executeAttack(){
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        hideCursor();

        currentOrb.power = (int)powerSlider.value;
        currentOrb.angle = (int)angleSlider.value;
        currentOrb.FireInAngle();

        StartCoroutine(runTurn());
        
    }

    public IEnumerator runTurn(){

        yield return new WaitForSeconds(1f);

        GameManager.instance.TeamA_Attack();

        yield return new WaitForSeconds(2f);

        GameManager.instance.TeamB_Attack();

        yield return new WaitForSeconds(2f);

        GameManager.instance.newTurn();

        yield return null;
    }
}
