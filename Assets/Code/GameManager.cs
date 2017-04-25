using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    //Orb Prefab
    public GameObject orbPrefab;

    //Spawn Points
    public GameObject xOne_Spawn;
    public GameObject xTwo_Spawn;
    public GameObject yOne_Spawn;
    public GameObject yTwo_Spawn;

    //UI
    public ExecuteManager executeManager;
    public RewardManager rewardManager;

    //Orb Team List
    public List<OrbController> playerTeam = new List<OrbController>();
    public List<OrbController> aTeam = new List<OrbController>();
    public List<OrbController> bTeam = new List<OrbController>();

    //Other Params
    public float currentAILevel = 1;
    public bool kiledThisTurn = false;

    public int killCount = 0;
    public int levelCount = 0;
    public int tokenCount = 1;

    private void Awake(){
        instance = this;
    }

    // Use this for initialization
    void Start () {
        startGame();
        executeManager.showDialog();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void startGame(){
        for(int i = 1;i<=3;i++){
            addToAITeam();
        }
    }

    public void newTurn(){
        //Increase AI Level
        if(currentAILevel <= 3){
            currentAILevel += 0.05f;
        }
        //Add New AI Tokens
        addToAITeam();

        //Check to see if the player gets a reward
        if(kiledThisTurn)
        {
            //Give Player his just rewards!
            kiledThisTurn = false;
            rewardManager.showDialog();
        } else{
            executeManager.showDialog();
        }

        StopCoroutine(executeManager.runTurn());
    }

    void addToAITeam(){
        //Team A
        OrbController orbA = spawnOrb();
        orbA.setTeam(1);
        orbA.LevelUpAI(currentAILevel);
        aTeam.Add(orbA);

        //Team B
        OrbController orbB = spawnOrb();
        orbB.setTeam(2);
        orbB.LevelUpAI(currentAILevel);
        bTeam.Add(orbB);
    }

    public void addToPlayerTeam(){
        OrbController orb = spawnOrb();
        orb.setTeam(0);
        playerTeam.Add(orb);
    }

    public void TeamA_Attack(){
        OrbController orb = aTeam[Random.Range(0, aTeam.Count)];
        orb.target = getPlayerTarget();
        orb.FireTowardTarget();
    }

    public void TeamB_Attack()
    {
        OrbController orb = bTeam[Random.Range(0, bTeam.Count)];
        orb.target = getPlayerTarget();
        orb.FireTowardTarget();
    }

    public void Player_Attack(int orbIndex, int power, int angle){
        OrbController orb = playerTeam[orbIndex];
        orb.power = power;
        orb.angle = angle;
        orb.FireInAngle();
    }

    GameObject getPlayerTarget(){
        return playerTeam[Random.Range(0, playerTeam.Count)].gameObject;
    }

    private OrbController spawnOrb(){
        float xPos = Random.Range(xOne_Spawn.transform.position.x, xTwo_Spawn.transform.position.x);
        float yPos = Random.Range(yOne_Spawn.transform.position.y, yTwo_Spawn.transform.position.y);
        return Instantiate(orbPrefab, new Vector3(xPos, yPos, -2), Quaternion.identity).GetComponent<OrbController>();
    }
}
