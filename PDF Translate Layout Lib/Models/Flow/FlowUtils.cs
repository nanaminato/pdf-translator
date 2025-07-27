using PDF_Translate_Layout_Lib.Models.HTMLLayout;
using PDF_Translate_Layout_Lib.Models.PDFLayout;

namespace PDF_Translate_Layout_Lib.Models.Flow;

public class FlowUtils
{
    public static List<List<Flow>> GetFlows(HtmlLayoutWorkspace htmlWorkspace, PdfLayoutWorkspace pdfWorkspace)
    {
        var flows = new List<List<Flow>>();
        for (var i = 0; i < htmlWorkspace.Pages.Count; i++)
        {
            var pageFlows = new List<Flow>();
            /*
             * 获取pdf flow的一页
             */
            var pdfPage = pdfWorkspace.GetPage(i);
            /*
             * 获取html的一页
             */
            var htmlPage = htmlWorkspace.GetPage(i);
            /*
             * 获取该页的宽度和高度
             */
            var pdfDimension = pdfWorkspace.GetPageDimensionByPageId(i);
            var htmlDimension = new Dimension()
            {
                Width = htmlPage.Width,
                Height = htmlPage.Height
            };
            pdfPage.Rects = GetFitRects(pdfPage.Rects,pdfDimension, htmlDimension);
            // 循环处理一个pdf page的多个flow
            for (var j = 0; j < pdfPage.Rects.Count; j++)
            {
                var pdfFlowRect = pdfPage.Rects[j];
                var htmlLayouts = htmlPage.Children;
                var flowElements = GetFlowElements(pdfFlowRect, htmlLayouts);
                pageFlows.Add(new Flow()
                {
                    PageId = i,
                    FlowId = j,
                    Width = pdfFlowRect.Width,
                    Height = pdfFlowRect.Height,
                    FlowElements = flowElements
                });
            }
            flows.Add(pageFlows);
        }
        return flows;
    }

    
    
    /// <summary>
    /// 找到所有在pdfFlowRect 之内的Layout,includeRate是放宽限制，只要layout的一定比例的部分在pdfFlowRect 中，就认为在其中
    /// </summary>
    /// <param name="pdfFlowRect">一个pdf flow</param>
    /// <param name="htmlLayouts">html一页中的所有元素的布局</param>
    /// <param name="includeRate">可置信比率</param>
    /// <returns>在pdf flow 中的html的元素的布局</returns>
    private static List<Layout> GetFlowElements(Rectangle pdfFlowRect, List<Layout> htmlLayouts,double includeRate = 0.8)
    {
        var result = new List<Layout>();

        // 计算pdfFlowRect的边界
        var pdfLeft = pdfFlowRect.X;
        var pdfTop = pdfFlowRect.Y;
        var pdfRight = pdfFlowRect.X + pdfFlowRect.Width;
        var pdfBottom = pdfFlowRect.Y + pdfFlowRect.Height;

        foreach (var layout in htmlLayouts)
        {
            // 计算layout的边界
            var layoutLeft = layout.Left;
            var layoutTop = layout.Top;
            var layoutRight = layout.Left + layout.Width;
            var layoutBottom = layout.Top + layout.Height;

            // 计算重叠区域的边界
            var overlapLeft = Math.Max(pdfLeft, layoutLeft);
            var overlapTop = Math.Max(pdfTop, layoutTop);
            var overlapRight = Math.Min(pdfRight, layoutRight);
            var overlapBottom = Math.Min(pdfBottom, layoutBottom);

            // 计算重叠宽度和高度
            var overlapWidth = overlapRight - overlapLeft;
            var overlapHeight = overlapBottom - overlapTop;

            if (!(overlapWidth > 0) || !(overlapHeight > 0)) continue;
            var overlapArea = overlapWidth * overlapHeight;
            var layoutArea = layout.Width * layout.Height;

            // 判断重叠面积是否达到80%
            if (overlapArea >= includeRate * layoutArea)
            {
                result.Add(layout);
            }
        }

        return result;
    }


    public static List<Rectangle> GetFitRects(List<Rectangle> rects,Dimension pdfDimension, Dimension htmlDimension)
    {
        var wRate = htmlDimension.Width / pdfDimension.Width;
        var hRate = htmlDimension.Height / pdfDimension.Height;
        return rects.Select(rect => new Rectangle() { Width = wRate * rect.Width, Height = hRate * rect.Height, X = rect.X * wRate, Y = rect.Y * hRate }).ToList();
    }
}