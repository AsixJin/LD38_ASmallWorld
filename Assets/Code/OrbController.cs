using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {

    private Rigidbody2D rBody;
    [Range(1, 10)]
    public int power;
    [Range(0, 360)]
    public int angle = 0;
    [Range(1,3)]
    public float level = 1;
    public TeamType currentTeam;

    public GameObject target;

    private void Awake(){
        rBody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void testFunctions(){
        if(Input.GetKeyDown(KeyCode.A))
        {
            FireInAngle();
            Debug.Log("Apply Force");
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            LevelUp();
            Debug.Log("Level Up to " + level);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            FireTowardTarget();
            Debug.Log("Firing toward target");
        }
    }

    public void FireInAngle(){
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        rBody.AddForce(dir * CalculatePower());
    }

    public void FireTowardTarget(){
        Vector2 dir = (Vector2)(target.transform.position);
        dir.Normalize();
        power = Random.Range(3, 9);
        //GetComponent<Rigidbody2D>().AddForce(dir * CalculatePower(), ForceMode2D.Force);

        rBody.AddForce((target.transform.position - transform.position).normalized * (power*8), ForceMode2D.Impulse);
    }

    private float CalculatePower(){
        return ((power * 4) * 100) * level;
    }

    public void LevelUpAI(float newLevel){
        level = newLevel;
        transform.localScale = new Vector3(level * 2, level * 2, 1);
        rBody.mass = level * 2;

    }

    public void LevelUp(){
        if(level < 3){
            level++;
            transform.localScale = new Vector3(level * 2, level*2, 1);
            rBody.mass = level * 2;
        }
    }

    public void setTeam(int team){
        if(team == 0){
            currentTeam = TeamType.player;
            GetComponent<SpriteRenderer>().color = Color.red;
            GameManager.instance.tokenCount++;
        } else if(team == 1){
            currentTeam = TeamType.AI_One;
            GetComponent<SpriteRenderer>().color = Color.green;
        } else{
            currentTeam = TeamType.AI_Two;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    public void OnRingOut(){
        //If an AI kill make sure player gets their reward
        if(currentTeam == TeamType.AI_One || currentTeam == TeamType.AI_Two)
        {
            GameManager.instance.killCount++;
            GameManager.instance.kiledThisTurn = true;
        } else{
            GameManager.instance.levelCount += (int)level;
        }

        //Remove from list
        if(currentTeam == TeamType.player) {
            GameManager.instance.playerTeam.Remove(this);
        } else if(currentTeam == TeamType.AI_One) {
            GameManager.instance.aTeam.Remove(this);
        } else if(currentTeam == TeamType.AI_Two){
            GameManager.instance.bTeam.Remove(this);
        }

        //Destroy
        Destroy(this.gameObject);
    }

    private void OnTriggerExit2D(Collider2D col){
        if(col.transform.tag == "arena"){
            OnRingOut();
        }
    }
}
