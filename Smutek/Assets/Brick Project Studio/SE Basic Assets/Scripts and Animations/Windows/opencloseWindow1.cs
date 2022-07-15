using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseWindow1 : MonoBehaviour, Interactable
	{
		public bool isInteractable = true;
		public Animator openandclosewindow1;
		public bool open;
		public Transform Player;

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
			print("you are opening the Window");
			openandclosewindow1.Play("Openingwindow 1");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the Window");
			openandclosewindow1.Play("Closingwindow 1");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}