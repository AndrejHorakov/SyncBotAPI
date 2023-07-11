namespace SyncTelegramBot.Models.Entities;

public record LoanAgreement(string Number, string ВидДоговора, int РазмерПлатежа, int СуммаДокумента)
{
    public override string ToString()
    {
        return $"{Number} {ВидДоговора} {РазмерПлатежа}/{СуммаДокумента}";
    }
}