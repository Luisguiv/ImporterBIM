using Dummiesman;
using System.IO;
using System.Text;
using UnityEngine;

public class ObjFromStream : MonoBehaviour {
	void Start () {
        //make www
        var www = new UnityEngine.Networking.UnityWebRequest("https://people.sc.fsu.edu/~jburkardt/data/obj/lamp.obj");
        www.SendWebRequest();

        while (!www.isDone)
            System.Threading.Thread.Sleep(1);
        
        //create stream and load
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.downloadHandler.text));
        var loadedObj = new OBJLoader().Load(textStream);
	}
}