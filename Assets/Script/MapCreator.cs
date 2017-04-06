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
public enum ItemBlockType
{
    Force,
    Coin,
    Space,
};

public enum MonsterType
{
    None,
    Gumba,
}


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
    private MonsterCreator monster_creator = null;
    private LevelControl level_control = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        last_block.is_created = false;
        block_creator = GetComponent<BlockCreator>();
        item_creator = GetComponent<ItemCreator>();
        monster_creator = GetComponent<MonsterCreator>();
        game_root = GetComponent<GameRoot>();

        level_control = new LevelControl();
        level_control.initialize();
        level_control.loadLevelData(this.level_data_text);
    }

    void Update()
    {
        createMap();
    }

    private void createMap()
    {

        float block_generate_x = this.player.transform.position.x;
        block_generate_x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN + 1) / 2.0f;

        while (this.last_block.position.x < block_generate_x)
        {
            level_control.update(this.game_root.getPlayTime());

            creare_blocks();

            if (level_control.current_block.block_type == Block.TYPE.FLOOR)
            {
                level_control.update(this.game_root.getPlayTime());
                create_monster();
            }
        }
        while (this.last_item.position.x < block_generate_x)
        {
            level_control.update(this.game_root.getPlayTime());

            create_item();
        }
    }

    private void create_monster()
    {
        LevelControl.CreationInfo current_block = this.level_control.current_block;
        //HACK : Second line for monster proper position
        if (level_control.current_monster.monster_type == MonsterType.Gumba &&
            current_block.current_count >= current_block.max_sequnce_count * Random.Range(0.1f, 0.9f))
        {
            Vector3 monster_position = last_block.position + new Vector3(0, 1, 0);
            monster_creator.createMonster(monster_position);
            level_control.current_monster.current_count++;
        }
    }

    private void create_item()
    {

        Vector3 item_position;
        if (!this.last_item.is_created)
        {
            item_position = this.player.transform.position;
            item_position.x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
            item_position.y = 3;
        }
        else
        {
            item_position = this.last_item.position;
        }

        item_position.y = level_control.current_block.height * BLOCK_HEIGHT + 3;
        ItemBlockType currentType = level_control.current_item.item_type;
        if (currentType != ItemBlockType.Space)
        {
            item_creator.create_item(currentType, item_position);
            last_item.is_created = true;
        }

        level_control.current_item.current_count++;
        item_position.x += BLOCK_WIDTH;
        last_item.position = item_position;
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

        
        block_position.y = level_control.current_block.height * BLOCK_HEIGHT;
        
        LevelControl.CreationInfo current = this.level_control.current_block;
        
        if (current.block_type == Block.TYPE.FLOOR)
        {
            block_position.x += BLOCK_WIDTH;
            block_creator.CreateBlock(block_position);
        }
        else if(current.block_type == Block.TYPE.SPINE_FLOOR)
        {
            block_position.x += SPINE_BLOCK_WIDTH - BLOCK_WIDTH*0.5f;
            block_creator.CreateSpineBlock(block_position);
            block_position.x += BLOCK_WIDTH * 0.5f;
        }
        else
        {
            block_position.x += BLOCK_WIDTH;
        }
        level_control.current_block.current_count++;
        this.last_block.position = block_position;
        this.last_block.is_created = true;
    }


    public bool is_delete(GameObject game_object)
    {
        bool ret = false;
        float left_limit = this.player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

        if (game_object.transform.position.x < left_limit)
        {
            ret = true; // 반환값을 true(사라져도 좋다)로
        }

        return ret; // 판정 결과를 돌려줌
    }
}