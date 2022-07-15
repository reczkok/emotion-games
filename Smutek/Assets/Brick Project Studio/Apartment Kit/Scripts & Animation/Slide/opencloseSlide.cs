using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseSlide : MonoBehaviour, Interactable
	{

		public Animator openandclosewindow;
		public bool open;
		public bool isInteractable = true;
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
			openandclosewindow.Play("OpeningSlide");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the Window");
			openandclosewindow.Play("ClosingSlide");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}