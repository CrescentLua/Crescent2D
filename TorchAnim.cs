using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchAnim : MonoBehaviour {

    public Animator animator; 
	// Use this for initialization
	void Start () {
        animator.Play("Torch_Animation");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
