using Sigil;

namespace PageOfBob.Parsing.Compiled
{
    public interface IRule<TValue>
    {
        string Name { get; }

        // returns true if "fail" branch is possible.
        bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success);
    }
}
