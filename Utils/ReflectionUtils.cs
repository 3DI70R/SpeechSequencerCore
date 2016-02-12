using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public static class ReflectionUtils
    {
        public static T CreateConstructorDelegate<T>(Type objectType, params Type[] paramTypes)
        {
            return (T)(object)CreateConstructorDelegate(typeof(T), objectType, paramTypes);
        }
        public static Delegate CreateConstructorDelegate(this Type type, Type delegateType, params Type[] paramTypes)
        {
            ConstructorInfo ctor;
            DynamicMethod method;
            ILGenerator il;

            ctor = type.GetConstructor(paramTypes);
            method = new DynamicMethod(type.Name + "Creator", type, paramTypes);
            il = method.GetILGenerator();

            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.EmitLoadArgIL(i);
            }

            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            return method.CreateDelegate(delegateType);
        }

        public static void EmitLoadArgIL(this ILGenerator il, int arg)
        {
            switch(arg)
            {
                case 0: il.Emit(OpCodes.Ldarg_0); break;
                case 1: il.Emit(OpCodes.Ldarg_1); break;
                case 2: il.Emit(OpCodes.Ldarg_2); break;
                case 3: il.Emit(OpCodes.Ldarg_3); break;
                default: il.Emit(OpCodes.Ldarg, arg); break;
            }
        }
    }
}
