// assigning ints to the enums
// so that the order doesn't get messed up
// if you decide to add a new enum
// inbetween existing ones.
// MAKE SURE TO ALWAYS ASSIGN A NEW INT TO AN ENUM WHEN CREATING IT!

public enum LogChannel
{
    GENERAL = 0,
    LOGGER = 1,
    PLAYER = 2,
    ITEM = 3,
    INVENTORY = 4,
    INGREDIENT = 5,
    ASSEMBLY_TABLE = 6,

    /// <summary>
    ///     Hierarchical State Machine
    /// </summary>
    HSM = 7,
    INTERACTION_SYSTEM = 8,
    SERVING_STATION = 9,
    DRINK_DISPENSER = 10,
    SPLINE = 11,
    CUSTOMER_STATES = 12,
    CUSTOMER_ORDER = 13,
    COOKING_STATE = 14,
    PREFAB_FACTORY = 15,
    CUTTING_STATION = 16,
    MAIN_MENU = 17,
    TABLES = 18,
    DAY_SYSTEM = 19,
    INVENTORY_DISPLAYER = 20,
    FRYING_STATION = 21,
    UTILITY_SCRIPTS = 22,
    TOGGLEABLE_OUTLINE = 23,
    CROSSHAIR = 24,
    INGREDIENT_ASSEMBLER = 25,
    COOKING_AREA = 26,
    TOGGLEABLE_OUTLINE_GROUP = 27,
    RECIPE = 28,
    TUTORIAL,
    REPUTATION_BARS = 100,
    SUBTITLES = 101,
    CUSTOMER_MANAGER = 102,
    PHONE = 103,
    VFX = 104,
}