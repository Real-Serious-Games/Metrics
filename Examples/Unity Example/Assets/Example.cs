using UnityEngine;
using System.Collections;
using RSG;
using System.Collections.Generic;
using System.Text;

public class Example : MonoBehaviour, IHttpService
{
    private readonly string postUrl = "http://localhost:3000/emit";

    private Metrics metrics;

    public void Awake () 
    {
        metrics = new Metrics(new HttpJsonPostEmitter(postUrl, this));
    }
    
    public void Start () 
    {
        metrics.SetProperty("Metrics entry", "test");
        for (var i = 0; i < 10; i++)
        {
            metrics.Entry("Test int entry", i);
        }
        metrics.SetProperty("Second property", "true");
        metrics.Event("An event occurred");
        metrics.Entry("Test string entry", "foo");
        
        Debug.Log("Sent metrics data to " + postUrl);
    }

    public void Post(string url, string text)
    {
        // Create the headers to send in the post.
        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        // Create and send the www object.
        WWW www = new WWW(url, Encoding.UTF8.GetBytes(text), headers);
        SendPost(www);
    }

    private IEnumerator SendPost(WWW www)
    {
        yield return www;

        if (www.error != null)
        {
            Debug.LogError("Post error: " + www.error);
        }
    }
}
