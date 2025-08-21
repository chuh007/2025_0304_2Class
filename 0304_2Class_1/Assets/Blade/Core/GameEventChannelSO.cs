using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blade.Core
{
    // 모든 이벤트 클래스의 부모 클래스
    public class GameEvent
    {
        
    }
    
    [CreateAssetMenu(fileName = "EventChannel", menuName = "SO/EventChannel", order = 0)]
    public class GameEventChannelSO : ScriptableObject
    {
        // 이벤트 타입(Type)을 키로, 해당 타입에 연결된 델리게이트(여러개 들어갈 수 있음)를 값으로 저장하는 딕셔너리
        private Dictionary<Type, Action<GameEvent>> _events = new Dictionary<Type, Action<GameEvent>>();
        // 구독 해지에 필요한 테이블 같은 애. value로 들어가는 애는 우리가 넘겨준게 아니라 래핑된 애라. 이거로 기억해두는 것.
        // 전달받은 원래 델리게이트(Action<T>)를 키로,
        // 내부 처리용으로 래핑된 Action<GameEvent> 델리게이트(람다)를 값으로 저장
        private Dictionary<Delegate, Action<GameEvent>> _lookup = new Dictionary<Delegate, Action<GameEvent>>();

        // T타입으로 받으며 T는 GameEvent를 상속받아야 함.
        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookup.ContainsKey(handler) == false) // 이 함수가 구독되어있지 않은가?
            {
                // GameEvent를 받아 T로 캐스팅한 후, 원래 handler를 실행하는 래핑 델리게이트 생성함.
                Action<GameEvent> castHandler = (evt) => handler(evt as T);
                // 해지 시 필요하므로 원본 handler와 래핑된 castHandler 쌍을 저장
                _lookup[handler] = castHandler;
                
                // T타입으로 받은거의 타입 가져오기 ex) SaveGameEvent
                Type evtType = typeof(T);
                // 타입별 델리게이트에 추가하기
                if (_events.ContainsKey(evtType))
                {
                    _events[evtType] += castHandler; // 기존 델리게이트에 추가
                }
                else
                {
                    _events[evtType] = castHandler; // 새로 등록
                }
            }
        }
        // T타입으로 받으며 T는 GameEvent를 상속받아야 함.
        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            Type evtType = typeof(T); // Add와 같음
            // _lookup에서 래핑된 델리게이트를 찾아
            if (_lookup.TryGetValue(handler, out Action<GameEvent> action))
            {
                // _events 딕셔너리에서 해당 이벤트 타입의 체인에서 제거
                if (_events.TryGetValue(evtType, out Action<GameEvent> internalAction))
                {
                    internalAction -= action; 
                    if (internalAction == null) // 구독자가 없어 델리게이트가 null이면
                    {
                        _events.Remove(evtType); // 이벤트에서 해당 Type를 지워준다.
                    }
                    else
                    {
                        _events[evtType] = internalAction; // 아니면 지우고 남은 델리게이트로 바꿔준다.
                    }
                }
                _lookup.Remove(handler); // 룩업에서도 지운다.
            }
        }

        public void RaiseEvent(GameEvent evt)
        {
            // events에서 evt의 타입이 key인 델리게이트를 찾아 해당 델리게이트 호출
            if (_events.TryGetValue(evt.GetType(), out Action<GameEvent> handlers))
            {
                handlers?.Invoke(evt); // ?는 때도 된다.(null일때 위에서 빼줬기 때문)
            }
        }

        public void Clear()
        {
            // 이벤트와 룩업 전부를 지운다.(초기화)
            _events.Clear();
            _lookup.Clear();
        }
    }
}