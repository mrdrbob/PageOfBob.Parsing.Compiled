using Sigil;
using System.Linq;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class MatchCharRule : AbstractMatchRule
    {
        private readonly char[] charToMatch;

        public MatchCharRule(char[] charToMatch)
        {
            this.charToMatch = charToMatch;
            Name = "(" + string.Join("|", charToMatch.Select(x => x.ToString())) + ")";
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label match)
        {
            // v2
            foreach (var c in charToMatch)
            {
                context.Emit.Duplicate(); // v2, v2
                context.Emit.LoadConstant(c); // v2, v2, c
                context.Emit.BranchIfEqual(match); // v2
            }

            // Failed.
            context.Emit.Pop(); // ...
        }
    }
}
