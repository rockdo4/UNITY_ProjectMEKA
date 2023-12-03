using UnityEngine;

public class CharacterInfoUIManager : MonoBehaviour
{
    public Defines.CharacterInfoMode windowMode;
    private StageManager stageManager;
    public bool arrangeModeSet;
    public bool settingModeSet;

    private void Awake()
    {
        stageManager = stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
    }

    private void Update()
    {
        if(windowMode == Defines.CharacterInfoMode.Arrange && !arrangeModeSet)
        {
            // current player�� ��ġ ���� Ÿ�� ǥ��
            Debug.Log("ĳ�������� Ÿ�� �޽� ����");
            foreach (var tile in stageManager.currentPlayer.arrangableTiles)
            {
                tile.SetTileMaterial(Tile.TileMaterial.Arrange);
            }
            arrangeModeSet = true;
        }
    }
}
