using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLocalizeManager : CSingleton<CLocalizeManager>
{
	private Dictionary<string, string> _oStringDict = null;

	public override void Awake()
	{
		base.Awake();
		_oStringDict = new Dictionary<string, string>();
	}

	public string GetStringForKey(string oKey)
	{
		if (_oStringDict.ContainsKey(oKey))
		{
			return _oStringDict[oKey];
		}
		return oKey;
	}

	public void ResetDict()
	{
		_oStringDict.Clear();
	}

	public void LoadStringListFromFile(string oFilepath)
	{
		this.ResetDict();
		var oStringPairList = CSVParser.ParseFromResource(oFilepath);

		for (int i = 0; i < oStringPairList.Count; ++i)
		{
			var oStringPair = oStringPairList[i];

			string oKey = oStringPair["Key"];
			string oString = oStringPair["Value"];

			_oStringDict.Add(oKey, oString);
		}
	}
}
