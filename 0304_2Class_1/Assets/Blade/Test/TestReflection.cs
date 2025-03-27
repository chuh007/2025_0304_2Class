using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Blade.Test
{
    public class TestObj
    {
        public int testValue;
    }
    
    [Serializable]
    public class MyClass
    {
        public int age;
        public string name;
        public TestObj testObj;
        
        private int number;

        public MyClass(int age, string name, int number)
        {
            this.age = age;
            this.name = name;
            this.number = number;
            testObj = new TestObj();
        }
        
        public void Introduce()
        {
            Debug.Log($"{age}살, {name}입니다.");
            number++;
        }
    }
    public class TestReflection : MonoBehaviour
    {
        [SerializeField] private MyClass myClass;

        public string targetField;

        void Start()
        {
            AssemblyBuilder newAssembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("GGMAssembly"), AssemblyBuilderAccess.Run);

            ModuleBuilder newModule = newAssembly.DefineDynamicModule("GGM");

            TypeBuilder newType = newModule.DefineType("SumTo100");
    
            //이름, 접근제어자, 반환형식, 매개변수
            MethodBuilder newMethod = newType.DefineMethod("Print", MethodAttributes.Public, typeof(int), Type.EmptyTypes);

            ILGenerator generator = newMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldc_I4, 1);  //스택에 32비트 정수 1넣고
            //LDC_I4 => Load Constant _integer 4
            for (int i = 2; i <= 100; ++i)
            {
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Add); //스택의 마지막 두 값을 꺼내 더한뒤에 결과물을 다시 push
            }
            generator.Emit(OpCodes.Ret);


            Type t = newType.CreateType();
            //만들기 끝났으면 이 타입으로 생성하고 사용 가능    
            object sumTo100 = Activator.CreateInstance(t);
            MethodInfo print = sumTo100.GetType().GetMethod("Print");
            int result = (int)print.Invoke(sumTo100, null);
    
            Debug.Log(result);
        }


        
        
        [ContextMenu("Test Reflection")]
        private void TestField()
        {
            Type t = myClass.GetType();

            //FieldInfo nameField = t.GetField(targetField, BindingFlags.NonPublic | BindingFlags.Public);
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            
            MethodInfo method = t.GetMethod("Introduce");
            
            method?.Invoke(myClass, null);
            //Debug.Log( nameField.GetValue(myClass));
        }

        [ContextMenu("Create test")]
        private void CreateTest()
        {
            Type t = typeof(MyClass);
            myClass = Activator.CreateInstance(t, 17, "장준서", 1) as MyClass;
        }

        public int testAge;

        [ContextMenu("Set age")]
        private void SetAge()
        {
            FieldInfo ageField = myClass.GetType().GetField("age");
            ageField.SetValue(myClass, testAge);

        }
    }
}