using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour {

    private Vector2 v1, v2, v3, v4;

    public float maxBoidDistance = 5;
    public float speed = 0.01f;
    public float movementSpeed = 5f;
    public GameObject[] boids;

    private void Update()
    {
        MoveAllBoids();
    }

    private void MoveAllBoids(){
        for(int i = 0; i < boids.Length; i++){
            v1 = Cohesion(boids[i]);
            v2 = Separation(boids[i]);
            v3 = Allignment(boids[i]);
            v4 = WallAvoidance(boids[i]);

            Vector2 totalVector = (v1 + v2 + v3 + v4) * speed;
            totalVector = totalVector.magnitude > movementSpeed ? totalVector.normalized * movementSpeed : totalVector;
            boids[i].GetComponent<Rigidbody2D>().velocity = totalVector; 
        }
    }

    private Vector2 Cohesion(GameObject boid){
        Vector2 c = Vector2.zero;
        for (int i = 0; i < boids.Length; i++){
            c += new Vector2(boids[i].transform.position.x, boids[i].transform.position.y);
        }
        c = c / boids.Length;

        return c;
    }

    private Vector2 Separation(GameObject boid){

        Vector2 c = Vector2.zero;
        for (int i = 0; i < boids.Length; i++){
            if(boids[i] != boid){
                float boidDistance = Vector2.Distance(boid.transform.position, boids[i].transform.position);
                if(boidDistance < maxBoidDistance){
                    c = c - new Vector2((boids[i].transform.position.x - boid.transform.position.x), (boids[i].transform.position.y - boid.transform.position.y));
                }

            }
        }

        return c;
    }

    private Vector2 Allignment(GameObject boid){
        Vector2 avgDirection = Vector2.zero;

        for (int i = 0; i < boids.Length; i++){
            if (boids[i] != boid){
                avgDirection += boids[i].GetComponent<Rigidbody2D>().velocity;
            }
        }
        avgDirection = avgDirection / (boids.Length - 1);

        return (avgDirection - boid.GetComponent<Rigidbody2D>().velocity) / 8;
    }

    private Vector2 WallAvoidance(GameObject boid)
    {
        Vector2 wallAvoidance = Vector2.zero;
        for (int i = 0; i < boids.Length; i++){
            if(Physics2D.OverlapCircle(boids[i].transform.position, 3f)){
                
            }
        }

        return wallAvoidance;
    }

}
