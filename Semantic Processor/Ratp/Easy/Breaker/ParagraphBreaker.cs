using PDF_Translator_Utils.Utils.HTML;
using Semantic_Processor.Natp.Parameters;
using Semantic_Processor.Ratp.Page.Processors;
using Semantic_Processor.Ratp.Page.Region;

namespace Semantic_Processor.Ratp.Easy.Breaker;

public abstract class ParagraphBreaker
{
    public abstract List<SemanticParagraph> Breaker(List<LineRegion> regions, StyleNode styleNode,
        DebugParameter parameter, List<(int, double, double, double, double)> scopes);
}