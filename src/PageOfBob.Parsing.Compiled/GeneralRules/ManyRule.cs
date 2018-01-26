using Sigil;
using System.Collections.Generic;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class ManyRule<T, K> : IRule<List<T>>
    {
        private readonly IRule<T> rule;
        private readonly IRule<K> separator;
        private readonly bool keepResults;

        public ManyRule(IRule<T> rule, bool keepResults, IRule<K> separator = null, string name = null)
        {
            this.rule = rule;
            this.keepResults = keepResults;
            this.separator = separator;
            Name = name ?? $"{rule.Name}.Many()";
        }

        public string Name { get; }

        void EmitTryRule<TDelegate>(CompilerContext<TDelegate> context, Label end, Local pos, Local list, bool checkSeparator)
        {
            var emit = context.Emit;

            // pos
            if (checkSeparator && separator != null)
            {
                var sepSuccess = emit.DefineLabel();
                bool sepCanFail = separator.Emit(context, sepSuccess);
                if (sepCanFail)
                {
                    // Fail, bail
                    emit.Branch(end); // pos
                }

                // Success, throw away the result
                emit.MarkLabel(sepSuccess); // x, pos
                emit.StoreLocal(pos); // x
                emit.Pop(); // ...
                emit.LoadLocal(pos); // pos
            }


            var localSuccess = emit.DefineLabel();
            bool canFail = rule.Emit(context, localSuccess); 
            if (canFail)
            {
                // This rule failed.
                emit.Branch(end); // pos
            }

            emit.MarkLabel(localSuccess);
            // This rule succeeded.
            // c, pos
            if (keepResults)
            {
                emit.StoreLocal(pos); // c
                using (var c = emit.DeclareLocal<T>())
                {
                    emit.StoreLocal(c); // ...
                    emit.LoadLocal(list); // list
                    emit.LoadLocal(c); // list, c
                    emit.CallVirtual(typeof(List<T>).GetMethod("Add", new[] { typeof(T) })); // ...
                    emit.LoadLocal(pos); // pos
                }
            }
            else
            {
                emit.StoreLocal(pos); // c
                emit.Pop();
                emit.LoadLocal(pos); // pos
            }
        }


        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;

            Local list = keepResults ? emit.DeclareLocal<List<T>>() : null;
            using (var pos = emit.DeclareLocal<int>())
            {
                if (keepResults)
                {
                    emit.NewObject<List<T>>();
                    emit.StoreLocal(list);
                }

                // pos
                var start = emit.DefineLabel();
                var end = emit.DefineLabel();

                if (separator != null)
                {
                    // Execute the rule up-front, without worrying about separator
                    EmitTryRule(context, end, pos, list, false);
                }
                
                // Start of the loop
                emit.MarkLabel(start); // pos

                // Try the rule
                EmitTryRule(context, end, pos, list, true);

                // Back to the top of the loop
                emit.Branch(start);

                // End of the loop
                emit.MarkLabel(end); // pos
                emit.StoreLocal(pos); // ...
                if (keepResults)
                {
                    emit.LoadLocal(list); // list
                }
                else
                {
                    emit.LoadNull(); // null
                }
                emit.LoadLocal(pos); // v, pos
            }

            if (list != null)
                list.Dispose();

            emit.Branch(success);
            return false;
        }
    }
}
