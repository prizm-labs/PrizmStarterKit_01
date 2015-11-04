using UnityEngine;
using System.Collections;
using TouchScript;
using TouchScript.Gestures;
using System;

//Handles player's input to show cards, fold, and tabletop's input to recall cards
public class HandManager : MonoBehaviour {

	private bool showingCards = false;	//if cards are face down and the player has to hold to show them
	private bool flashingCards = false;

	private void OnEnable()
	{
		GetComponent<FlickGesture>().Flicked += HandleFlicked;		//flicking the cards folds
		GetComponent<TapGesture>().Tapped += HandleTapped;			//doubletap toggles showing cards
		GetComponent<LongPressGesture>().LongPressed += HandlePressed;		//pressing shows cards temporarily
		GetComponent<ReleaseGesture>().Released += HandleReleased;	//release hides cards again
	}

	private void OnDisable()
	{
		GetComponent<FlickGesture>().Flicked -= HandleFlicked;
		GetComponent<TapGesture>().Tapped -= HandleTapped;
		GetComponent<LongPressGesture>().LongPressed -= HandlePressed;
		GetComponent<ReleaseGesture>().Released -= HandleReleased;
	}

	//hide all cards
	void HandleReleased (object sender, EventArgs e)
	{
		if (flashingCards) {
			Debug.Log ("Released");
			if (showingCards == false) {
				foreach (Transform child in transform) {
					if (child.gameObject.tag == "card" && child.gameObject.GetComponent<PlayingCard>().cardFacingUp) {
						child.gameObject.transform.Rotate (new Vector3 (0, 0, 180));
						child.gameObject.GetComponent<PlayingCard>().cardFacingUp = false;
					}
				}
			}
			flashingCards = false;
		}
	}

	//show all cards
	void HandlePressed (object sender, EventArgs e)
	{
		Debug.Log ("pressed");
		if (showingCards == false) {
			foreach (Transform child in transform) {
				if (child.gameObject.tag == "card" && !child.gameObject.GetComponent<PlayingCard>().cardFacingUp) {
					child.gameObject.transform.Rotate (new Vector3 (0, 0, 180));
					child.gameObject.GetComponent<PlayingCard>().cardFacingUp = true;
				}
			}
		}
		flashingCards = true;
	}

	//(Double)tap - Toggle between showing/not showing cards
	void HandleTapped (object sender, EventArgs e)
	{
		Debug.Log ("(double)tapped");
		Debug.Log ("double tapped, showing cards: " + showingCards);
		if (!showingCards) {
			foreach (Transform child in transform) {
				if (child.gameObject.tag == "card" && !child.gameObject.GetComponent<PlayingCard>().cardFacingUp) {
					child.gameObject.transform.Rotate (new Vector3 (0, 0, 180));
					child.gameObject.GetComponent<PlayingCard>().cardFacingUp = true;
				}
			}
			showingCards = true;
			Debug.Log ("now it's true");
		} else {
			showingCards = false;
			Debug.Log ("Now its false");
			foreach (Transform child in transform) {
				if (child.gameObject.tag == "card" && child.gameObject.GetComponent<PlayingCard>().cardFacingUp) {
					child.gameObject.transform.Rotate (new Vector3 (0, 0, 180));
					child.gameObject.GetComponent<PlayingCard>().cardFacingUp = false;
				}
			}
		}
	}

	//flicking signals folding
	void HandleFlicked (object sender, EventArgs e)
	{
		Debug.Log ("flicked! with showing cards: " + showingCards);
		if (!showingCards) {
			Debug.Log ("Folding");
			Fold ();
		} else {
			Flip ();
			Debug.Log ("sending cards to 'flip'");
		}
	}

	//sends the local card to the handheld's local discard pile to hide the card
	//syncs the database returning the card to 'discardPile'
	public void Fold() {
		foreach(Transform child in transform) {
			if (child.gameObject.tag == "card") {
				child.gameObject.GetComponent<physicalCard>().moveToTarget(GameObject.Find ("Discard"));
				child.gameObject.GetComponent<PlayingCard>().Fold();	//syncs the database
			}
		}
	}

	//sends the local card to the handheld's local discard pile to hide the card
	//syncs the database returning the card to 'flip'
	public void Flip() {
		foreach(Transform child in transform) {
			if (child.gameObject.tag == "card") {
				child.gameObject.GetComponent<physicalCard>().moveToTarget(GameObject.Find ("Discard"));
				child.gameObject.GetComponent<PlayingCard>().Flip();	//syncs the database
			}
		}
	}

	//sends the playing card to 'deck' so that the tabletop can reshuffle
	public void Recall() {
		Debug.Log ("Recalling");
		foreach(Transform child in transform) {
			if (child.gameObject.tag == "card") {
				child.gameObject.GetComponent<physicalCard>().moveToTarget(GameObject.Find ("Discard"));
				//StartCoroutine(child.gameObject.GetComponent<PlayingCard>().Detonate(5));	//removes object from associates list
				//child.gameObject.GetComponent<PlayingCard>().SendTo("deck");	//syncs the database
				//StartCoroutine(child.gameObject.GetComponent<PlayingCard>().Detonate(5.0f));
			}
		}
	}

	public void resetShowingCards() {
		showingCards = false;
	}
}
