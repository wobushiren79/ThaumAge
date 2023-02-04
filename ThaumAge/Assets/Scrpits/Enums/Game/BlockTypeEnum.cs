public enum BlockTypeEnum
{
    None = 0,    //����
    Foundation = 1,    //�ػ�

    Sand = 3,    //ɳ��
    Dirt = 4,    //����
    Stone = 5,    //ʯͷ
    StoneMoss = 6, //̦޺ʯ
    StoneVolcanic = 7,//��ɽ��
    StoneIce = 8,//����
    StoneSnow = 9,//ѩ��

    Grass = 101,    //�ݵ�
    GrassMagic = 102,    //��ħ�ݵ�
    GrassWild = 103,    //�Ĳݵ�
    GrassSnow = 104,    //ѩ�ݵ�

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
    OreZinc = 907,//п��

    ElementalCrystalMetalSeed = 981,//Ԫ��ˮ������
    ElementalCrystalWoodSeed = 982,
    ElementalCrystalWaterSeed = 983,
    ElementalCrystalFireSeed = 984,
    ElementalCrystalEarthSeed = 985,
    ElementalCrystalLightSeed = 986,
    ElementalCrystalDarkSeed = 987,

    ElementalCrystalMetal = 991,//��Ԫ��ˮ��
    ElementalCrystalWood = 992,//ľԪ��ˮ��
    ElementalCrystalWater = 993,//ˮԪ��ˮ��
    ElementalCrystalFire = 994,//��Ԫ��ˮ��
    ElementalCrystalEarth = 995,//��Ԫ��ˮ��
    ElementalCrystalLight = 996,//��Ԫ��ˮ��
    ElementalCrystalDark = 997,//��Ԫ��ˮ��
    ElementalCrystalThaum = 999,//����Ԫ��ˮ��

    TreeOak = 1001,//��ľ
    TreeSilver = 1002,//����
    TreeWorld = 1003,//������
    TreeCherry = 1004,//ӣ����ľ
    TreePalm = 1005,//�����ľ
    TreeBirch = 1006,//����ľ
    TreeWalnut = 1007,//������ľ
    TreeMushroom = 1101,//Ģ����

    LeavesOak = 2001, //��ľҶ
    LeavesSilver = 2002,//����Ҷ
    LeavesWorld = 2003,//������Ҷ
    LeavesCherry = 2004,//ӣ��Ҷ
    LeavesPalm = 2005,//���Ҷ
    LeavesBirch = 2006,//����Ҷ
    LeavesWalnut = 2007,//������Ҷ
    LeavesMushroom = 2101,//ĥ����Ҷ��

    Vines = 2201,//����
    Wicker = 2211,//����
    LotusLeaf = 2221,//��Ҷ

    WeedGrassLong = 3001,    //�Ӳݳ�
    WeedGrassNormal = 3002,    //�Ӳ��е�
    WeedGrassShort = 3003,    //�Ӳݶ�
    WeedGrassStart = 3004, //�Ӳݳ���

    WeedWildLong = 3011,    //�Ӳݳ�
    WeedWildNormal = 3012,    //�Ӳ��е�
    WeedWildShort = 3013,    //�Ӳݶ�
    WeedWildStart = 3014, //�Ӳݳ���

    WeedSnowLong = 3021,    //�Ӳݳ�
    WeedSnowNormal = 3022,    //�Ӳ��е�
    WeedSnowShort = 3023,    //�Ӳݶ�
    WeedSnowStart = 3024, //�Ӳݳ���

    WeedMagicLong = 3031,    //�Ӳݳ�
    WeedMagicNormal = 3032,    //�Ӳ��е�
    WeedMagicShort = 3033,    //�Ӳݶ�
    WeedMagicStart = 3034, //�Ӳݳ���

    CoralRed = 3091,//��ɺ��
    CoralBlue = 3092,//��ɺ��
    CoralYellow = 3093,//��ɺ��

    Seaweed = 3101,//����

    FlowerSun = 3111,    //���տ�
    FlowerRose = 3112,    //õ�廨
    FlowerChrysanthemum = 3113,    //�ջ�

    FlowerMetal = 3121,//��ҫ��
    FlowerWood = 3122,//ľֲ��
    FlowerWater = 3123,//ˮ�û�
    FlowerFire = 3124,//���滨
    FlowerEarth = 3125,//���һ�

    MushroomLuminous = 3201,//ҹ��Ģ��
    MushroomRed = 3202,//��Ģ��
    MushroomWhite1 = 3211,//��Ģ��1
    MushroomWhite2 = 3212,//��Ģ��2
    MushroomWhite3 = 3213,//��Ģ��3

    WoodDead = 3301,//��ľ

    Cactus = 4001,    //������

    CropPotato = 5001,//��ֲ ����
    CropCorn = 5002,//��ֲ ����
    CropWheat = 5003,//��ֲ С��
    CropFlax = 5004,//����
    CropChili = 5005,//����
    CropTomato = 5006,//������

    CropWatermelonGrow = 5101,//��ֲ������ ����
    CropWatermelon = 5102,//��ֲ ����

    SaplingOak = 5801,//   ������
    SaplingSilver = 5802,//   ������
    SaplingWorld = 5803,//   ��������
    SaplingCherry = 5804,//   ӣ������
    SaplingPalm = 5805,//   �������
    SaplingBirch = 5806,//   ������
    SaplingWalnut = 5807,//   ��������

    BerryBushGrowRed = 5901,//��ɫ������ - ����
    BerryBushRed = 5902,//��ɫ������
    BerryBushGrowBlue = 5903,//��ɫ������ - ����
    BerryBushBlue = 5904,//��ɫ������

    LadderWood = 6001,//ľ����
    Door = 6010,//��
    CraftingTableSimple = 6101,//��������̨
    CraftingTableArcane = 6111,//��������̨

    FurnacesSimple = 6151,//������¯
    CrankWooden = 6155,//ľ������
    GrinderSimple = 6156,//������ĥ��

    MagicInstrumentAssemblyTable = 6160,//������װ̨
    Crucible = 6161,//����
    ResearchTable = 6162,//�о�̨
    FocalManipulator = 6163,//��������̨
    RechargePedestal = 6164,//�������ܻ���
    WorkbenchCharger = 6165,//����������

    EverfullUrn = 6166,//�޾�֮��
    ElementSmeltery = 6168,//Դ��ұ����
    ArcaneAlembic = 6169,//����������
    ArcaneBellows = 6170,//��������
    ArcaneLevitator = 6171,//��������̨

    Chest = 6201,//����
    ChestHungry = 6210,//��������
    WardedJar = 6211,//Դ�ʹ���

    SwitchesWooden = 6301,//ľ�ƿ���
    TorchWooden = 6311, //ľ�ƻ��
    LanternStone = 6322,//ʯ�Ƶ���
    FrameItem = 6331,//��Ʒչʾ��

    SignWooden = 6341,//ľ������

    FenceWooden = 6351,//ľ��դ��
    FenceIron = 6352,//����դ��

    FenceDoorWooden = 6361,//ľ��դ����
    Bonfire = 6371,//����
    TableWooden = 6381,//ľ��
    TableStone = 6382,//ʯ��
    BedWooden = 6901,//��
    ArmorStand = 6981,//�¼�

    TombstoneRare = 6998,//Ĺ��ϡ��
    Tombstone = 6999,//Ĺ��

    HalfSand = 7003,//ɳ��-��
    HalfDirt = 7004,//����-��
    HalfStone = 7005,//ʯͷ-��
    HalfStoneIce = 7008,//����-��
    HalfStoneSnow = 7009,//ѩ��-��

    HalfFloorOak = 7101,//�����ذ�-��
    HalfFloorSilver = 7102, //�����ذ�-��
    HalfFloorWorld = 7103, //�������ذ�-��
    HalfFloorCherry = 7104,//ӣ���ذ�-��
    HalfFloorPalm = 7105,//��鵵ذ�-��
    HalfFloorBirch = 7106,//�����ذ�-��
    HalfFloorWalnut = 7107,//���ҵذ�-��

    BookshelfOak = 8001, //���-����           
    BookshelfSilver = 8002, //���-����
    BookshelfWorl = 8003,  //���-������

    FloorOak = 8051,//��ľ�ذ�
    FloorSilver = 8052,//�����ذ�
    FloorWorld = 8053,//�������ذ�
    FloorCherry = 8054,//ӣ���ذ�
    FloorPalm = 8055,//��鵵ذ�
    FloorBirch = 8056,//�����ذ�
    FloorWalnut = 8057,//���ҵذ�

    StairsOak = 8101,//��ľ¥��
    StairsSilver = 8102, //����¥��
    StairsWorld = 8103, //������¥��
    StairsCherry = 8104,//ӣ��¥��
    StairsPalm = 8105,//���¥��
    StairsBirch = 8106,//����¥��
    StairsWalnut = 8107,//����¥��

    Glass = 8151,//����

    BricksStone = 8201,//ʯש
    BricksStoneMoss = 8202,//̦޺ʯש
    BricksRed = 8203,//ש��
    BricksRedMoss = 8204,//̦޺ש��
    BricksSand = 8205,//ɳש
    BricksSandMoss = 8206,//̦޺ɳש

    ArcaneStone = 8301,//����ʯ��
    ArcaneStoneBricks = 8302,//����ʯש
    RunicMatrix = 8303,//���ľ���
    ArcanePedestal = 8304,//��������

    PavingStoneOfTravel = 8311,//��������ʯ
    PavingStoneOfWarding = 8312,//��������ʯ
    PavingStoneOfHealth = 8313,//�����ָ�ʯ

    ShineLightWhite = 8401, //��ҫ֮��
    ShineLightRed = 8402,
    ShineLightBlue = 8403,
    ShineLightYellow = 8404,
    ShineLightGreen = 8405,

    Water = 9001,//ˮ
    Magma = 9002,//�ҽ�

    LinkChild = 9981,//�����ӷ���
    LinkLargeChild = 9982,//�����ӷ���(�෽��ṹ)
    TestBlockAnim = 9998,//���Զ�������
    TestBlockDirection = 9999,//���Է��򷽿�

    InfusionAltar = 10001,//עħ��̳
    InfernalFurnace = 10002,//������¯
}
