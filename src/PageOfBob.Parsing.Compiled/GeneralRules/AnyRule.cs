using Sigil;
using System.Linq;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class AnyRule<T> : IRule<T>
    {
        private readonly IRule<T>[] rules;

        public AnyRule(IRule<T>[] rules)
        {
            this.rules = rules;
            Name = $"( {string.Join("|", rules.Select(x => x.Name))} )";
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            foreach (var rule in rules)
            {
                bool canFail = rule.Emit(context, success);
                if (!canFail)
                    return false;
            }

            return true;
        }
    }
}
