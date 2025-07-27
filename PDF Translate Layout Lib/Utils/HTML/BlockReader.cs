using System.Text;

namespace PDF_Translate_Layout_Lib.Utils.HTML;

public class BlockReader
{
    private const char FlagForLastCommentStar = '&';

    private void Add(IDictionary<string, string> dict, string key, string value)
    {
        if (dict.ContainsKey(key))
        {
            var valurIn = dict[key];
            dict[key] = valurIn + value;
            // Console.WriteLine($"append {key}");
        }
        else
        {
            dict.Add(key,value);
        }
    }
    public Dictionary<string, string> ReadBlocks(string style)
    {
        var blocks = new Dictionary<string, string>();
        var keyBuilder = new StringBuilder();
        var valueBuilder = new StringBuilder();
        var stack = new Stack<char>();

        var inComment = false;
        var inBlock = false;
        var startComment = false;
        var readyToOutComment = false;
        var nestedBraces = 0;

        foreach (var c in style)
        {
            if (inComment)
            {
                // Console.WriteLine($" incomment");
                if (c == '*')
                {
                    if (stack.Peek() != FlagForLastCommentStar)
                    {
                        stack.Push(FlagForLastCommentStar);
                    }

                    readyToOutComment = true;
                }
                else if (c == '/')
                {
                    if (stack.Peek() == '&' && readyToOutComment)
                    {
                        stack.Pop(); // Pop '&'
                        stack.Pop();// Pop '*'
                        stack.Pop(); // Pop '/'
                        inComment = false;
                    }

                    readyToOutComment = false;
                }
                else
                {
                    if (c != '*')
                    {
                        if (stack.Peek() == FlagForLastCommentStar)
                        {
                            stack.Pop();
                        }
                    }
                    readyToOutComment = false;
                }
            }
            else if (startComment)
            {
                // Console.WriteLine($" startcomment");
                startComment = false;
                if (c == '*')
                {
                    stack.Push('*');
                    inComment = true;
                }
                else
                {
                    stack.Pop();
                }
            }
            else if (inBlock)
            {
                // Console.WriteLine($" inblock");
                if (c == '{')
                {
                    stack.Push(c);
                    if (nestedBraces > 0)
                    {
                        valueBuilder.Append(c);
                    }
                    nestedBraces++;
                }
                else if (c == '}')
                {
                    nestedBraces--;
                    if (nestedBraces == 0)
                    {
                        stack.Pop();
                        keyBuilder.Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        Add(blocks,keyBuilder.ToString().Trim(), valueBuilder.ToString().Trim());
                        // Console.WriteLine($"{keyBuilder.ToString().Trim()} " +
                        //                   $"lenght: {keyBuilder.ToString().Trim().Length}");
                        keyBuilder.Clear();
                        valueBuilder.Clear();
                        inBlock = false;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
                else
                {
                    valueBuilder.Append(c);
                }
            }
            else
            {
                // Console.WriteLine("not at all region");
                if (c == '/')
                {
                    stack.Push(c);
                    startComment = true;
                }
                else if (c == '*' && stack.Peek() == '/')
                {
                    stack.Push(c);
                    inComment = true;
                }
                else if (c == '{')
                {
                    stack.Push(c);
                    inBlock = true;
                    nestedBraces = 1;
                }
                else if (c == '}')
                {
                    // Ignore standalone closing brace
                }
                else
                {
                    keyBuilder.Append(c);
                }
            }
        }

        return blocks;
    }
}
