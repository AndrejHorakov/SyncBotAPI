namespace SyncTelegramBot.Models.Entities;

public record ReceiptInvoice(string Number, string ВидОперации, int СуммаДокумента, Автор author)
{
    public override string ToString()
    {
        return $"{Number} {ВидОперации} {СуммаДокумента} {author}";
    }
}