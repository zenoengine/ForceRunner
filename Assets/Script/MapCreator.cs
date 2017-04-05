using System.Collections;
using UnityEngine;

public class Block
{
    // 블록의 종류를 나타내는 열거체.
    public enum TYPE
    {
        NONE = -1, // 없음.
        FLOOR = 0, // 마루.
        HOLE, // 구멍.
        SPINE_FLOOR,
        NUM, // 블록이 몇 종류인지 나타낸다(＝2).
    };
};
public enum ItemType
{
    Force,
    Coin,
    Bomb
};

public class MapCreator : MonoBehaviour
{
    public TextAsset level_data_text = null;
    private GameRoot game_root = null;

    // MapCreator.cs
    public static float BLOCK_WIDTH = 1.0f; // 블록의 폭.
    public static float SPINE_BLOCK_WIDTH = 2.0f; // 스파인 블록의 폭.
    public static float BLOCK_HEIGHT = 0.2f; // 블록의 높이.
    public static int BLOCK_NUM_IN_SCREEN = 24; // 화면 내에 들어가는 블록의 개수.
    private struct FloorBlock
    { 
        public bool is_created;
        public Vector3 position;
    };
    private struct Item
    {
        public bool is_created;
        public Vector3 position;
    }

    private Item last_item;

    private FloorBlock last_block;
    private PlayerControl player = null;
    private BlockCreator block_creator = null;
    private ItemCreator item_creator = null;
    private LevelControl level_control = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        last_block.is_created = false;
        block_creator = GetComponent<BlockCreator>();
        item_creator = GetComponent<ItemCreator>();
        game_root = GetComponent<GameRoot>();

        level_control = new LevelControl();
        level_control.initialize();
        level_control.loadLevelData(this.level_data_text);
    }

    void Update()
    {
        float block_generate_x = this.player.transform.position.x;
        block_generate_x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN + 1) / 2.0f;
        while (this.last_block.position.x < block_generate_x)
        {
            createMap();
        }
    }

    private void createMap()
    {
        creare_blocks();
        create_coins();
    }
    
    private void create_coins()
    {
        Vector3 item_position;
        if (!this.last_item.is_created)
        {
            item_position = this.player.transform.position;
            item_position.x -= BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
            item_position.y = 0.0f;
        }
        else
        {
            item_position = this.last_item.position;
        }

        LevelControl.CreationInfo current = this.level_control.current_block;
        if(current.block_type == Block.TYPE.HOLE)
        {

        }

        //item_creator.createItem(ItemType.Coin, item_position + new Vector3(0, 2, 0));
    }

    private void creare_blocks()
    {
        Vector3 block_position;
        if (!this.last_block.is_created)
        {
            block_position = this.player.transform.position;
            block_position.x -= BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
            block_position.y = 0.0f;
        }
        else
        {
            block_position = this.last_block.position;
        }

        this.level_control.update(this.game_root.getPlayTime());

        // level_control에 저장된 current_block(지금 만들 블록 정보)의 height(높이)를 씬 상의 좌표로 변환.
        block_position.y = level_control.current_block.height * BLOCK_HEIGHT;
        // 지금 만들 블록에 관한 정보를 변수 current에 넣는다.
        LevelControl.CreationInfo current = this.level_control.current_block;
        // 지금 만들 블록이 바닥이면 (지금 만들 블록이 구멍이라면)
        
        if (current.block_type == Block.TYPE.FLOOR)
        {
            block_position.x += BLOCK_WIDTH;
            block_creator.createBlock(block_position);
        }
        else if(current.block_type == Block.TYPE.SPINE_FLOOR)
        {
            block_position.x += SPINE_BLOCK_WIDTH;
            block_creator.createSpineBlock(block_position);
        }
        else
        {
            block_position.x += BLOCK_WIDTH;
        }

        this.last_block.position = block_position;
        this.last_block.is_created = true;
    }


    public bool isDelete(GameObject block_object)
    {
        bool ret = false;
        float left_limit = this.player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

        if (block_object.transform.position.x < left_limit)
        {
            ret = true; // 반환값을 true(사라져도 좋다)로
        }

        return ret; // 판정 결과를 돌려줌
    }
}