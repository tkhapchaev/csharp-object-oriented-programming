namespace Banks.Console;

public static class Program
{
    public static void Main()
    {
        var banksConsoleHandler = new BanksConsoleHandler();
        string? command;

        ShowCommands();

        while ((command = System.Console.ReadLine()) != "/exit")
        {
            string[] arguments = command?.Split(" ") ?? throw new ArgumentNullException();

            switch (arguments[0])
            {
                case "/add-bank":
                {
                    string name = arguments[1],
                        transferLimit = arguments[2],
                        withdrawalLimit = arguments[3],
                        balancePercentage = arguments[4],
                        minimalCreditBalance = arguments[5],
                        creditAccountCommission = arguments[6];

                    banksConsoleHandler.AddBank(
                        name,
                        transferLimit,
                        withdrawalLimit,
                        balancePercentage,
                        minimalCreditBalance,
                        creditAccountCommission);

                    System.Console.WriteLine($"Банк \"{name}\" успешно создан.");

                    break;
                }

                case "/add-client":
                {
                    string name = arguments[1],
                        surname = arguments[2],
                        passportSeries = arguments[3],
                        passportNumber = arguments[4],
                        country = arguments[5],
                        city = arguments[6],
                        street = arguments[7],
                        houseNumber = arguments[8];

                    banksConsoleHandler.AddClient(
                        name,
                        surname,
                        passportSeries,
                        passportNumber,
                        country,
                        city,
                        street,
                        houseNumber);

                    System.Console.WriteLine($"Клиент \"{name} {surname}\" успешно создан.");

                    break;
                }

                case "/add-debit-account":
                {
                    string accountId = banksConsoleHandler.AddDebitAccount(arguments[1], arguments[2], arguments[3]);

                    System.Console.WriteLine($"Счёт {accountId} успешно создан.");

                    break;
                }

                case "/add-deposit-account":
                {
                    string accountId = banksConsoleHandler.AddDepositAccount(
                        arguments[1],
                        arguments[2],
                        arguments[3],
                        arguments[4],
                        arguments[5]);

                    System.Console.WriteLine($"Счёт {accountId} успешно создан.");

                    break;
                }

                case "/add-credit-account":
                {
                    string accountId = banksConsoleHandler.AddCreditAccount(
                        arguments[1],
                        arguments[2],
                        arguments[3],
                        arguments[4]);

                    System.Console.WriteLine($"Счёт {accountId} успешно создан.");

                    break;
                }

                case "/top-up":
                {
                    banksConsoleHandler.TopUp(arguments[1], arguments[2]);

                    break;
                }

                case "/withdraw":
                {
                    banksConsoleHandler.Withdraw(arguments[1], arguments[2]);

                    break;
                }

                case "/transfer":
                {
                    banksConsoleHandler.Transfer(arguments[1], arguments[2], arguments[3]);

                    break;
                }

                case "/get-balance":
                {
                    System.Console.WriteLine(banksConsoleHandler.GetBalance(arguments[1]));

                    break;
                }

                case "/get-accounts":
                {
                    List<string> accounts = banksConsoleHandler.GetAccounts(arguments[1], arguments[2]);

                    foreach (string account in accounts)
                    {
                        System.Console.WriteLine(account);
                    }

                    break;
                }

                case "/exit":
                {
                    Environment.Exit(0);

                    break;
                }

                default:
                {
                    System.Console.WriteLine("Неизвестная команда.");

                    break;
                }
            }
        }
    }

    private static void ShowCommands()
    {
        System.Console.WriteLine("\n(!) CENTRAL BANK CLI\n\n" + "Доступны 11 команд:\n");

        System.Console.WriteLine(
            "/add-bank [имя банка] [лимит на перевод] [лимит на снятие] [процент на остаток] [минимальный кредитный баланс] [комиссия (для кредитных счетов)]");

        System.Console.WriteLine(
            "/add-client [имя клиента] [фамилия клиента] [серия паспорта] [номер паспорта] [страна] [город] [улица] [дом]");

        System.Console.WriteLine("/add-debit-account [имя банка] [имя клиента] [фамилия клиента]");

        System.Console.WriteLine(
            "/add-deposit-account [имя банка] [имя клиента] [фамилия клиента] [депозит] [продолжительность]");

        System.Console.WriteLine("/add-credit-account [имя банка] [имя клиента] [фамилия клиента] [кредит]");

        System.Console.WriteLine("/top-up [ID счёта] [сумма]");

        System.Console.WriteLine("/withdraw [ID счёта] [сумма]");

        System.Console.WriteLine("/transfer [ID счёта отправителя] [ID счёта получателя] [сумма]");

        System.Console.WriteLine("/get-balance [ID счёта]");

        System.Console.WriteLine("/get-accounts [имя клиента] [фамилия клиента]");

        System.Console.WriteLine("/get-notifications [имя клиента] [фамилия клиента]");

        System.Console.WriteLine("/exit - выход.\n");
    }
}