using System;
using System.Collections.Generic;
using UnityEngine;

namespace Envast.Models
{
    [Serializable]
    public class CouponModel 
    {
        public string id;
        public string value;
        public int numbersOfItems;
        public Sprite image;
        public  string ToJson() => JsonUtility.ToJson(this);
        public  Dictionary<string, string> ToForm()
        {
            return new Dictionary<string, string>
            {
                {"id", id.ToString()},
                {"value", value},
            };
        }
        public  int GetId() => int.Parse(id);
        public  string GetUri(bool includeBase = true) => throw new System.NotImplementedException();
    }
}
