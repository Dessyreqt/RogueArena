namespace RogueArena.Tests.Data.Entities
{
    using System.Collections.Generic;
    using RogueArena.Data;
    using RogueArena.Data.Components;
    using Shouldly;

    public class EntitiesTests
    {
        public EntitiesTests()
        {
            Entities.SetEntities(null, new List<Entity>());
        }

        public void ShouldReturnEmptyListIfNoEntitiesWithComponent()
        {
            var list = Entities.With<FighterComponent>();

            list.ShouldSatisfyAllConditions(() => list.ShouldNotBeNull(), () => list.Count.ShouldBe(0));
        }

        public void ShouldReturnEntityWithMatchingComponent()
        {
            var entity = new Entity();
            entity.Add(new FighterComponent());

            var list = Entities.With<FighterComponent>();
            list.ShouldSatisfyAllConditions(() => list.ShouldNotBeNull(), () => list.Count.ShouldBe(1), () => list.ShouldContain(entity));
        }

        public void ShouldNotReturnEntityWithNonmatchingComponent()
        {
            var entity = new Entity();
            entity.Add(new ItemComponent());

            var list = Entities.With<FighterComponent>();
            list.ShouldSatisfyAllConditions(() => list.ShouldNotBeNull(), () => list.Count.ShouldBe(0));
        }
    }
}
