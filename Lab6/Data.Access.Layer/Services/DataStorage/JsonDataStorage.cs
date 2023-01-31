using System.Text.Json;
using Business.Layer.Entities.Account;
using Business.Layer.Models.AccountInfo;
using Business.Layer.Models.Message;
using Business.Layer.Services.MessageSystemService;
using Data.Access.Layer.Exceptions.JsonDataStorageException;

namespace Data.Access.Layer.Services.DataStorage;

public class JsonDataStorage : IDataStorage
{
    private readonly string _path;

    public JsonDataStorage(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!path.EndsWith(Path.DirectorySeparatorChar))
        {
            throw JsonDataStorageException.PathMustEndWithDirectorySeparatorChar();
        }

        _path = path;
    }

    public MessageSystemService LoadData()
    {
        string accountsJson = File.ReadAllLines($"{_path}accounts.json").ToList().First(),
            smsMessagesJson = File.ReadAllLines($"{_path}smsMessages.json").ToList().First(),
            emailMessagesJson = File.ReadAllLines($"{_path}emailMessages.json").ToList().First(),
            messengerMessagesJson = File.ReadAllLines($"{_path}messengerMessages.json").ToList().First(),
            accountInfosJson = File.ReadAllLines($"{_path}accountInfos.json").ToList().First();

        List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(accountsJson) ??
                                 throw JsonDataStorageException.UnableToDeserializeAccounts();

        List<SmsMessage> smsMessages = JsonSerializer.Deserialize<List<SmsMessage>>(smsMessagesJson) ??
                                       throw JsonDataStorageException.UnableToDeserializeSmsMessages();

        List<EmailMessage> emailMessages = JsonSerializer.Deserialize<List<EmailMessage>>(emailMessagesJson) ??
                                           throw JsonDataStorageException.UnableToDeserializeEmailMessages();

        List<MessengerMessage> messengerMessages =
            JsonSerializer.Deserialize<List<MessengerMessage>>(messengerMessagesJson) ??
            throw JsonDataStorageException.UnableToDeserializeMessengerMessages();

        List<AccountInfo> accountInfos = JsonSerializer.Deserialize<List<AccountInfo>>(accountInfosJson) ??
                                         throw JsonDataStorageException.UnableToDeserializeAccountInfos();

        var messages = new List<IMessage>();

        foreach (SmsMessage smsMessage in smsMessages)
        {
            messages.Add(smsMessage);
        }

        foreach (EmailMessage emailMessage in emailMessages)
        {
            messages.Add(emailMessage);
        }

        foreach (MessengerMessage messengerMessage in messengerMessages)
        {
            messages.Add(messengerMessage);
        }

        return new MessageSystemService(accounts, messages, accountInfos);
    }

    public void SaveData(MessageSystemService messageSystemService)
    {
        IReadOnlyList<Account> accounts = messageSystemService.Accounts;
        IReadOnlyList<IMessage> messages = messageSystemService.Messages;
        IReadOnlyList<AccountInfo> accountInfos = messageSystemService.AccountInfos();

        var smsMessages = new List<IMessage>();
        var emailMessages = new List<IMessage>();
        var messengerMessages = new List<IMessage>();

        foreach (IMessage message in messages)
        {
            string messageType = message.MessageType;

            if (messageType == "SMS")
            {
                smsMessages.Add(message);
            }

            if (messageType == "E-mail")
            {
                emailMessages.Add(message);
            }

            if (messageType == "Messenger")
            {
                messengerMessages.Add(message);
            }
        }

        string accountsJson = JsonSerializer.Serialize(accounts),
            smsMessagesJson = JsonSerializer.Serialize(smsMessages),
            emailMessagesJson = JsonSerializer.Serialize(emailMessages),
            messengerMessagesJson = JsonSerializer.Serialize(messengerMessages),
            accountInfosJson = JsonSerializer.Serialize(accountInfos);

        File.WriteAllText($"{_path}accounts.json", accountsJson);
        File.WriteAllText($"{_path}smsMessages.json", smsMessagesJson);
        File.WriteAllText($"{_path}emailMessages.json", emailMessagesJson);
        File.WriteAllText($"{_path}messengerMessages.json", messengerMessagesJson);
        File.WriteAllText($"{_path}accountInfos.json", accountInfosJson);
    }
}