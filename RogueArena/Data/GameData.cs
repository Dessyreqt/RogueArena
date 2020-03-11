namespace RogueArena.Data
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using RogueArena.Components;
    using RogueArena.Events;
    using RogueArena.Map;

    public class GameData
    {
        private readonly List<Entity> _entities;
        private int _playerIndex;
        private GameMap _gameMap;
        private GameState _gameState;
        private MessageLog _messageLog;

        private GameData()
        {
            _entities = new List<Entity>();
            _playerIndex = -1;
            _gameMap = null;
            _gameState = GameState.PlayersTurn;
            _messageLog = null;
        }

        public List<Entity> Entities => _entities;

        public Entity Player => _playerIndex != -1 ? _entities[_playerIndex] : null;

        public GameMap GameMap => _gameMap;

        public MessageLog MessageLog => _messageLog;

        public GameState GameState
        {
            get => _gameState;
            set => _gameState = value;
        }

        public static GameData New()
        {
            var gameData = new GameData();

            gameData._messageLog = new MessageLog(Constants.MessageX, Constants.MessageWidth, Constants.MessageHeight);

            var player = new Entity(0, 0, '@', Color.White, "Player", true, RenderOrder.Actor, new Fighter(30, 2, 5), inventory: new Inventory(26));
            
            gameData._entities.Clear();
            gameData._entities.Add(player);

            gameData._playerIndex = gameData._entities.IndexOf(player);

            gameData._gameMap = new GameMap(Constants.MapWidth, Constants.MapHeight);
            gameData._gameMap.MakeMap(
                Constants.MaxRooms,
                Constants.MinRoomSize,
                Constants.MaxRoomSize,
                Constants.MapWidth,
                Constants.MapHeight,
                player,
                gameData._entities,
                Constants.MaxMonstersPerRoom,
                Constants.MaxItemsPerRoom);
            
            gameData._gameState = GameState.PlayersTurn;

            return gameData;
        }
    }
}
