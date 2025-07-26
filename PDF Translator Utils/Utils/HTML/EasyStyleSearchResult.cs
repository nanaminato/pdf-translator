namespace PDF_Translator_Utils.Utils.HTML;

public class EasyStyleSearchResult
{
    public string? FindClass
    {
        get;
        set;
    }

    public string? Result
    {
        get;
        set;
    }

    public override string ToString()
    {
        return $"class: {FindClass}\nResult: \n{Result}";
    }
}