using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class RotatingEnemy : MonoBehaviour
{
    private Health health;

    public float speed;

    private Transform target;

    // Use this for initialization
    void Start()
    {
        health = GetComponent<Health>();

        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
            return;

        target = players[0].transform;
        var minDistance = Vector3.Distance(players[0].transform.position, transform.position);
        foreach (var player in players)
        {
            var distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < minDistance)
            {
                target = player.transform;
                minDistance = distance;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (health.health <= 0f)
            Destroy(gameObject);

        if (target == null)
            return;

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                target.position + new Vector3(0f, 2f, 0f),
                speed * Time.deltaTime);
    }
}
