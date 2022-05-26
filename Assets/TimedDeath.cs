using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeath : MonoBehaviour
{
    public int pointIndex = 0;
    public Positions[] points;

    public GameObject deathBox;

    bool reachedEnd = false;

    private void Update()
    {
        if (!GameManager.instance.isCorruptionFrozen && !reachedEnd) {
            deathBox.transform.rotation = Quaternion.Lerp(deathBox.transform.rotation, Quaternion.Euler(0, 0, points[pointIndex].rot), Time.deltaTime * points[pointIndex].turnSpeed);
            deathBox.transform.position += (deathBox.transform.position = points[pointIndex].pos).normalized * points[pointIndex].speed * Time.deltaTime;
            //deathBox.transform.position = Vector3.Lerp(deathBox.transform.position, points[pointIndex].pos, Time.deltaTime * points[pointIndex].speed);
        }

        bool hasRotated = false;
        bool hasMoved = false;

        float currentRotDist = Quaternion.Angle(deathBox.transform.rotation, Quaternion.Euler(0, 0, points[pointIndex].rot));
        if (currentRotDist < 3f && currentRotDist > -3f)
            hasRotated = true;
        if ((deathBox.transform.position - points[pointIndex].pos).magnitude < 5f && (deathBox.transform.position - points[pointIndex].pos).magnitude > -5f)
            hasMoved = true;

        if (hasMoved && hasRotated) {
            if (points.Length != pointIndex + 1) {
                pointIndex++;
            } else {
                reachedEnd = true;
            }     
        }
    }

    [System.Serializable]
    public class Positions {
        public Vector3 pos;
        public float rot;
        public float speed;
        public float turnSpeed;
    }
}
