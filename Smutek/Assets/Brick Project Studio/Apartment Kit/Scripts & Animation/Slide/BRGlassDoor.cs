using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class BRGlassDoor : MonoBehaviour
	{

		public Animator openandclose;
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
			print("you are opening");
			openandclose.Play("BRGlassDoorOpen");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing");
			openandclose.Play("BRGlassDoorClose");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}