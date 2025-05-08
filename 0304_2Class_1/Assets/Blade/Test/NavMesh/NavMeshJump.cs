using System.Collections;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

namespace Blade.Test.NavMesh
{
    public class NavMeshJump : MonoBehaviour
    {
        [SerializeField] private int targetArea = 2;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float jumpSpeed = 10f;
        [SerializeField] private float gravity = -9.81f;

        private IEnumerator Start()  //이렇게 해주면 Start함수가 코루틴으로 동작
        {
            while (true)
            {
                yield return new WaitUntil(IsOnJump);
                yield return StartCoroutine(StartJump());
            }
        }

        //스플라인용 점프 코드
        private IEnumerator StartJump()
        {
            agent.isStopped = true;
            
            OffMeshLinkData linkData = agent.currentOffMeshLinkData;
            Vector3 start = transform.position;
            Vector3 end = linkData.endPos;
            
            SplineContainer spline = (linkData.owner as NavMeshLink).GetComponent<SplineContainer>();
            
            float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / jumpSpeed);
            float currentTime = 0;
            float percent = 0;

            Vector3 first = spline.Spline.Knots.First().Position;
            Vector3 last = spline.Spline.Knots.Last().Position;
            first += spline.transform.position;
            last += spline.transform.position;
            
            bool isReversed = Vector3.Distance(first, transform.position) > Vector3.Distance(last, transform.position);

            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / jumpTime;

                Vector3 position = spline.EvaluatePosition(isReversed ? 1 - percent : percent);
                transform.position = position;
                yield return null;
            }
            agent.CompleteOffMeshLink();
            
            agent.isStopped = false;
        }
        
        // private IEnumerator StartJump()
        // {
        //     agent.isStopped = true;
        //
        //     OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        //     Vector3 start = transform.position;
        //     Vector3 end = linkData.endPos;
        //
        //     float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / jumpSpeed);
        //     float currentTime = 0;
        //     float percent = 0;
        //
        //     float v0 = (end - start).y - gravity; // y방향 초기 속도 
        //
        //     while(percent < 1)
        //     {
        //         currentTime += Time.deltaTime;
        //         percent = currentTime / jumpTime;
        //
        //         Vector3 pos = Vector3.Lerp(start, end, percent);
        //         //포물선 운동 : 시작위치 + 초기속도 * 시간 + 중력 * 시간제곱
        //         pos.y = start.y + (v0 * percent) + (gravity * percent * percent);
        //         transform.position = pos;
        //
        //         yield return null;
        //     }
        //
        //     agent.CompleteOffMeshLink();
        //
        //     agent.isStopped = false;
        //
        // }

        private bool IsOnJump()
        {
            if (agent.isOnOffMeshLink)
            {
                OffMeshLinkData linkData = agent.currentOffMeshLinkData;
                var link = linkData.owner as NavMeshLink;
                if (link != null && link.area == targetArea)
                    return true;
            }
            return false;
        }
    }
}