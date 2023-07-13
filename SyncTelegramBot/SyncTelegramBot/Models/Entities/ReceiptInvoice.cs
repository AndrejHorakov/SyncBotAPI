namespace SyncTelegramBot.Models.Entities;

public record ReceiptInvoice(string Number, string ВидОперации, int СуммаДокумента, OnlyDescription author)
{
    public override string ToString()
    {
        return $"{Number} {ВидОперации} {СуммаДокумента} {author}";
    }
}