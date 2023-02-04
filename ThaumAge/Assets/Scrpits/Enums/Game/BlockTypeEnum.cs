public enum BlockTypeEnum
{
    None = 0,    //空气
    Foundation = 1,    //地基

    Sand = 3,    //沙子
    Dirt = 4,    //泥土
    Stone = 5,    //石头
    StoneMoss = 6, //苔藓石
    StoneVolcanic = 7,//火山岩
    StoneIce = 8,//冰块
    StoneSnow = 9,//雪块

    Grass = 101,    //草地
    GrassMagic = 102,    //蕴魔草地
    GrassWild = 103,    //荒草地
    GrassSnow = 104,    //雪草地

    PloughGrass = 201,//耕地
    PloughMagic = 202,//蕴魔耕地
    PloughWild = 203,//荒耕地

    OreCoal = 900, //煤矿
    OreCopper = 901,//铜矿
    OreIron = 902,//铁矿
    OreSilver = 903,//银矿
    OreGold = 904,//金矿
    OreTin = 905,//锡矿
    OreAluminum = 906,//铝矿
    OreZinc = 907,//锌矿

    ElementalCrystalMetalSeed = 981,//元素水晶种子
    ElementalCrystalWoodSeed = 982,
    ElementalCrystalWaterSeed = 983,
    ElementalCrystalFireSeed = 984,
    ElementalCrystalEarthSeed = 985,
    ElementalCrystalLightSeed = 986,
    ElementalCrystalDarkSeed = 987,

    ElementalCrystalMetal = 991,//金元素水晶
    ElementalCrystalWood = 992,//木元素水晶
    ElementalCrystalWater = 993,//水元素水晶
    ElementalCrystalFire = 994,//火元素水晶
    ElementalCrystalEarth = 995,//土元素水晶
    ElementalCrystalLight = 996,//光元素水晶
    ElementalCrystalDark = 997,//暗元素水晶
    ElementalCrystalThaum = 999,//神秘元素水晶

    TreeOak = 1001,//橡木
    TreeSilver = 1002,//银树
    TreeWorld = 1003,//世界树
    TreeCherry = 1004,//樱花树木
    TreePalm = 1005,//棕榈树木
    TreeBirch = 1006,//桦树木
    TreeWalnut = 1007,//胡桃树木
    TreeMushroom = 1101,//蘑菇树

    LeavesOak = 2001, //橡木叶
    LeavesSilver = 2002,//银树叶
    LeavesWorld = 2003,//世界树叶
    LeavesCherry = 2004,//樱花叶
    LeavesPalm = 2005,//棕榈叶
    LeavesBirch = 2006,//桦树叶
    LeavesWalnut = 2007,//胡桃树叶
    LeavesMushroom = 2101,//磨菇树叶子

    Vines = 2201,//藤蔓
    Wicker = 2211,//柳条
    LotusLeaf = 2221,//荷叶

    WeedGrassLong = 3001,    //杂草长
    WeedGrassNormal = 3002,    //杂草中等
    WeedGrassShort = 3003,    //杂草短
    WeedGrassStart = 3004, //杂草超短

    WeedWildLong = 3011,    //杂草长
    WeedWildNormal = 3012,    //杂草中等
    WeedWildShort = 3013,    //杂草短
    WeedWildStart = 3014, //杂草超短

    WeedSnowLong = 3021,    //杂草长
    WeedSnowNormal = 3022,    //杂草中等
    WeedSnowShort = 3023,    //杂草短
    WeedSnowStart = 3024, //杂草超短

    WeedMagicLong = 3031,    //杂草长
    WeedMagicNormal = 3032,    //杂草中等
    WeedMagicShort = 3033,    //杂草短
    WeedMagicStart = 3034, //杂草超短

    CoralRed = 3091,//红珊瑚
    CoralBlue = 3092,//蓝珊瑚
    CoralYellow = 3093,//黄珊瑚

    Seaweed = 3101,//海草

    FlowerSun = 3111,    //向日葵
    FlowerRose = 3112,    //玫瑰花
    FlowerChrysanthemum = 3113,    //菊花

    FlowerMetal = 3121,//金耀花
    FlowerWood = 3122,//木植花
    FlowerWater = 3123,//水悦花
    FlowerFire = 3124,//火焰花
    FlowerEarth = 3125,//土岩花

    MushroomLuminous = 3201,//夜光蘑菇
    MushroomRed = 3202,//红蘑菇
    MushroomWhite1 = 3211,//白蘑菇1
    MushroomWhite2 = 3212,//白蘑菇2
    MushroomWhite3 = 3213,//白蘑菇3

    WoodDead = 3301,//枯木

    Cactus = 4001,    //仙人掌

    CropPotato = 5001,//种植 土豆
    CropCorn = 5002,//种植 玉米
    CropWheat = 5003,//种植 小麦
    CropFlax = 5004,//亚麻
    CropChili = 5005,//辣椒
    CropTomato = 5006,//西红柿

    CropWatermelonGrow = 5101,//种植生长中 西瓜
    CropWatermelon = 5102,//种植 西瓜

    SaplingOak = 5801,//   橡树苗
    SaplingSilver = 5802,//   银树苗
    SaplingWorld = 5803,//   世界树苗
    SaplingCherry = 5804,//   樱花树苗
    SaplingPalm = 5805,//   棕榈树苗
    SaplingBirch = 5806,//   桦树苗
    SaplingWalnut = 5807,//   胡桃树苗

    BerryBushGrowRed = 5901,//红色浆果丛 - 生长
    BerryBushRed = 5902,//红色浆果丛
    BerryBushGrowBlue = 5903,//蓝色浆果丛 - 生长
    BerryBushBlue = 5904,//蓝色浆果丛

    LadderWood = 6001,//木梯子
    Door = 6010,//门
    CraftingTableSimple = 6101,//简易制造台
    CraftingTableArcane = 6111,//奥术制造台

    FurnacesSimple = 6151,//简易熔炉
    CrankWooden = 6155,//木制曲柄
    GrinderSimple = 6156,//简易研磨机

    MagicInstrumentAssemblyTable = 6160,//法器组装台
    Crucible = 6161,//坩埚
    ResearchTable = 6162,//研究台
    FocalManipulator = 6163,//核心镶饰台
    RechargePedestal = 6164,//奥术充能基座
    WorkbenchCharger = 6165,//奥术充能器

    EverfullUrn = 6166,//无尽之瓮
    ElementSmeltery = 6168,//源质冶炼厂
    ArcaneAlembic = 6169,//奥术蒸馏器
    ArcaneBellows = 6170,//奥术风箱
    ArcaneLevitator = 6171,//奥术悬浮台

    Chest = 6201,//箱子
    ChestHungry = 6210,//饕餮箱子
    WardedJar = 6211,//源质罐子

    SwitchesWooden = 6301,//木制开关
    TorchWooden = 6311, //木制火把
    LanternStone = 6322,//石制灯笼
    FrameItem = 6331,//物品展示框

    SignWooden = 6341,//木制牌子

    FenceWooden = 6351,//木制栅栏
    FenceIron = 6352,//铁制栅栏

    FenceDoorWooden = 6361,//木制栅栏门
    Bonfire = 6371,//篝火
    TableWooden = 6381,//木桌
    TableStone = 6382,//石桌
    BedWooden = 6901,//床
    ArmorStand = 6981,//衣架

    TombstoneRare = 6998,//墓碑稀有
    Tombstone = 6999,//墓碑

    HalfSand = 7003,//沙子-半
    HalfDirt = 7004,//泥土-半
    HalfStone = 7005,//石头-半
    HalfStoneIce = 7008,//冰块-半
    HalfStoneSnow = 7009,//雪块-半

    HalfFloorOak = 7101,//橡树地板-半
    HalfFloorSilver = 7102, //银树地板-半
    HalfFloorWorld = 7103, //世界树地板-半
    HalfFloorCherry = 7104,//樱花地板-半
    HalfFloorPalm = 7105,//棕榈地板-半
    HalfFloorBirch = 7106,//桦树地板-半
    HalfFloorWalnut = 7107,//胡桃地板-半

    BookshelfOak = 8001, //书架-橡树           
    BookshelfSilver = 8002, //书架-银树
    BookshelfWorl = 8003,  //书架-世界树

    FloorOak = 8051,//橡木地板
    FloorSilver = 8052,//银树地板
    FloorWorld = 8053,//世界树地板
    FloorCherry = 8054,//樱花地板
    FloorPalm = 8055,//棕榈地板
    FloorBirch = 8056,//桦树地板
    FloorWalnut = 8057,//胡桃地板

    StairsOak = 8101,//橡木楼梯
    StairsSilver = 8102, //银树楼梯
    StairsWorld = 8103, //世界树楼梯
    StairsCherry = 8104,//樱花楼梯
    StairsPalm = 8105,//棕榈楼梯
    StairsBirch = 8106,//桦树楼梯
    StairsWalnut = 8107,//胡桃楼梯

    Glass = 8151,//玻璃

    BricksStone = 8201,//石砖
    BricksStoneMoss = 8202,//苔藓石砖
    BricksRed = 8203,//砖块
    BricksRedMoss = 8204,//苔藓砖块
    BricksSand = 8205,//沙砖
    BricksSandMoss = 8206,//苔藓沙砖

    ArcaneStone = 8301,//奥术石材
    ArcaneStoneBricks = 8302,//奥术石砖
    RunicMatrix = 8303,//符文矩阵
    ArcanePedestal = 8304,//奥术基座

    PavingStoneOfTravel = 8311,//奥术旅行石
    PavingStoneOfWarding = 8312,//奥术屏障石
    PavingStoneOfHealth = 8313,//奥术恢复石

    ShineLightWhite = 8401, //闪耀之光
    ShineLightRed = 8402,
    ShineLightBlue = 8403,
    ShineLightYellow = 8404,
    ShineLightGreen = 8405,

    Water = 9001,//水
    Magma = 9002,//岩浆

    LinkChild = 9981,//链接子方块
    LinkLargeChild = 9982,//链接子方块(多方块结构)
    TestBlockAnim = 9998,//测试动画方块
    TestBlockDirection = 9999,//测试方向方块

    InfusionAltar = 10001,//注魔祭坛
    InfernalFurnace = 10002,//炼狱熔炉
}
