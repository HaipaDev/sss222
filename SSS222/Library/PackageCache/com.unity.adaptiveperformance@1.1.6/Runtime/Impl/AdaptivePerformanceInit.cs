
namespace UnityEngine.AdaptivePerformance
{
    internal class AdaptivePerformanceManagerSpawner : ScriptableObject
    {
        public GameObject m_ManagerGameObject;

        void OnEnable()
        {
            if (m_ManagerGameObject == null)
            {
                m_ManagerGameObject = new GameObject("AdaptivePerformanceManager");
                var ap = m_ManagerGameObject.AddComponent<AdaptivePerformanceManager>();
                Holder.Instance = ap;
                DontDestroyOnLoad(m_ManagerGameObject);
            }
        }
    }

    internal static class AdaptivePerformanceInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Initialize()
        {
            ScriptableObject.CreateInstance<AdaptivePerformanceManagerSpawner>();
        }
    }
}
