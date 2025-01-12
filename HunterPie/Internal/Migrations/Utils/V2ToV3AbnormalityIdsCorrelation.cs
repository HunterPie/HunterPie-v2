using System.Collections.Generic;

namespace HunterPie.Internal.Migrations.Utils;

internal class V2ToV3AbnormalityIdsCorrelation : Dictionary<string, string>
{
    public V2ToV3AbnormalityIdsCorrelation()
    {
        Add("88-10", "ABN_MIGHT_SEED");
        Add("88-25", "ABN_BUTTERFLAME");
        Add("8C", "ABN_ADAMANT_SEED");
        Add("90", "ABN_CLOTHFLY");
        Add("94", "ABN_DASH_JUICE");
        Add("A0", "ABN_DEMON_POWDER");
        Add("A4", "ABN_HARDSHELL_POWDER");
        Add("A8", "ABN_ARC_SHOT_BRACE");
        Add("AC", "ABN_REDLAMPSQUID");
        Add("B4", "ABN_YELLOWLAMPSQUID");
        Add("C0", "ABN_ARC_SHOT_AFFINITY");
        Add("D4", "ABN_STINKMINK");
        Add("DC", "ABN_IMMUNITY");
        Add("E0", "ABN_GOURMET_FISH");
        Add("E4", "ABN_NATURAL_HEALING");
        Add("E8", "ABN_GO_FIGHT_WIN");
        Add("EC", "ABN_DEMON_AMMO");
        Add("F4", "ABN_ARMOR_AMMO");
        Add("F8", "F4");
        Add("FC", "ABN_POWER_DRUM");
        Add("124", "ABN_ROUSING_ROAR");
        Add("65C-1", "ABN_POISON");
        Add("65C-2", "ABN_VENOM");
        Add("674", "ABN_DEF_DOWN");
        Add("678", "ABN_RES_DOWN");
        Add("67C", "ABN_STENCH");
        Add("680", "ABN_HELLFIRE");
        Add("684", "ABN_BLAST");
        Add("68C", "ABN_FIRE");
        Add("690", "ABN_WATER");
        Add("694", "ABN_ICE");
        Add("698", "ABN_THUNDER");
        Add("69C", "ABN_DRAGON");
        Add("6A8", "ABN_BUBBLES_PLUS");
        Add("F0", "ABN_OFFENSIVE_GUARD");
        Add("10C", "ABN_AFFINITY_SLIDING");
        Add("118", "ABN_COUNTERSTRIKE");
        Add("1D8", "ABN_PROTECTIVE_POLISH");
        Add("1DC", "ABN_LATENTPOWER");
        Add("88-15", "ABN_DANGO_BULKER");
        Add("130", "ABN_DANGO_BOOSTER");
        Add("134", "ABN_DANGO_GLUTTON");

    }
}