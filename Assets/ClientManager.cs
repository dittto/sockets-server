using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public GameObject clientPrefab;
    public GameObject map;
    private Dictionary<int, Vector2> clientPos = new Dictionary<int, Vector2>();
    private Dictionary<int, Vector2> clientTargets = new Dictionary<int, Vector2>();
    private Dictionary<int, GameObject> clientObj = new Dictionary<int, GameObject>();
    private Vector2 randomPos = new Vector2();

    public Vector2? AddClient(int clientId)
    {
        if (!clientPos.ContainsKey(clientId)) {

            clientPos.Add(clientId, new Vector2(randomPos.x, randomPos.y));
            clientTargets.Add(clientId, new Vector2(randomPos.x - 0.5f, randomPos.y - 0.5f));

            return clientPos[clientId];
        }

        return null;
    }

    public void UpdateClient(int clientId, float x, float y)
    {
        if (clientTargets.ContainsKey(clientId)) {
            clientTargets[clientId] = new Vector2(x - 0.5f, y - 0.5f);
        }
    }

    public List<Dictionary<string, object>> GetClients()
    {
        List <Dictionary<string, object>> clientList = new List<Dictionary<string, object>>();

        foreach (KeyValuePair<int, Vector2> kvp in clientPos) {
            clientList.Add(new Dictionary<string, object> { { "id", kvp.Key }, { "x", kvp.Value.x }, { "y", kvp.Value.y } });
        }

        return clientList;
    }
    
    private void Update ()
    {
        randomPos = new Vector2(Random.value, Random.value);

        foreach (KeyValuePair<int, Vector2> kvp in clientTargets) {
            int clientId = kvp.Key;
            if (!clientObj.ContainsKey(clientId)) {
                clientObj.Add(clientId, Instantiate(clientPrefab));
                clientObj[clientId].transform.parent = map.transform;
                clientObj[clientId].transform.position = new Vector3((kvp.Value.x * map.transform.localScale.x) * -1f, 0, kvp.Value.y * map.transform.localScale.z);
            }

            clientObj[clientId].transform.position = Vector3.MoveTowards(clientObj[clientId].transform.position, new Vector3((clientTargets[clientId].x * map.transform.localScale.x) * -1f, 0, clientTargets[clientId].y * map.transform.localScale.z), 0.2f);

            clientPos[clientId] = new Vector2(((clientObj[clientId].transform.position.x / map.transform.localScale.x) * -1f) + 0.5f, (clientObj[clientId].transform.position.z / map.transform.localScale.z) + 0.5f);
        }
    }
}
