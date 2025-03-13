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
            Debug.Log($"{age}��, {name}�Դϴ�.");
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

            //�̸�, ����������, ��ȯ����, �Ű�����
            MethodBuilder newMethod = newType.DefineMethod("Print", MethodAttributes.Public, typeof(int), Type.EmptyTypes);

            ILGenerator generator = newMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldc_I4, 1);  //���ÿ� 32��Ʈ ���� 1�ְ�
                                                //LDC_I4 => Load Constant _integer 4
            for (int i = 2; i <= 100; ++i)
            {
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Add); //������ ������ �� ���� ���� ���ѵڿ� ������� �ٽ� push
            }
            generator.Emit(OpCodes.Ret);


            Type t = newType.CreateType();
            //����� �������� �� Ÿ������ �����ϰ� ��� ����    
            object sumTo100 = Activator.CreateInstance(t);
            MethodInfo print = sumTo100.GetType().GetMethod("Print");
            int result = (int)print.Invoke(sumTo100, null);

            Debug.Log(result);
        }

        //void Start()
        //{
        //    Type t = typeof(MyClass);

        //    MyClass mc1 = Activator.CreateInstance(t, 20, "�ּ���", 30) as MyClass;

        //    Stopwatch stopWatch = new Stopwatch(); //�ð� ����
        //    stopWatch.Start();
        //    int dummy = 40;
        //    for (int i = 0; i < 50000; ++i)
        //    {
        //        int temp = mc1.age;
        //        mc1.age = dummy;
        //        dummy = temp;
        //    }

        //    stopWatch.Stop();
        //    Debug.Log($"�Ϲ� �ð� : {stopWatch.ElapsedMilliseconds}ms");

        //    stopWatch.Start();
        //    dummy = 40;
        //    FieldInfo ageField = t.GetField("testObj"); //�̸����� �������⵵ ����
        //    TestObj testObj = ageField.GetValue(mc1) as TestObj;
        //    for (int i = 0; i < 50000; ++i)
        //    {
        //        int temp = testObj.testValue;
        //        testObj.testValue = temp;
        //        dummy = temp;
        //    }

        //    stopWatch.Stop();
        //    Debug.Log($"���÷��� �ð� : {stopWatch.ElapsedMilliseconds}ms");
        //}



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
            myClass = Activator.CreateInstance(t, 17, "���ؼ�", 1) as MyClass;
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
