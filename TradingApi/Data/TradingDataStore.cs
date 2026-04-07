using TradingLibrary.Models;

namespace TradingApi.Data;

public static class TradingDataStore
{
    public static List<Account> Accounts { get; } =
    [
        new() { AccountId = 1, ClientName = "Alice", AccountNumber = "ACC001", CashBalance = 100000 },
        new() { AccountId = 2, ClientName = "Bob", AccountNumber = "ACC002", CashBalance = 250000 },
        new() { AccountId = 3, ClientName = "Charlie", AccountNumber = "ACC003", CashBalance = 50000 },
        new() { AccountId = 4, ClientName = "David", AccountNumber = "ACC004", CashBalance = 180000 },
        new() { AccountId = 5, ClientName = "Emma", AccountNumber = "ACC005", CashBalance = 320000 },
        new() { AccountId = 6, ClientName = "Frank", AccountNumber = "ACC006", CashBalance = 90000 }
    ];

    public static List<Position> Positions { get; } =
    [
        // Account 1
        new() { PositionId = 1, AccountId = 1, EquityId = 1, Quantity = 50, AverageCostPerShare = 160 }, // AAPL
        new() { PositionId = 2, AccountId = 1, EquityId = 2, Quantity = 20, AverageCostPerShare = 300 }, // MSFT
        new() { PositionId = 3, AccountId = 1, EquityId = 7, Quantity = 5, AverageCostPerShare = 850 },  // NVDA

        // Account 2
        new() { PositionId = 4, AccountId = 2, EquityId = 3, Quantity = 10, AverageCostPerShare = 260 }, // TSLA
        new() { PositionId = 5, AccountId = 2, EquityId = 4, Quantity = 15, AverageCostPerShare = 130 }, // GOOG
        new() { PositionId = 6, AccountId = 2, EquityId = 9, Quantity = 25, AverageCostPerShare = 170 }, // AMD

        // Account 3
        new() { PositionId = 7, AccountId = 3, EquityId = 6, Quantity = 8, AverageCostPerShare = 280 },  // META
        new() { PositionId = 8, AccountId = 3, EquityId = 11, Quantity = 40, AverageCostPerShare = 90 }, // BABA

        // Account 4
        new() { PositionId = 9, AccountId = 4, EquityId = 8, Quantity = 6, AverageCostPerShare = 550 },  // NFLX
        new() { PositionId = 10, AccountId = 4, EquityId = 5, Quantity = 18, AverageCostPerShare = 120 }, // AMZN
        new() { PositionId = 11, AccountId = 4, EquityId = 14, Quantity = 30, AverageCostPerShare = 105 }, // DIS

        // Account 5
        new() { PositionId = 12, AccountId = 5, EquityId = 7, Quantity = 3, AverageCostPerShare = 880 },  // NVDA
        new() { PositionId = 13, AccountId = 5, EquityId = 12, Quantity = 35, AverageCostPerShare = 65 }, // SHOP
        new() { PositionId = 14, AccountId = 5, EquityId = 15, Quantity = 50, AverageCostPerShare = 58 }, // KO

        // Account 6
        new() { PositionId = 15, AccountId = 6, EquityId = 10, Quantity = 60, AverageCostPerShare = 40 }, // INTC
        new() { PositionId = 16, AccountId = 6, EquityId = 13, Quantity = 22, AverageCostPerShare = 70 }  // UBER
    ];

    public static List<Equity> Equities { get; } =
    [
        new() { EquityId = 1, Symbol = "AAPL", CurrentPrice = 175 },
        new() { EquityId = 2, Symbol = "MSFT", CurrentPrice = 320 },
        new() { EquityId = 3, Symbol = "TSLA", CurrentPrice = 250 },
        new() { EquityId = 4, Symbol = "GOOG", CurrentPrice = 140 },
        new() { EquityId = 5, Symbol = "AMZN", CurrentPrice = 130 },

        new() { EquityId = 6, Symbol = "META", CurrentPrice = 300 },
        new() { EquityId = 7, Symbol = "NVDA", CurrentPrice = 900 },
        new() { EquityId = 8, Symbol = "NFLX", CurrentPrice = 600 },
        new() { EquityId = 9, Symbol = "AMD", CurrentPrice = 180 },
        new() { EquityId = 10, Symbol = "INTC", CurrentPrice = 45 },

        new() { EquityId = 11, Symbol = "BABA", CurrentPrice = 85 },
        new() { EquityId = 12, Symbol = "SHOP", CurrentPrice = 70 },
        new() { EquityId = 13, Symbol = "UBER", CurrentPrice = 75 },
        new() { EquityId = 14, Symbol = "DIS", CurrentPrice = 110 },
        new() { EquityId = 15, Symbol = "KO", CurrentPrice = 60 }
    ];
}