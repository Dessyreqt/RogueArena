namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using RogueArena.Components;
    using RogueArena.Data;

    public static class Ecs
    {
        private static readonly Lazy<Dictionary<Type, List<Entity>>> lazy = new Lazy<Dictionary<Type, List<Entity>>>(() => new Dictionary<Type, List<Entity>>());

        public static Dictionary<Type, List<Entity>> EntitiesWithComponent => lazy.Value;

        public static void Add<T>(this Entity entity, T component) where T : Component
        {
            var componentType = component.GetType().GetBaseComponentType();

            if (entity.Get(componentType) is Component)
            {
                entity.Components.Remove(componentType);
            }

            component.Owner = entity;
            entity.Components[componentType] = component;

            if (!EntitiesWithComponent.ContainsKey(componentType))
            {
                EntitiesWithComponent[componentType] = new List<Entity>();
            }

            EntitiesWithComponent[componentType].Add(entity);
        }

        public static void Remove<T>(this Entity entity) where T : Component
        {
            var existingComponent = entity.Get<T>();

            if (existingComponent == null)
            {
                return;
            }

            var componentType = typeof(T).GetBaseComponentType();

            if (EntitiesWithComponent.ContainsKey(componentType))
            {
                EntitiesWithComponent[componentType].Remove(entity);
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
            EntitiesWithComponent.Clear();

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
                if (!EntitiesWithComponent.ContainsKey(componentType))
                {
                    EntitiesWithComponent[componentType] = new List<Entity>();
                }

                EntitiesWithComponent[componentType].Remove(entity);
            }
        }

        private static void AddEntityToDictionaries(Entity entity)
        {
            foreach (var componentType in entity.Components.Keys)
            {
                if (!EntitiesWithComponent.ContainsKey(componentType))
                {
                    EntitiesWithComponent[componentType] = new List<Entity>();
                }

                EntitiesWithComponent[componentType].Add(entity);
            }
        }
    }
}
