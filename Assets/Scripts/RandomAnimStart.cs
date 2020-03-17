using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimStart : MonoBehaviour {

	void Start() {
		Animator animator = this.GetComponent<Animator>();
		if (animator)
			animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, -1, Random.value);
			// animator.playbackTime = Random.value;
	}
}
