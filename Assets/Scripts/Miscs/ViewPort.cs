using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPort : Singleton<ViewPort>
{
    float minX;
    float minY;
    float maxX;
    float maxY;
    float middleX;
    float middleY;

    void Start()
    {
        Camera camera = Camera.main;
        Vector2 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 upRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f));
        middleX = camera.ViewportToWorldPoint(new Vector3(0.5f, 0f)).x;
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = upRight.x;
        maxY = upRight.y;
    }

    public Vector3 PlayerEnablePosition(Vector3 playerPositions, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPositions.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPositions.y, minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomEnemyCreatePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomEnemyMoveRightPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomEnemyMoveAllPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

}
