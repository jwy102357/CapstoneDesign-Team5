using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CStringTyping : MonoBehaviour
{
	public string resultString;
	public Text textString;
	void Awake()
	{
		textString = this.GetComponent<Text>();
		StartCoroutine(TypingString(resultString));
	}

	IEnumerator TypingString(string result)
	{
		for (int i = 0; i < result.Length; i++)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				textString.text = "";
				textString.text = result;
				yield break;
			}
			
			textString.text += result[i];
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}
}
