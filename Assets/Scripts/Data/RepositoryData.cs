using Assets.Scripts.Models;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    [System.Serializable]
    public sealed class RepositoryData
    {
        public List<KeyValue<string, string>> Data;
    }
}