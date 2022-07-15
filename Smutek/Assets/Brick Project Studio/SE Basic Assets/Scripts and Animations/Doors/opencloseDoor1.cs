using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class opencloseDoor1 : MonoBehaviour, Interactable
	{
		public bool isInteractable = true;
		public Animator openandclose1;
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
				var dialogueComponent = GetComponent<DialogueTrigger>();
				if (dialogueComponent != null)
				{
					dialogueComponent.TriggerDialogue();
				}
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
			openandclose1.Play("Opening 1");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			openandclose1.Play("Closing 1");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}