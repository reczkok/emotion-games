using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseDoor : InteractableObject
	{
		public Animator openandclose;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
		}

		public override void OnAction()
		{
			{
				base.OnAction();
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

		IEnumerator opening()
		{
			print("you are opening the door");
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			openandclose.Play("Closing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}