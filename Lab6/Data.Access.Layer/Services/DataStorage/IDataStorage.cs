using Business.Layer.Services.MessageSystemService;

namespace Data.Access.Layer.Services.DataStorage;

public interface IDataStorage
{
    MessageSystemService LoadData();

    void SaveData(MessageSystemService messageSystemService);
}