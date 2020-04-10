namespace RogueArena.Commands
{
    using RogueArena.Data;

    public abstract class Command
    {
        public static void Run(Command command, ProgramData data)
        {
            command?.Handle(data);
        }

        protected abstract void Handle(ProgramData data);
    }
}
