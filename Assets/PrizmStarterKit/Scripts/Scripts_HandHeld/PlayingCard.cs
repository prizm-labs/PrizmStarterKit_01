using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript;

public static class Enums
{
	public enum Suit { H=0,D,C,S, Count }
	public enum Number { _JOKER=0,_2,_3,_4,_5,_6,_7,_8,_9,_10,_J,_Q,_K,_A,Count }
	public enum Back { Red=0,Blue }
	public enum Face { Simple=0,Traditional }
	public enum Chip { BlackChip=0,BlueChip,GreenChip,OrangeChip,RedChip,WhiteBlackChip,WhiteBlueChip,WhiteGreenChip,WhiteRedChip } 

}

public class PlayingCard : PrizmRecord{
	
	Enums.Suit suit = Enums.Suit.S;
	Enums.Number number = Enums.Number._A;
	Enums.Back back = Enums.Back.Red;	

	GameObject card;

	public bool cardFacingUp = false;
	
	protected override void Awake()
	{
		base.Awake ();		//call base.Awake() to initialize PrizmRecord::Awake()
		Refresh();			//shows the right face of a card
	}

	protected override void Start ()
	{
		base.Start ();		//call base.Start() to initialize PrizmRecord::Start()
							//for now, this automatically adds the PrizmRecord to PrizmRecordGroup
	}

	//<----------> Functions manage database operations <---------->//
	public void Fold() {
		dbEntry.location = "discardPile";
		needsUpdate = true;
		StartCoroutine (GameObject.Find ("GameManager").GetComponent<PrizmRecordGroup>().SyncAll());
		GameObject.Find ("GameManager").GetComponent<PrizmRecordGroup> ().RemoveRecord (this);
	}

	public void Flip() {
		dbEntry.location = "flip";
		needsUpdate = true;
		StartCoroutine (GameObject.Find ("GameManager").GetComponent<PrizmRecordGroup>().SyncAll());
		GameObject.Find ("GameManager").GetComponent<PrizmRecordGroup> ().RemoveRecord (this);
	}
	
	public void SendTo(string recallToLocation) {
		dbEntry.location = recallToLocation;
		needsUpdate = true;
		StartCoroutine (GameObject.Find ("GameManager").GetComponent<PrizmRecordGroup>().SyncAll());

	}

	//<----------> Functions to manage card's face <---------->//
	public Enums.Back Back
	{
		get { return back; }
		set { back = value; }
	}
	
	public Enums.Suit Suit
	{
		get { return suit; }
		set { suit = value; }
	}
	
	public Enums.Number Number
	{
		get { return number; }
		set { number = value; }
	}
	
	public void ChangeToRandom(bool ignoreJoker)
	{

		Enums.Suit changeSuit = (Enums.Suit) UnityEngine.Random.Range(0, ((int) Enums.Suit.Count) - 1);
		Enums.Number changeNumber;
		
		if (ignoreJoker)
		{
			changeNumber = (Enums.Number) UnityEngine.Random.Range(1, ((int) Enums.Suit.Count) - 2);
		}
		else
		{
			changeNumber = (Enums.Number) UnityEngine.Random.Range(0, ((int) Enums.Suit.Count) - 1);
		}
	
		Change(changeSuit,changeNumber);
	
	}
	
	public void Change(Enums.Suit cardSuit, Enums.Number cardNumber)
	{
	
	   suit = cardSuit;
	   number = cardNumber;
	
	   Refresh();
	
	}
	
	public void Style(Enums.Back styleBack)
	{
	
		back = styleBack;
		
		Refresh(); 
		
	}
	
	public void Refresh()
	{
	
		string findCard = Enum.GetName(typeof(Enums.Back), back) + Enum.GetName(typeof(Enums.Number), number) + Enum.GetName(typeof(Enums.Suit), suit);

		if (card)
		{
			
			card.GetComponent<Renderer>().enabled = false;
		
		}

				
		Transform foundCard = this.transform.FindChild(findCard);
		
		if (foundCard)
		{
			
			card = foundCard.gameObject; 
			card.GetComponent<Renderer>().enabled = true;
		}
	}
}
