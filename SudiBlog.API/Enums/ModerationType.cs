using System.ComponentModel;

namespace SudiBlog.API.Enums
{
    public enum ModerationType
    {
        [Description("Politicla Propaganda")]
        Political,
        [Description("Offensive Language")]
        Langauge,
        [Description("Drug references")]
        Drugs,
        [Description("Threatening Speech")]
        Threatening,
        [Description("Sexual Content")]
        Sexual,
        [Description("Hate Speech")]
        HateSpeech,
        [Description("Tageted Shaming ")]
        Shaming
    }
}