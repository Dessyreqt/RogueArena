namespace RogueArena.Data
{
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Xna.Framework;
    using Newtonsoft.Json;
    using RogueArena.Components;
    using RogueArena.Map;
    using RogueArena.Messages;

    public class GameData
    {
        private const string _saveLocation = "savegame.dat";

        private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            NullValueHandling = NullValueHandling.Ignore
        };

        public GameData()
        {
            Entities = new List<Entity>();
            DungeonLevel = null;
            GameState = GameState.PlayersTurn;
            MessageLog = null;
        }

        public List<Entity> Entities { get; set; }

        public Entity Player { get; set; }

        public DungeonLevel DungeonLevel { get; set; }

        public MessageLog MessageLog { get; set; }

        public GameState GameState { get; set; }

        public static GameData New(ProgramData data)
        {
            var player = new Entity { X = 0, Y = 0, Character = '@', Color = Color.White, Name = "Player", Blocks = true, RenderOrder = RenderOrder.Actor };
            player.Add(new InventoryComponent { Capacity = 26 });
            player.Add(new LevelComponent());
            player.Add(new EquipmentComponent());
            player.Add(new FighterComponent { BaseMaxHp = 100, Hp = 100, BaseDefense = 1, BasePower = 2 });

            var gameData = new GameData
            {
                MessageLog = new MessageLog { X = Constants.MessageX, Width = Constants.MessageWidth, Height = Constants.MessageHeight },
                DungeonLevel = new DungeonLevel { LevelNumber = 1, Map = new DungeonMap(1, Constants.MapWidth, Constants.MapHeight) },
                Player = player,
                GameState = GameState.PlayersTurn
            };

            var dagger = Items.Get(Items.Dagger, 0, 0);
            gameData.Player.Get<InventoryComponent>().AddItem(dagger, data);
            gameData.Player.Get<EquipmentComponent>().ToggleEquip(dagger, data);

            gameData.DungeonLevel.Map.MakeMap(
                Constants.MaxRooms,
                Constants.MinRoomSize,
                Constants.MaxRoomSize,
                Constants.MapWidth,
                Constants.MapHeight,
                player,
                gameData.DungeonLevel.Entities);

            gameData.SetEntities(gameData.DungeonLevel.Entities);

            return gameData;
        }

        public static GameData Load()
        {
            using (var file = new FileStream(_saveLocation, FileMode.Open))
            {
                var data = new byte[file.Length];
                file.Read(data, 0, (int)file.Length);
                var json = Compression.Unzip(data);
                return JsonConvert.DeserializeObject<GameData>(json, _serializerSettings);
            }
        }

        public void Save()
        {
            File.Delete(_saveLocation);

            using (var file = new FileStream(_saveLocation, FileMode.OpenOrCreate))
            {
                var json = JsonConvert.SerializeObject(this, _serializerSettings);
                var data = Compression.Zip(json);
                file.Write(data, 0, data.Length);
            }
        }
    }
}
