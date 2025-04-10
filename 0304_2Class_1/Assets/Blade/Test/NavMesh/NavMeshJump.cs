using System.Collections;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace Blade.Test.NavMesh
{
    public class NavMeshJump : MonoBehaviour
    {
        [SerializeField] private int targetArea = 2;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private float gravity = -9.81f;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitUntil(IsOnJump);
                yield return StartCoroutine(StartJump());
            }
        }
        
        private IEnumerator StartJump()
        {
            agent.isStopped = true;
            
            OffMeshLinkData linkData = agent.currentOffMeshLinkData;
            Vector3 start = transform.position;
            Vector3 end = linkData.endPos;

            SplineContainer spline = (linkData.owner as NavMeshAgent).GetComponent<SplineContainer>();
            
            float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / jumpSpeed);
            float current = 0;
            float percent = 0;

            Vector3 first = spline.Spline.Knots.First().Position;
            Vector3 last = spline.Spline.Knots.Last().Position;

            first += spline.transform.position;
            last += spline.transform.position;
            
            bool isReversed = Vector3.Distance(first, transform.position) > Vector3.Distance(last, transform.position);
            
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / jumpTime;

                Vector3 position = spline.EvaluatePosition(isReversed ? 1 - percent : percent);
                transform.position = position;
                yield return null;
            }
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
        //     float current = 0;
        //     float percent = 0;
        //     
        //     float v0 = (end - start).y - gravity;
        //     while (percent < 1)
        //     {
        //         current += Time.deltaTime;
        //         percent = current / jumpTime;
        //         
        //         Vector3 pos = Vector3.Lerp(start, end, percent);
        //
        //         pos.y = start.y + (v0 * percent) + (gravity * percent * percent);
        //         transform.position = pos;
        //         yield return null;
        //     }
        //     
        //     agent.CompleteOffMeshLink();
        //     agent.isStopped = false;
        // }

        private bool IsOnJump()
        {
            if (agent.isOnOffMeshLink)
            {
                OffMeshLinkData linkData = agent.currentOffMeshLinkData;
                var link = linkData.owner as NavMeshLink;
                if(link != null && link.area == targetArea)
                    return true;
            }
            return false;
        }
    }
}