using PDF_Translate_Layout_Lib.Natp.Parameters;
using PDF_Translate_Layout_Lib.Ratp.Page.Processors;
using PDF_Translate_Layout_Lib.Ratp.Page.Region;
using PDF_Translate_Layout_Lib.Utils.HTML;

namespace PDF_Translate_Layout_Lib.Ratp.Easy.Breaker;

public abstract class ParagraphBreaker
{
    public abstract List<SemanticParagraph> Breaker(List<LineRegion> regions, StyleNode styleNode,
        DebugParameter parameter, List<(int, double, double, double, double)> scopes);
}