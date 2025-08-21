using System;
using System.Linq.Expressions;
using UnityEngine;

namespace Blade.Test
{
    public class TestPlayer
    {
        public int hp;
        public string playerName;
        public float speed;
    }
    
    public class ExpressionTest : MonoBehaviour
    {
        public Func<T, TMember> CreateMember<T, TMember>(string memberName)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(param, memberName);
            var lambda = Expression.Lambda<Func<T, TMember>>(member, param);
            return lambda.Compile();
        }
        
        public Func<float, float, float> GetFunc()
        {
            ParameterExpression parama = Expression.Parameter(typeof(float), "x");
            ParameterExpression paramb = Expression.Parameter(typeof(float), "y");
            
            BinaryExpression addExp = Expression.Add(parama, paramb);
            
            Expression<Func<float,float, float>> lambda
                = Expression.Lambda<Func<float, float, float>>(addExp, parama, paramb);
            return lambda.Compile();
        }

        [ContextMenu("Test")]
        private void Test()
        {
            TestPlayer p = new TestPlayer{hp = 100, playerName = "Player1", speed = 5.0f};
            Func<TestPlayer, int> playerHpGetter = CreateMember<TestPlayer, int>("hp");
            Func<TestPlayer, string> playerNameGetter = CreateMember<TestPlayer, string>("playerName");
            Func<TestPlayer, float> playerSpeedGetter = CreateMember<TestPlayer, float>("speed");
        }
    }
}