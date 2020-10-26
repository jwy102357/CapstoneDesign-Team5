using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class CEmailSend : MonoBehaviour
{
	public void OnClickButtonEvent(Text text)
	{
		string myCompanyEmail = "ljw10827@naver.com";
		string myEmailTitle = this.EscapeURL("버그 리포트 및 문의 사항");
		string myEmailContentsString;
		StringBuilder myEmailContents = new StringBuilder();
		myEmailContents.Append("내용 : \n\n\n");
		myEmailContents.Append("Device Model : ");
		myEmailContents.Append(SystemInfo.deviceModel);
		myEmailContents.Append("\n\n");
		myEmailContents.Append("Device OS : ");
		myEmailContents.Append(SystemInfo.operatingSystem);
		myEmailContents.Append("\n\n");
		myEmailContents.Append("내용 : \n\n");
		myEmailContents.Append(text.text);

		myEmailContentsString = this.EscapeURL(myEmailContents.ToString());
		myEmailContents.Clear();

		myEmailContents.Append("mailto:");
		myEmailContents.Append(myCompanyEmail);
		myEmailContents.Append("?subject=");
		myEmailContents.Append(myEmailTitle);
		myEmailContents.Append("&body=");
		myEmailContents.Append(myEmailContentsString);

		Debug.Log(myEmailContentsString);
		Debug.Log(myEmailContents.ToString());

		Application.OpenURL(this.EscapeURL(myEmailContents.ToString()));

		Debug.Log(this.EscapeURL(myEmailContents.ToString()));
		this.gameObject.SetActive(false);

	}

	private string EscapeURL (string url)
	{
		return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
	}
}
