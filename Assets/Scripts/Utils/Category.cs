using System.Collections.Generic;

namespace GravityTanks.Utils
{
    [System.Serializable]
    public struct Category
    {
        public string name;
        public List<Author> authors;
    }
}