namespace SyncTelegramBot.Models.Entities;

public record Expense(string Number, int СуммаДокумента, Employee Подотчетник)
{
    public override string ToString()
    {
        return $"{Number} {Подотчетник} - {СуммаДокумента}";
    }
}