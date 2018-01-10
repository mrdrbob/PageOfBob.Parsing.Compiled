using System;
using System.Collections.Generic;
using System.Text;
using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractTextFuncRule<T> : AbstractTextRule<T>
    {
        private readonly Func<char, bool> match;

        public AbstractTextFuncRule(Func<char, bool> match, string name = null)
        {
            this.match = match;
            Name = name ?? "Match";
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label matchSuccess)
        {
            context.MatchFunction(match, matchSuccess);
        }
    }
}
