using System;
using System.Reflection;
using UnityEngine;

public partial class GameState
{
    public void Register()
    {
        EntityRegistry = new EntityRegistry();
        LevelRegistry = new LevelRegistry();
        SkillRegistry = new SkillRegistry();
        CategoryRegistry = new CategoryRegistry();
        Anvil = new Anvil();

        Anvil.GetMods();

        Anvil.ModsPreInit();
        Anvil.ModsLoad();
        Anvil.ModsPostInit();

        RegisterStatsAndCategories();

        Entities.RegisterEntities();
        Skills.RegisterSkills();
        Levels.RegisterLevels();

        UIMain.GenerateFirstSkills();

        new Character(new Rogue(), new Halfling());
    }

    public void RegisterStatsAndCategories()
    {
        Category defensive = new Category(Category.CATEGORY_DEFENSIVE);
        Category magical = new Category(Category.CATEGORY_MAGICAL);
        Category physical = new Category(Category.CATEGORY_PHYSICAL);
        Category shadows = new Category(Category.CATEGORY_SHADOWS);
        Category stealth = new Category(Category.CATEGORY_STEALTH);

        //Needs negative modifiers
        defensive.AddStat(Character.STATS_ARMOUR);
        defensive.AddStat(Character.STATS_CONSTITUTION);
        defensive.AddStat(Character.STATS_HEALTHREGEN);
        defensive.AddStat(Character.STATS_MAXHEALTH);

        magical.AddStat(Character.STATS_MAGIC);
        magical.AddStat(Character.STATS_MOVEMENTSPEED);

        physical.AddStat(Character.STATS_STRENGTH);
        physical.AddStat(Character.STATS_DAMAGE);
        physical.AddStat(Character.STATS_CONSTITUTION);

        shadows.AddStat(Character.STATS_DEXTERITY);
        shadows.AddStat(Character.STATS_MAGIC);
        shadows.AddStat(Character.STATS_ATTACKSPEED);

        stealth.AddStat(Character.STATS_LOS);
        stealth.AddStat(Character.STATS_DEXTERITY);
        stealth.AddStat(Character.STATS_MOVEMENTSPEED);
        stealth.AddStat(Character.STATS_ATTACKSPEED);

        CategoryRegistry.RegisterCategory(Category.CATEGORY_DEFENSIVE, defensive);
        CategoryRegistry.RegisterCategory(Category.CATEGORY_MAGICAL, magical);
        CategoryRegistry.RegisterCategory(Category.CATEGORY_PHYSICAL, physical);
        CategoryRegistry.RegisterCategory(Category.CATEGORY_SHADOWS, shadows);
        CategoryRegistry.RegisterCategory(Category.CATEGORY_STEALTH, stealth);
    }
}