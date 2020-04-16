namespace RogueArena.Data
{
    using System;
    using System.Collections.Generic;
    using RogueArena.Data.Components;

    public static class Entities
    {
        private static readonly Lazy<Dictionary<Type, List<Entity>>> lazy = new Lazy<Dictionary<Type, List<Entity>>>(() => new Dictionary<Type, List<Entity>>());

        public static Dictionary<Type, List<Entity>> WithComponent => lazy.Value;

        public static void Add<T>(this Entity entity, T component) where T : Component
        {
            var componentType = component.GetType().GetBaseComponentType();

            if (entity.Get(componentType) is Component)
            {
                entity.Components.Remove(componentType);
            }

            component.Owner = entity;
            entity.Components[componentType] = component;

            if (!WithComponent.ContainsKey(componentType))
            {
                WithComponent[componentType] = new List<Entity>();
            }

            WithComponent[componentType].Add(entity);
        }

        public static void Remove<T>(this Entity entity) where T : Component
        {
            var existingComponent = entity.Get<T>();

            if (existingComponent == null)
            {
                return;
            }

            var componentType = typeof(T).GetBaseComponentType();

            if (WithComponent.ContainsKey(componentType))
            {
                WithComponent[componentType].Remove(entity);
            }

            entity.Components.Remove(componentType);
        }

        public static Type GetBaseComponentType(this Type type)
        {
            var componentType = type;

            while (componentType.BaseType != typeof(Component))
            {
                if (componentType.BaseType == null)
                {
                    return null;
                }

                componentType = componentType.BaseType;
            }

            return componentType;
        }

        public static void SetEntities(this GameData data, List<Entity> entities)
        {
            data.Entities = entities;
            WithComponent.Clear();

            foreach (var entity in data.Entities)
            {
                AddEntityToDictionaries(entity);
            }
        }

        public static void AddEntity(this GameData data, Entity entity)
        {
            data.Entities.Add(entity);
            AddEntityToDictionaries(entity);
        }

        public static void RemoveEntity(this GameData data, Entity entity)
        {
            data.Entities.Remove(entity);
            RemoveEntityFromDictionaries(entity);
        }

        private static void RemoveEntityFromDictionaries(Entity entity)
        {
            foreach (var componentType in entity.Components.Keys)
            {
                if (!WithComponent.ContainsKey(componentType))
                {
                    WithComponent[componentType] = new List<Entity>();
                }

                WithComponent[componentType].Remove(entity);
            }
        }

        private static void AddEntityToDictionaries(Entity entity)
        {
            foreach (var componentType in entity.Components.Keys)
            {
                if (!WithComponent.ContainsKey(componentType))
                {
                    WithComponent[componentType] = new List<Entity>();
                }

                WithComponent[componentType].Add(entity);
            }
        }
    }
}
