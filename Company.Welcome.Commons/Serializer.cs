using Newtonsoft.Json;

namespace Company.Welcome.Commons
{
    public class Serializer : ISerializer
    {
        public string Serialize<TObject>(TObject toSerialize)
        {
            return JsonConvert.SerializeObject(toSerialize);
        }

        public TObject Deserialize<TObject>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<TObject>(serializedObject);
        }
    }
}