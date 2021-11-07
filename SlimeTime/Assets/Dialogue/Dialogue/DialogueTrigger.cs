using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour {
	public Button yourButton;
	public Dialogue dialogue;
	public Text textField;
	private bool pressed = false;


	void Start () {
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TriggerDialogue);
	}


	public void TriggerDialogue()
	{
		if(pressed){ChangeScene();}
		else {
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
			ChangeButtonText();
			pressed = true;
		}
	}

	private void ChangeButtonText() {
		textField.text = "Continue";
	}

	private void ChangeScene() { 
		Debug.Log("Not imlemented");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

}
