using SyncTelegramBot.Models.Entities;

namespace SyncTelegramBot.Models.HelpModels;

public static class StaticStructures
{
    public static readonly Dictionary<string, Type> Types = new()
    {
        ["Catalog_СтавкиНДС"] = typeof(AnswerFromUNF<VATRate>),
        ["Catalog_Сотрудники"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Catalog_Организации"] = typeof(AnswerFromUNF<OnlyDescription>),
        ["Document_РасходИзКассы"] = typeof(AnswerFromUNF<ReceiptInvoice>),
        ["Catalog_ВидыНалогов"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_Контрагенты"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Catalog_БанковскиеСчета"] = typeof(AnswerFromUNF<CodeAndDescription>),
        ["Document_ПриходнаяНакладная"] = typeof(AnswerFromUNF<ReceiptInvoice>),
        ["Document_ДоговорКредитаИЗайма"] = typeof(AnswerFromUNF<LoanAgreement>),
        ["ChartOfAccounts_Управленческий"] = typeof(AnswerFromUNF<Correspondence>),
        ["Catalog_ДоговорыКонтрагентов"] = typeof(AnswerFromUNF<CodeAndDescription>),
    };
}