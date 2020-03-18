using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class GoCountDict : SerializableDictionary<GameObject, int> {
    
}

public class TreasureHunter : MonoBehaviour
{

    public Collectible[] allCollectibles;
    public KeyCode[] keyCodes = new KeyCode[]{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3};
    private List<Collectible> collectibleTreasures;

    // OVRCameraRig oVRCameraRig;
    // OVRManager oVRManager;
    // OVRHeadsetEmulator oVRHeadsetEmulator;

    Camera viewpointCamera;
    float currentTotalScore;
    public TextMesh scoreText;
    public TextMesh winText;

    


    //Stuff for a04, starting with movement. Making a movespeed variable
    public float moveSpeed = 100f;
    public float inputHorizontal;
    //LayerMask taken from example code
    public LayerMask collectiblesMask;
    Collectible thingIGrabbed;
    
    // Start is called before the first frame update
    void Start()
    {
        allCollectibles = GameObject.FindObjectsOfType<Collectible>();
       // collectibleTreasures = this.gameObject.GetComponent<TreasureHunterInventory>().inventoryItems;
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < keyCodes.Length; x++){
            KeyCode code = keyCodes[x];
            if (Input.GetKeyDown(code) && !collectibleTreasures.Contains(allCollectibles[x])){
                collectibleTreasures.Add(allCollectibles[x]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)){
            scoreText.text = "Elliot Melfi says... Score: " + calculateScore().ToString() + " Collectibles: " + collectibleTreasures.Count;
        }

        //For movement, get the initial position then check which key is pressed.
        inputHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.W)){
            transform.position = transform.position + Camera.main.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)){
            transform.position = transform.position - Camera.main.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
            transform.Rotate(new Vector3(0f, inputHorizontal * Time.deltaTime * 100f, 0f));
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            scoreText.text = "Space bar has been pressed.";
            laserCollect();
            winText.text = "Score is: " + calculateScore().ToString();
        }
    }

    //Copied from / inspired by example code, especially the forceGrab method
    public void laserCollect() {
        RaycastHit outHit;
        if (Physics.Raycast(transform.position, transform.forward, out outHit, 100.0f, collectiblesMask)){
            thingIGrabbed = outHit.collider.gameObject.GetComponent<Collectible>();
            if (thingIGrabbed != null){
                scoreText.text = thingIGrabbed.name;
                if (!collectibleTreasures.Contains(thingIGrabbed)){
                    collectibleTreasures.Add(thingIGrabbed);
                }
                if (thingIGrabbed.treasureValue == 10){
                    
                }
            }
            
        }
    }

    float calculateScore() {

        float totalScore = 0;
        foreach (Collectible treasure in collectibleTreasures){
            if (treasure != null){
                totalScore+=treasure.treasureValue;
            }
        }
        this.currentTotalScore = totalScore;
        return totalScore;

    }
}
