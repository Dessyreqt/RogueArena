namespace RogueArena
{
    using System;
    using System.Collections.Generic;
    using RogueArena.Components;

    public static class Ecs
    {
        private static readonly Lazy<Dictionary<Type, List<Entity>>> lazy = new Lazy<Dictionary<Type, List<Entity>>>(() => new Dictionary<Type, List<Entity>>());
        
        public static Dictionary<Type, List<Entity>> EntitiesWithComponent => lazy.Value;

        public static void Add<T>(this Entity entity, T component) where T : Component
        {
            var componentType = component.GetType().GetBaseComponentType();;

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
            EntitiesWithComponent[componentType].Remove(entity);
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
    }
}
