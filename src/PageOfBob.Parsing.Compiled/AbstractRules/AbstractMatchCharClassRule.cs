using System;
using System.Collections.Generic;
using System.Text;
using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractMatchCharClassRule<T> : AbstractMatchRule<T>
    {
        private readonly string charClass;

        public AbstractMatchCharClassRule(string charClass, string name = null)
        {
            this.charClass = charClass;
            Name = name ?? charClass;
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            context.MatchCharacterClass(charClass, success);
        }
    }
}
