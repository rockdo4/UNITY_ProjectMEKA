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
            // current player의 배치 가능 타일 표시
            Debug.Log("캐릭터인포 타일 메쉬 변경");
            foreach (var tile in stageManager.currentPlayer.arrangableTiles)
            {
                tile.SetTileMaterial(Tile.TileMaterial.Arrange);
            }
            arrangeModeSet = true;
        }
    }
}
