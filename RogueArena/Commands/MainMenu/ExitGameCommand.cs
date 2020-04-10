namespace RogueArena.Commands.MainMenu
{
    using RogueArena.Data;
    using SadConsole;

    public class ExitGameCommand : Command
    {
        protected override void Handle(ProgramData data)
        {
            Game.Instance.Exit();
        }
    }
}
