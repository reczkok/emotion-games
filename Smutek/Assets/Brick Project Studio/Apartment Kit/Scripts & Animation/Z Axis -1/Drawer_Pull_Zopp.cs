using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class Drawer_Pull_Zopp : MonoBehaviour, Interactable
	{
		public bool isInteractable = true;
		public Animator pull;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
		}

		public void  OnAction()
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

		public bool IsInteractable()
        {
			return isInteractable;
        }

		IEnumerator opening()
		{
			print("you are opening the door");
			pull.Play("openpullopp");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			pull.Play("closepushopp");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}