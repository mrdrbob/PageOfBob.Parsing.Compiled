using System;
using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractMatchFunctionRule<T> : AbstractMatchRule<T>
    {
        private readonly Func<char, bool> match;

        protected AbstractMatchFunctionRule(Func<char, bool> match, string name = null)
        {
            this.match = match;
            Name = name ?? "Match";
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            context.MatchFunction(match, success);
        }
    }
}
