using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableFlipL: MonoBehaviour, Interactable {

	public Animator FlipL;
	public bool open;
	public Transform Player;
	public bool isInteractable = true;

	void Start (){
		open = false;
	}

	public bool IsInteractable()
    {
		return isInteractable;
    }

	public void OnAction (){
		{
			if (Player)
			{
				if (open == false)
				{
					StartCoroutine(opening());
				}
				else
				{
					if (open == true)
					{
						StartCoroutine(closing());
					}

				}

			}

		}

	}

	IEnumerator opening(){
		print ("you are opening the door");
        FlipL.Play ("Lup");
		open = true;
		yield return new WaitForSeconds (.5f);
	}

	IEnumerator closing(){
		print ("you are closing the door");
        FlipL.Play ("Ldown");
		open = false;
		yield return new WaitForSeconds (.5f);
	}


}

