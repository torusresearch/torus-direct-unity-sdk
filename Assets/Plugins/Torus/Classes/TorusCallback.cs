using UnityEngine;

namespace Torus
{
    public class TorusCallback
    {
        public GameObject gameObject { get; }
        public string method { get; }

        public TorusCallback(GameObject gameObject, string method)
        {
            this.gameObject = gameObject;
            this.method = method;
        }
    }

}