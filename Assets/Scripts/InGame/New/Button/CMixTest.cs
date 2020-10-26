using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMixTest : MonoBehaviour
{
	public void Mat1Plus()
	{
		CMixManager.Instance.PlayerMaterialPlus(EMatType.MATERIAL1, 1);
	}

	public void Mat2Plus()
	{
		CMixManager.Instance.PlayerMaterialPlus(EMatType.MATERIAL2, 1);
	}

	public void Mat3Plus()
	{
		CMixManager.Instance.PlayerMaterialPlus(EMatType.MATERIAL3, 1);
	}

	public void Mat4Plus()
	{
		CMixManager.Instance.PlayerMaterialPlus(EMatType.MATERIAL4, 1);
	}

	public void MatClear()
	{
		CMixManager.Instance.PlayerMaterialClear();
	}
}
