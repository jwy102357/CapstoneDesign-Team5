using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public enum EMatType
{
	MATERIAL1 = 1,
	MATERIAL2,
	MATERIAL3,
	MATERIAL4,
    MATERIAL5,
    MATERIAL6 = 6,
}

public class CMixManager : CSingleton<CMixManager>
{
	public struct STMaterial
	{
		public int material1;
		public int material2;
		public int material3;
		public int material4;
        public int material5;
        public int material6;


        public STMaterial(int nMat1 = 0, int nMat2 = 0, int nMat3 = 0, int nMat4 = 0, int nMat5 = 0, int nMat6 = 0)
		{
			material1 = nMat1;
			material2 = nMat2;
			material3 = nMat3;
            material4 = nMat4;
            material5 = nMat5;
            material6 = nMat6;
        }
	}

	STMaterial playerMaterial;
    Sprite[] materialSprite;

	private void Awake()
	{
		//세이브된 데이터 가져와서 구조체에 넣기
		this.LoadMaterials();
        materialSprite = new Sprite[6];
        materialSprite[0] = Resources.Load<Sprite>("Sprites/Item/Battery");
        materialSprite[1] = Resources.Load<Sprite>("Sprites/Item/Bolt");
        materialSprite[2] = Resources.Load<Sprite>("Sprites/Item/IronOre");
        materialSprite[3] = Resources.Load<Sprite>("Sprites/Item/IronPlate");
        materialSprite[4] = Resources.Load<Sprite>("Sprites/Item/Nut");
        materialSprite[5] = Resources.Load<Sprite>("sprites/Item/OldAlienGun");

    }

	public void PlayerMaterialMinus(EMatType materialType, int amount)
	{
		switch (materialType)
		{
			case EMatType.MATERIAL1:
				playerMaterial.material1 -= amount;
				if (playerMaterial.material1 < 0) playerMaterial.material1 = 0;
				break;
			case EMatType.MATERIAL2:
				playerMaterial.material2 -= amount;
				if (playerMaterial.material2 < 0) playerMaterial.material2 = 0;
				break;
			case EMatType.MATERIAL3:
				playerMaterial.material3 -= amount;
				if (playerMaterial.material3 < 0) playerMaterial.material3 = 0;
				break;
			case EMatType.MATERIAL4:
				playerMaterial.material4 -= amount;
				if (playerMaterial.material4 < 0) playerMaterial.material4 = 0;
				break;
            case EMatType.MATERIAL5:
                playerMaterial.material5 -= amount;
                if (playerMaterial.material5 < 0) playerMaterial.material5 = 0;
                break;
            case EMatType.MATERIAL6:
                playerMaterial.material6 -= amount;
                if (playerMaterial.material6 < 0) playerMaterial.material6 = 0;
                break;
        }
		this.SaveMaterials();
	}

	public void PlayerMaterialMinus(STMaterial materialStruct)
	{
		playerMaterial.material1 = playerMaterial.material1 >= materialStruct.material1 ? playerMaterial.material1 - materialStruct.material1 : 0;
		playerMaterial.material2 = playerMaterial.material2 >= materialStruct.material2 ? playerMaterial.material2 - materialStruct.material2 : 0;
		playerMaterial.material3 = playerMaterial.material3 >= materialStruct.material3 ? playerMaterial.material3 - materialStruct.material3 : 0;
		playerMaterial.material4 = playerMaterial.material4 >= materialStruct.material4 ? playerMaterial.material4 - materialStruct.material4 : 0;
        playerMaterial.material5 = playerMaterial.material5 >= materialStruct.material5 ? playerMaterial.material5 - materialStruct.material5 : 0;
        playerMaterial.material6 = playerMaterial.material6 >= materialStruct.material6 ? playerMaterial.material6 - materialStruct.material6 : 0;
        this.SaveMaterials();
	}

	public void PlayerMaterialMinus(int mat1Val, int mat2Val, int mat3Val, int mat4Val, int mat5Val, int mat6Val)
	{
		playerMaterial.material1 = playerMaterial.material1 >= mat1Val ? playerMaterial.material1 - mat1Val : 0;
		playerMaterial.material2 = playerMaterial.material2 >= mat2Val ? playerMaterial.material2 - mat2Val : 0;
		playerMaterial.material3 = playerMaterial.material3 >= mat3Val ? playerMaterial.material3 - mat3Val : 0;
		playerMaterial.material4 = playerMaterial.material4 >= mat4Val ? playerMaterial.material4 - mat4Val : 0;
        playerMaterial.material5 = playerMaterial.material5 >= mat5Val ? playerMaterial.material5 - mat5Val : 0;
        playerMaterial.material6 = playerMaterial.material6 >= mat6Val ? playerMaterial.material6 - mat6Val : 0;
        this.SaveMaterials();

	}

	public void PlayerMaterialClear()
	{
		playerMaterial = new STMaterial();
		this.SaveMaterials();

	}

	public void PlayerMaterialPlus(int mat1Val, int mat2Val, int mat3Val, int mat4Val, int mat5Val, int mat6Val)
	{
		playerMaterial.material1 += mat1Val;
		playerMaterial.material2 += mat2Val;
		playerMaterial.material3 += mat3Val;
		playerMaterial.material4 += mat4Val;
        playerMaterial.material5 += mat5Val;
        playerMaterial.material6 += mat6Val;
        this.SaveMaterials();
	}

	public void PlayerMaterialPlus(STMaterial materialStruct)
	{
		playerMaterial.material1 += materialStruct.material1;
		playerMaterial.material2 += materialStruct.material2;
		playerMaterial.material3 += materialStruct.material3;
		playerMaterial.material4 += materialStruct.material4;
        playerMaterial.material5 += materialStruct.material5;
        playerMaterial.material6 += materialStruct.material6;
        this.SaveMaterials();
	}

	public void PlayerMaterialPlus(EMatType materialType, int amount)
	{
		switch (materialType)
		{
			case EMatType.MATERIAL1:
				playerMaterial.material1 += amount;
				break;
			case EMatType.MATERIAL2:
				playerMaterial.material2 += amount;
				break;
			case EMatType.MATERIAL3:
				playerMaterial.material3 += amount;
				break;
			case EMatType.MATERIAL4:
				playerMaterial.material4 += amount;
				break;
            case EMatType.MATERIAL5:
                playerMaterial.material5 += amount;
                break;
            case EMatType.MATERIAL6:
                playerMaterial.material6 += amount;
                break;
        }
		this.SaveMaterials();
	}

	public STMaterial GetPlayerMaterial()
	{
		return playerMaterial;
	}

	public int GetMaterial(EMatType materialType)
	{
		int returnValue = 0;
		switch (materialType)
		{
			case EMatType.MATERIAL1:
				returnValue = playerMaterial.material1;
				break;
			case EMatType.MATERIAL2:
				returnValue = playerMaterial.material2;
				break;
			case EMatType.MATERIAL3:
				returnValue = playerMaterial.material3;
				break;
			case EMatType.MATERIAL4:
				returnValue = playerMaterial.material4;
				break;
            case EMatType.MATERIAL5:
                returnValue = playerMaterial.material5;
                break;
            case EMatType.MATERIAL6:
                returnValue = playerMaterial.material6;
                break;
        }

		return returnValue;
	}

	public bool IsMakeCondition(STMaterial stMat)
	{
		if (playerMaterial.material1 - stMat.material1 < 0) return false;
		if (playerMaterial.material2 - stMat.material2 < 0) return false;
		if (playerMaterial.material3 - stMat.material3 < 0) return false;
		if (playerMaterial.material4 - stMat.material4 < 0) return false;
        if (playerMaterial.material5 - stMat.material5 < 0) return false;
        if (playerMaterial.material6 - stMat.material6 < 0) return false;

        return true;
	}

	public bool IsAmountCondition(EMatType materialType, int materialAmount)
	{
		switch (materialType)
		{
			case EMatType.MATERIAL1:
				if (playerMaterial.material1 >= materialAmount)
				{
					return true;
				}
				break;
			case EMatType.MATERIAL2:
				if (playerMaterial.material2 >= materialAmount)
				{
					return true;
				}
				break;
			case EMatType.MATERIAL3:
				if (playerMaterial.material3 >= materialAmount)
				{
					return true;
				}
				break;
			case EMatType.MATERIAL4:
				if (playerMaterial.material4 >= materialAmount)
				{
					return true;
				}
				break;
            case EMatType.MATERIAL5:
                if (playerMaterial.material5 >= materialAmount)
                {
                    return true;
                }
                break;
            case EMatType.MATERIAL6:
                if (playerMaterial.material6 >= materialAmount)
                {
                    return true;
                }
                break;
        }

		return false;
	}

	private void SaveMaterials()
	{
		ES3.Save<STMaterial>("PlayerMaterials", this.playerMaterial, "PlayerMaterials.es3");
	}

	private void LoadMaterials()
	{
		if (!ES3.KeyExists("PlayerMaterials", "PlayerMaterials.es3"))
		{
			this.SaveMaterials();
		}

		this.playerMaterial = ES3.Load<STMaterial>("PlayerMaterials", "PlayerMaterials.es3");
	}

    public Sprite GetMaterialSprite(int index)
    {
        return materialSprite[index];
    }
}
