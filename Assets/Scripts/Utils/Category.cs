using System.Collections.Generic;

namespace HNW.Utils
{
    [System.Serializable]
    public struct Category
    {
        public string name;
        public List<Author> authors;
    }
}