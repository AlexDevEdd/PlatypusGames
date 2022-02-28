
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _agentRadius;
    [SerializeField] private float _countSpawn;
    [SerializeField] private float _spawnDelay = 1f;

    private Vector3 originPoint;

    private List<Vector3> freePosList = new List<Vector3>();
    private float possibleCount;

    private Vector3 placeFreePos;
    private Action FindPlace;
    private Action CreateAgent;

    private void Start()
    {
        _agentRadius = _prefab.transform.localScale.x;
        originPoint = transform.position;
        FindPlace = FindPlaceWithCircle;
        CreateAgent = CreateAgentWithCircle;
        FindPlace?.Invoke();
        StartCoroutine(CreateAgentCoroutine());
    }

    private IEnumerator CreateAgentCoroutine()
    {
        CreateAgent?.Invoke();
        yield return new WaitForSeconds(_spawnDelay);
        StartCoroutine(CreateAgentCoroutine());
    }

    private void FindPlaceWithCircle()
    {
        Vector3 point = originPoint;
        float distRing = 2 * 3.14f * _spawnRadius;

        var angle = 360 * Mathf.Deg2Rad;
        for (float i = 0; i <= _spawnRadius; i++)
        {
            possibleCount = (int)distRing / _agentRadius;
            for (int j = 0; j <= possibleCount; j++)
            {
                float posZ = originPoint.z + Mathf.Cos(angle / possibleCount * j) * _spawnRadius;
                float posX = originPoint.z + Mathf.Sin(angle / possibleCount * j) * _spawnRadius;
                point.z = posZ;
                point.x = posX;

                freePosList.Add(point);
            }

            _spawnRadius -= _agentRadius;
            distRing = 2 * 3.14f * _spawnRadius;
        }
    }

    private void CreateAgentWithCircle()
    {
        if (_countSpawn > 0)
        {
            float directionFacing = UnityEngine.Random.Range(0f, 360f);
            if(freePosList.Count > 0)
            {
                placeFreePos = freePosList[UnityEngine.Random.Range(0, freePosList.Count - 1)];
                freePosList.Remove(placeFreePos);
                Instantiate(_prefab, placeFreePos, Quaternion.Euler(new Vector3(0f, directionFacing, 0f)));
                _countSpawn--;
            }
        }
    }
}

