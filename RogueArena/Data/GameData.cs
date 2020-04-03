namespace RogueArena.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Xna.Framework;
    using Newtonsoft.Json;
    using RogueArena.Components;
    using RogueArena.Events;
    using RogueArena.Map;

    public class GameData
    {
        private const string _saveLocation = "savegame.dat";

        private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        public GameData()
        {
            Entities = new List<Entity>();
            GameMap = null;
            GameState = GameState.PlayersTurn;
            MessageLog = null;
        }

        public List<Entity> Entities { get; set; }

        public Entity Player { get; set; }

        public GameMap GameMap { get; set; }

        public MessageLog MessageLog { get; set; }

        public GameState GameState { get; set; }

        public static GameData New()
        {
            var gameData = new GameData();

            gameData.MessageLog = new MessageLog(Constants.MessageX, Constants.MessageWidth, Constants.MessageHeight);

            var player = new Entity(0, 0, '@', Color.White, "Player", true, RenderOrder.Actor, new FighterComponent(30, 2, 5), inventoryComponent: new InventoryComponent(26));

            gameData.Entities.Clear();
            gameData.Entities.Add(player);

            gameData.Player = player;

            gameData.GameMap = new GameMap(Constants.MapWidth, Constants.MapHeight);
            gameData.GameMap.MakeMap(
                Constants.MaxRooms,
                Constants.MinRoomSize,
                Constants.MaxRoomSize,
                Constants.MapWidth,
                Constants.MapHeight,
                player,
                gameData.Entities,
                Constants.MaxMonstersPerRoom,
                Constants.MaxItemsPerRoom);

            gameData.GameState = GameState.PlayersTurn;

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
            using (var file = new FileStream(_saveLocation, FileMode.OpenOrCreate))
            {
                var json = JsonConvert.SerializeObject(this, _serializerSettings);
                var data = Compression.Zip(json);
                file.Write(data, 0, data.Length);
            }
        }
    }
}
