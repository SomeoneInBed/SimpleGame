using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {
        //new lists
        public static readonly List<Item> Items = [];
        public static readonly List<Monster> Monsters = [];
        public static readonly List<Quest> Quests = [];
        public static readonly List<Location> Locations = [];

        //items
        public const int ITEM_ID_IRON_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL= 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;

        //monsters
        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_SPIDER = 3;
        public const int MONSTER_ID_GIANT_SPIDER = 4;

        //quests
        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
        public const int QUEST_ID_SURVIVE = 3;

        //locations
        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;

        //Methods that will be called everytime the entire class is called upon
        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
        }

        public static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_IRON_SWORD, "Iron Sword", "Iron Swords", null, 2, 5, null));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat Tail", "Rat Tails"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of Fur", "Pieces of Fur"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake Fang", "Snake Fangs"));
            Items.Add(new HealingSpell(ITEM_ID_HEALING_POTION, "Healing Potion", "Healing Potions", 10));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider Silk", "Spider Silks"));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer Pass", "Adventurer Passes"));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins"));
            Items.Add(new Item(ITEM_ID_CLUB, "Club", "Clubs"));
        }

        public static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 5, 5, 5, 5);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL)!, 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR)!, 60, false));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 20, 20, 15, 10, 10);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG)!, 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN)!, 60, false));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant Spider", 75, 75, 25, 100, 50);
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG)!, 75, false));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK)!, 60, false));

            Monster spider = new Monster(MONSTER_ID_SPIDER, "Spider", 25, 30, 20, 25, 25);
            spider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK)!, 50, false));
            spider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG)!, 75, false));

            Monsters.Add(spider);         
            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);

        }

        public static void PopulateQuests()
        {
            Quest clearAlchemistGarden = new Quest(QUEST_ID_CLEAR_ALCHEMIST_GARDEN, "Clear the Alchemist's Garden", "Kill rats and find 3 rat tails. You will recieve a healing potion and 30 gold.", 30, 20);
            clearAlchemistGarden.Completion.Add(new QuestComplete(ItemByID(ITEM_ID_RAT_TAIL), 3));
            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField = new Quest(QUEST_ID_CLEAR_FARMERS_FIELD, "Clear the Farmer's Field", 
                "Clear the field by killing snakes. Collect 10 snake skins and five snake fangs, and you will recieve 75 gold and an Adventurer's Pass.", 50, 45);
            clearFarmersField.Completion.Add(new QuestComplete(ItemByID(ITEM_ID_SNAKE_FANG)!, 5));
            clearFarmersField.Completion.Add(new QuestComplete(ItemByID(ITEM_ID_SNAKESKIN)!, 10));
            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            Quest survive = new Quest(QUEST_ID_SURVIVE, "Sudden Quest: Survive.", "You stumbled upon the spider's field. They are now coming.", 100, 100);
            survive.Completion.Add(new QuestComplete(ItemByID(ITEM_ID_SPIDER_FANG)!, 25));
            survive.Completion.Add(new QuestComplete(ItemByID(ITEM_ID_SPIDER_SILK)!, 50));

            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
            Quests.Add(survive);
        }

        public static void PopulateLocations()
        {
            Location home = new Location(LOCATION_ID_HOME, "Home", "There is trash all over the place");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town Square", "There are rats all over the place. Sadly, they own a restaurant down the street meaning you can't kill them.");

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard Post", "The guards are standing around doing nothing. They are not very good at their job.");

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "The water is beautiful and the fish are wet. What will you do?");

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's Hut", "The alchemist is brewing potions and is not paying attention to you.");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "The Alchemist's Garden", "You can hear to squeaking with every step you take. No wonder the alchemist is so afraid.");
            alchemistGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "The farmer has a shotgun right next to him. Why is everyone so tense here");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmField = new Location(LOCATION_ID_FARM_FIELD, "Farmer's Field", "The hissing is getting increasingly louder.");
            farmField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Spider Field", "Many creepy crawlies all around. You hear a loud tremor.");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_SPIDER);
            spiderField.QuestAvailableHere = QuestByID(QUEST_ID_SURVIVE);

            home.LocationToNorth = townSquare;

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmField;

            farmField.LocationToEast = farmhouse;

            alchemistHut.LocationToSouth = townSquare;
            alchemistHut.LocationToNorth = alchemistGarden;

            alchemistGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToWest = townSquare;
            guardPost.LocationToEast = bridge;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;

            spiderField.LocationToWest = bridge;

            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(bridge);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmField);
            Locations.Add(spiderField);
        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in Items)
            {
                if(item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }
        
        public static Monster MonsterByID(int id)
        {
            foreach(Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }
            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach(Quest quest in Quests)
            {
                if(quest.ID == id)
                {
                    return quest;
                }
            }
            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach(Location location in Locations)
            {
                if(location.ID == id)
                {
                    return location;
                }
            }
            return null;
        }
    }
}