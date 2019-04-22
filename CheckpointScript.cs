using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
  static CheckpointScript instance;
  public  bool HasCheckPoint = false;
   public bool ResetCheckPoint = false; 
   public Vector3 PlayersFirstPosition; 
   public Vector3 CheckpointPosition;
    // Use this for initialization

    public GameObject Player;
    public GameObject RunTimePlayer; 

    void Start()
    {

        DontDestroyOnLoad(transform.gameObject);
        PlayersFirstPosition = gameObject.transform.position; 

    }

    // Update is called once per frame
    void Update()
    {
        RunTimePlayer = GameObject.Find("player_John");
        Movement MovementScript = RunTimePlayer.GetComponent<Movement>();
       
        if (HasCheckPoint == true && ResetCheckPoint == false)
        {
            ResetCheckPoint = true;
            RunTimePlayer.transform.position = CheckpointPosition;
        }

        if (MovementScript.isDead == true)
        {
            ResetCheckPoint = false; 
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
      

      if (collision.gameObject.tag == "Player")
        {
            CheckpointPosition = gameObject.transform.position;
            HasCheckPoint = true; 
        }
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.

    }
}
