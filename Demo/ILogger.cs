namespace Demo;

public interface ILogger
{
    void Log(ReadOnlySpan<char> messageTemplate, int arg);
}

public class ConsoleLogger : ILogger
{
    public void Log(ReadOnlySpan<char> messageTemplate, int arg)
    {
        int vaiableLength = 0;
        int variablePosition = 0;

        for (int i = 0; i < messageTemplate.Length; i++)
        {
            if (messageTemplate[i] != '{')
                continue;

            variablePosition = i;
            var variableCut = messageTemplate.Slice(i);
            for (int j = 0; j < variableCut.Length; j++)
            {
                vaiableLength++;
                if (messageTemplate[i] == '}')
                    break;
            }
        }

        Span<char> parsedText = stackalloc char[messageTemplate.Length - vaiableLength + 1];

        var index = 0;
        for (int i = 0; i < parsedText.Length; i++)
        {
            if (i == variablePosition)
            {
                parsedText[i] = (char)(arg+48);
                index += vaiableLength -1;
                continue;
            }

            parsedText[i] = messageTemplate[index + i];
        }

        var text = parsedText.ToString();
    }
}