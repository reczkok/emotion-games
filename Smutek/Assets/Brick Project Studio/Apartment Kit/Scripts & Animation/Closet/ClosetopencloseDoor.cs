using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class ClosetopencloseDoor : MonoBehaviour, Interactable
	{

		public Animator Closetopenandclose;
		public bool open;
		public Transform Player;
		public bool isInteractable = true;

		void Start()
		{
			open = false;
		}

		public void OnAction()
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
			Closetopenandclose.Play("ClosetOpening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			Closetopenandclose.Play("ClosetClosing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}