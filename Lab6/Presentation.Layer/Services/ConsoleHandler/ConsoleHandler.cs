namespace Presentation.Layer.Services.ConsoleHandler;

public class ConsoleHandler
{
    private readonly MessageSystemServiceFacade.MessageSystemServiceFacade _messageSystemServiceFacade;

    public ConsoleHandler()
    {
        _messageSystemServiceFacade = new MessageSystemServiceFacade.MessageSystemServiceFacade();
    }

    public string ExecuteCommand(string[] command)
    {
        switch (command[0])
        {
            case "/login":
            {
                _messageSystemServiceFacade.LogIn(command[1], command[2]);

                return $"Вы успешно вошли в систему как \"{command[1]}\".";
            }

            case "/logout":
            {
                _messageSystemServiceFacade.LogOut();

                return "Вы вышли из системы.";
            }

            case "/add-account":
            {
                _messageSystemServiceFacade.AddAccount(command[1], command[2], command[3]);
                _messageSystemServiceFacade.Save();

                return $"Учётная запись для {command[1]} с логином {command[2]} успешно создана.";
            }

            case "/add-sms-message":
            {
                _messageSystemServiceFacade.AddSmsMessage(command[1], command[2]);
                _messageSystemServiceFacade.Save();

                return $"SMS-сообщение от {command[1]} успешно отправлено в систему.";
            }

            case "/add-email-message":
            {
                _messageSystemServiceFacade.AddEmailMessage(command[1], command[2]);
                _messageSystemServiceFacade.Save();

                return $"E-mail сообщение от {command[1]} успешно отправлено в систему.";
            }

            case "/add-messenger-message":
            {
                _messageSystemServiceFacade.AddMessengerMessage(command[1], command[2]);
                _messageSystemServiceFacade.Save();

                return $"Messenger-сообщение от {command[1]} успешно отправлено в систему.";
            }

            case "/request-messages":
            {
                return _messageSystemServiceFacade.RequestMessages();
            }

            case "/make-report":
            {
                return _messageSystemServiceFacade.MakeReport();
            }

            case "/make-message-received":
            {
                _messageSystemServiceFacade.MakeMessageReceived(command[1]);
                _messageSystemServiceFacade.Save();

                return $"Сообщение с ID {command[1]} помечено как полученное.";
            }

            case "/make-message-processed":
            {
                _messageSystemServiceFacade.MakeMessageProcessed(command[1]);
                _messageSystemServiceFacade.Save();

                return $"Сообщение с ID {command[1]} помечено как обработанное.";
            }

            case "/add-permission":
            {
                _messageSystemServiceFacade.AddPermission(command[1], command[2]);
                _messageSystemServiceFacade.Save();

                return
                    $"Сотруднику {command[1]} добавлено разрешение {command[2]}.";
            }

            case "/add-subordinate":
            {
                _messageSystemServiceFacade.AddSubordinate(command[1], command[2]);
                _messageSystemServiceFacade.Save();

                return $"Сотруднику {command[1]} добавлен подчинённый {command[2]}.";
            }

            case "/remove-subordinate":
            {
                _messageSystemServiceFacade.RemoveSubordinate(command[1], command[2]);
                _messageSystemServiceFacade.Save();

                return $"Сотрудник {command[2]} больше не является подчинённым сотрудника {command[1]}.";
            }

            case "/view-id":
            {
                string id = _messageSystemServiceFacade.GetUserId();

                return id == "null" ? "Вы не вошли в систему." : id;
            }

            case "/save":
            {
                _messageSystemServiceFacade.Save();

                return "Текущее состояние системы успешно сохранено.";
            }

            case "/load":
            {
                _messageSystemServiceFacade.Load();

                return "Загружено последнее сохранённое состояние системы.";
            }

            default:
            {
                return "Неизвестная команда.";
            }
        }
    }

    public string ShowCommands()
    {
        string commands = $"(!) СИСТЕМА ОБРАБОТКИ СООБЩЕНИЙ{Environment.NewLine}" +
                          $"Доступно 17 команд:{Environment.NewLine}" +
                          $"/login [логин] [пароль] - вход в систему.{Environment.NewLine}" +
                          $"/logout - выход из системы.{Environment.NewLine}" +
                          $"/add-account [имя владельца] [логин] [пароль] - регистрация в системе.{Environment.NewLine}" +
                          $"/add-sms-message [номер телефона источника] [содержимое] - отправка SMS сообщения в систему.{Environment.NewLine}" +
                          $"/add-email-message [e-mail адрес источника] [содержимое] - отправка E-mail сообщения в систему.{Environment.NewLine}" +
                          $"/add-messenger-message [имя источника] [содержимое] - отправка сообщения из мессенджера в систему.{Environment.NewLine}" +
                          $"/request-messages - запрос полученных сообщений (доступно только авторизованным пользователям).{Environment.NewLine}" +
                          $"/make-report - генерация отчёта (доступно только авторизованным руководителям).{Environment.NewLine}" +
                          $"/make-message-received [ID сообщения] - пометить сообщение как полученное.{Environment.NewLine}" +
                          $"/make-message-processed [ID сообщения] - пометить сообщение как обработанное.{Environment.NewLine}" +
                          $"/add-permission [ID сотрудника] [разрешение] - добавить разрешение сотруднику.{Environment.NewLine}" +
                          $"/add-subordinate [ID сотрудника] [ID подчинённого] - добавить сотруднику подчинённого.{Environment.NewLine}" +
                          $"/remove-subordinate [ID сотрудника] [ID подчинённого] - убрать подчинённого у сотрудника.{Environment.NewLine}" +
                          $"/view-id - посмотреть ID текущего пользователя.{Environment.NewLine}" +
                          $"/save - сохранить текущее состояние системы.{Environment.NewLine}" +
                          $"/load - загрузить последнее сохранённое состояние системы.{Environment.NewLine}" +
                          $"/exit - выход из программы.{Environment.NewLine}";

        return commands;
    }
}