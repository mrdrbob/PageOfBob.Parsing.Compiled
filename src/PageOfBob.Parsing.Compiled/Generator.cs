using Sigil;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PageOfBob.Parsing.Compiled
{
    public delegate bool TryParse<T>(string input, out T result, out int position, int startPosition = 0);

    public interface IParser<T>
    {
        bool TryParse(string input, out T result, out int position, int startPosition = 0);
    }

    public class CompilerContext<T>
    {
        Dictionary<string, object> fields = new Dictionary<string, object>();

        public System.Reflection.Emit.TypeBuilder TypeBuilder { get; }
        public Emit<TryParse<T>> Emit { get; }
        public Local StringLocal { get; set; }
        public Local LengthLocal { get; set; }

        public CompilerContext(string parserName)
        {
            TypeBuilder = ParserCompiler.Builder.DefineType(parserName, TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class, typeof(object), new[] { typeof(IParser<T>) });
            Emit = Emit<TryParse<T>>.BuildInstanceMethod(TypeBuilder, "TryParse", MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.HideBySig);
        }

        public System.Reflection.Emit.FieldBuilder SaveInField<TField>(TField item)
        {
            var name = $"field{fields.Count}";
            var builder = TypeBuilder.DefineField(name, typeof(TField), FieldAttributes.Public | FieldAttributes.Static);
            fields.Add(name, item);
            return builder;
        }

        public IParser<T> Finalize()
        {
            var finalType = TypeBuilder.CreateTypeInfo().AsType();
            var instance = Activator.CreateInstance(finalType);
            foreach (var kvp in fields)
            {
                instance.GetType().GetField(kvp.Key).SetValue(instance, kvp.Value);
            }

            return (IParser<T>)instance;
        }
    }

    public static class ParserCompiler
    {
        public static System.Reflection.Emit.ModuleBuilder Builder { get; }

        static ParserCompiler()
        {
            var assemblyBuilder = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName("PageOfBob.Parsing.Gen"), System.Reflection.Emit.AssemblyBuilderAccess.Run);
            Builder = assemblyBuilder.DefineDynamicModule("PageOfBob.Parsing.Mod");
        }

        public static IParser<T> CompileParser<T>(this IRule<T> rule, string name)
        {
            var type = typeof(T);

            var context = new CompilerContext<T>(name);

            var emit = context.Emit;
            var success = emit.DefineLabel();
            var end = emit.DefineLabel();

            using (var str = emit.DeclareLocal<string>())
            using (var len = emit.DeclareLocal<int>())
            {
                emit.LoadArgument(1); // str
                emit.Duplicate(); // str, str
                emit.CallVirtual(typeof(string).GetProperty("Length").GetMethod); // str, len
                emit.StoreLocal(len); // str 
                emit.StoreLocal(str); // ...

                context.StringLocal = str;
                context.LengthLocal = len;

                // Start at zero index
                emit.LoadArgument(4); // pos

                // Emit the rule
                bool canFail = rule.Emit(context, success); // pos

                if (canFail)
                {
                    // Put pos in the appropriate place
                    using (var pos = emit.DeclareLocal<int>())
                    {
                        emit.StoreLocal(pos); // ...
                        emit.LoadArgument(3); // pos_addr
                        emit.LoadLocal(pos); // pos_addr, pos
                        emit.StoreObject<int>(); // ...
                    }

                    // FAILED BRANCH: ...
                    emit.LoadArgument(2); // addr
                    emit.InitializeObject<T>(); // def

                    emit.LoadConstant(false); // false
                    emit.Branch(end); // end.
                }

                // SUCCESS BRANCH:
                emit.MarkLabel(success); // v, pos

                // Put pos in the appropriate place
                using (var pos = emit.DeclareLocal<int>())
                {
                    emit.StoreLocal(pos); // v
                    emit.LoadArgument(3); // v, pos_addr
                    emit.LoadLocal(pos); // v, pos_addr, pos
                    emit.StoreObject<int>(); // v
                }

                using (var val = emit.DeclareLocal<T>())
                {
                    emit.StoreLocal(val); // ...
                    emit.LoadArgument(2); // addr
                    emit.LoadLocal(val); // addr, v
                }

                if (type.GetTypeInfo().IsValueType)
                {
                    emit.StoreObject(typeof(T)); // ...
                }
                else
                {
                    emit.StoreIndirect<T>(); // ...
                }
                emit.LoadConstant(true); // true

                emit.MarkLabel(end);
                emit.Return();

                context.Emit.CreateMethod();
            }

            return context.Finalize();
        }
    }
}
