namespace Company.Welcome.Commons
{
    public interface ISerializer
    {
        string Serialize<TObject>(TObject toSerialize);

        TObject Deserialize<TObject>(string serializedObject);
    }
}