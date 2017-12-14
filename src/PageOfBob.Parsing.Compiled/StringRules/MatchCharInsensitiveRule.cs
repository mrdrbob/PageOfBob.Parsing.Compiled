using System;
using System.Collections.Generic;
using System.Text;
using Sigil;
using System.Linq;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class MatchCharInsensitiveRule : AbstractMatchRule
    {
        private readonly char[] charToMatch;

        public MatchCharInsensitiveRule(char[] charToMatch)
        {
            this.charToMatch = charToMatch;
            Name = "(i " + string.Join("|", charToMatch.Select(x => x.ToString())) + ")";
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label match)
        {
            // v2
            context.Emit.Duplicate(); // v2, v2
            context.Emit.Call(typeof(char).GetMethod("ToUpperInvariant", new[] { typeof(char) })); // v2, V2
            using (var upper = context.Emit.DeclareLocal<char>())
            {
                context.Emit.StoreLocal(upper); // v2

                foreach (var c in charToMatch)
                {
                    context.Emit.LoadLocal(upper); // v2, V2
                    context.Emit.LoadConstant(char.ToUpperInvariant(c)); // v2, v2, c
                    context.Emit.BranchIfEqual(match); // v2
                }
            }

            // Failed.
            context.Emit.Pop(); // ...
        }
    }
}
