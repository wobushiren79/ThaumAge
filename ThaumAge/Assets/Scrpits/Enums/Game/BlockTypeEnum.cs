public enum BlockTypeEnum
{
    None = 0,    //��
    Foundation = 1,    //�ػ�
    LinkChild = 2,//�����ӷ���

    Sand = 3,    //ɳ��
    Dirt = 4,    //����
    Stone = 5,    //ʯͷ
    StoneMoss = 6, //̦޺ʯ
    StoneVolcanic = 7,//��ɽ��

    Grass = 101,    //�ݵ�
    GrassMagic = 102,    //��ħ�ݵ�
    GrassWild = 103,    //�Ĳݵ�

    PloughGrass = 201,//����
    PloughMagic = 202,//��ħ����
    PloughWild = 203,//�ĸ���

    OreCoal = 900, //ú��
    OreCopper = 901,//ͭ��
    OreIron = 902,//����
    OreSilver = 903,//����
    OreGold = 904,//���
    OreTin = 905,//����
    OreAluminum = 906,//����

    TreeOak = 1001,//��ľ
    TreeSilver = 1002,//����
    TreeWorld = 1003,//������
    TreeCherry = 1004,//ӣ����ľ
    TreePalm = 1005,//�����ľ
    TreeBirch = 1006,//����ľ
    TreeMushroom = 1101,//Ģ����

    LeavesOak = 2001, //��ľҶ
    LeavesSilver = 2002,//����Ҷ
    LeavesWorld = 2003,//������Ҷ
    LeavesCherry = 2004,//ӣ��Ҷ
    LeavesPalm = 2005,//���Ҷ
    LeavesBirch = 2006,//����Ҷ
    LeavesMushroom = 2101,//ĥ����Ҷ��

    Wicker = 2211,//����
    Vines = 2201,//����

    WeedGrassLong = 3001,    //�Ӳݳ�
    WeedGrassNormal = 3002,    //�Ӳ��е�
    WeedGrassShort = 3003,    //�Ӳݶ�
    WeedWildLong = 3011,    //�Ӳݳ�
    WeedWildNormal = 3012,    //�Ӳ��е�
    WeedWildShort = 3013,    //�Ӳݶ�
    WeedSnowLong = 3021,    //�Ӳݳ�
    WeedSnowNormal = 3022,    //�Ӳ��е�
    WeedSnowShort = 3023,    //�Ӳݶ�
    WeedMagicLong = 3031,    //�Ӳݳ�
    WeedMagicNormal = 3032,    //�Ӳ��е�
    WeedMagicShort = 3033,    //�Ӳݶ�

    FlowerSun = 3111,    //���տ�
    FlowerRose = 3112,    //õ�廨
    FlowerChrysanthemum = 3113,    //�ջ�

    FlowerMetal = 3121,//��ҫ��
    FlowerWood = 3122,//ľֲ��
    FlowerWater = 3123,//ˮ�û�
    FlowerFire = 3124,//���滨
    FlowerEarth = 3125,//���һ�

    MushroomLuminous = 3201,//ҹ��Ģ��
    WoodDead = 3301,//��ľ

    Cactus = 4001,    //������

    CropPotato = 5001,//��ֲ ����
    CropCorn = 5002,//��ֲ ����
    CropWatermelonGrow = 5101,//��ֲ������ ����
    CropWatermelon = 5102,//��ֲ ����

    LadderWood = 6001,//ľ����
    Door = 6010,//��
    CraftingTableSimple = 6101,//��������̨
    FurnacesSimple = 6151,//������¯
    Box = 6201,//����
    SwitchesWooden = 6301,//ľ�ƿ���
    TorchWooden = 6311, //ľ�ƻ��
    LanternStone = 6322,//ʯ�Ƶ���
    FrameItem = 6331,//��Ʒչʾ��

    SignWooden = 6341,//ľ������
    FenceWooden = 6351,//ľ��դ��
    FenceDoorWooden = 6361,//ľ��դ����

    BedWooden = 6901,//��

    BookshelfOak = 8001, //���-����           
    BookshelfSilver = 8002, //���-����
    BookshelfWorl = 8003,  //���-������

    StairsOak = 8101,//��ľ¥��
    StairsSilver = 8102, //����¥��
    StairsWorld = 8103, //������¥��
    StairsCherry = 8104,//ӣ��¥��
    StairsPalm = 8105,//���¥��
    StairsBirch = 8106,//����¥��

    Glass = 8151,//����

    BricksStone = 8201,//ʯש
    BricksStoneMoss = 8202,//̦޺ʯש
    BricksRed = 8203,//ש��
    BricksRedMoss = 8204,//̦޺ש��
    BricksSand = 8205,//ɳש
    BricksSandMoss = 8206,//̦޺ɳש

    HalfSand = 7003,//ɳ��-��
    HalfDirt = 7004,//����-��
    HalfStone = 7005,//ʯͷ-��
    HalfFloorOak = 7101,//�����ذ�-��
    HalfFloorSilver = 7102, //�����ذ�-��
    HalfFloorWorld = 7103, //�������ذ�-��
    HalfFloorCherry = 7104,//ӣ���ذ�-��
    HalfFloorPalm = 7105,//��鵵ذ�-��
    HalfFloorBirch = 7106,//�����ذ�-��

    FloorOak = 8051,//��ľ�ذ�
    FloorSilver = 8052,//�����ذ�
    FloorWorld = 8053,//�������ذ�
    FloorCherry = 8054,//ӣ���ذ�
    FloorPalm = 8055,//��鵵ذ�
    FloorBirch = 8056,//�����ذ�

    Water = 9001,//ˮ
    Magma = 9002,//�ҽ�

    TestBlockAnim = 9998,//���Զ�������
    TestBlockDirection = 9999,//���Է��򷽿�
}
