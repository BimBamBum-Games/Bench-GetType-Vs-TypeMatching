using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class BenchGetTypeVsTypeMatching : MonoBehaviour {

    [SerializeField][Range(0, 2000)] int numberOfIteration = 100;
    [SerializeField] MobileUnit mobileUnitPb;

    private List<UnitEntityBase> unitEntities;
    private WaitForSecondsRealtime wait4read_secs = new(0.01f);
    private Stopwatch swatch = new Stopwatch();

    private void Start() {
        Debug.LogWarning("<color=yellow>The Bench Started!</color>");
        CreateList();
        Debug.LogWarning("<color=yellow>The process is in progress...</color>");
        StartCoroutine(Bench_GetType_TypeMatch_Sync());

    }

    public void DestroyAllPrefabs() {
        for (int i = 0; i < unitEntities.Count; i++) {
            if (Application.isPlaying) {
                Destroy(unitEntities[i].gameObject);
            }
            else if (Application.isEditor) {
                DestroyImmediate(unitEntities[i].gameObject);
            }
        }
        unitEntities.Clear();
    }

    public IEnumerator Bench_GetTypeSync() {

        swatch.Start();

        for (int i = 0; i < numberOfIteration; i++) {
            if (unitEntities[i].IsUnitEntity()) {
                //ToDo
            }
            yield return wait4read_secs;
        }

        Debug.LogWarning($"<color=cyan>GetType Performance > {swatch.ElapsedMilliseconds}</color>");
        swatch.Reset();
    }

    public IEnumerator Bench_TypeMatchSync() {

        swatch.Start();

        for (int i = 0; i < numberOfIteration; i++) {
            if (unitEntities[i] is MobileUnit) {

            }
            yield return wait4read_secs;
        }

        Debug.LogWarning($"<color=cyan>Type Match Performance > {swatch.ElapsedMilliseconds}</color>");
        swatch.Reset();
    }

    public void CreateList() {
        unitEntities = new List<UnitEntityBase>();
        for (int i = 0; i < numberOfIteration; i++) {
            UnitEntityBase ueb = Instantiate(mobileUnitPb);
            unitEntities.Add(Instantiate(mobileUnitPb));       
        }
        Debug.LogWarning($"<color=yellow>Total GameObject > {unitEntities.Count}</color>");
        Debug.LogWarning("<color=yellow>List Created!</color>");
    }

    public IEnumerator Bench_GetType_TypeMatch_Sync() {
        yield return Bench_GetTypeSync();
        yield return Bench_TypeMatchSync();
        //DestroyAllPrefabs();
        Debug.LogWarning($"<color=red>Bench Has Ended!</color>");
        GC.Collect();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}

public static class Utility {
    public static bool IsUnitEntity<T>(this T entity) {
        return entity.GetType().IsSubclassOf(typeof(MobileUnit));
    }
}
