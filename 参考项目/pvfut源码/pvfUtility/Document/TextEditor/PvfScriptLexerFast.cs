using System.Threading.Tasks;
using ScintillaNET;

namespace pvfUtility.Document.TextEditor
{
    public static class PvfScriptLexerFast
    {
        public const int StyleDefault = 0;
        public const int StyleNumber = 2;
        public const int StyleString = 3;

        public static Task Style(Scintilla scintilla, int startPos, int endPos)
        {
            const int stateUnknown = 0;
            const int stateNumber = 2;
            const int stateString = 3;
            // Back up to the line start
            var line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;
            var length = 0;
            var state = stateUnknown;

            // Start styling
            scintilla.StartStyling(startPos);
            while (startPos < endPos)
            {
                var c = (char) scintilla.GetCharAt(startPos);
                REPROCESS:
                switch (state)
                {
                    case stateUnknown:
                        if (c == '`')
                        {
                            // Start of "string"
                            scintilla.SetStyling(1, StyleString);
                            state = stateString;
                        }
                        else if (char.IsDigit(c))
                        {
                            state = stateNumber;
                            goto REPROCESS;
                        }
                        else
                        {
                            // Everything else
                            scintilla.SetStyling(1, StyleDefault);
                        }

                        break;
                    case stateString:
                        if (c == '`')
                        {
                            length++;
                            scintilla.SetStyling(length, StyleString);
                            length = 0;
                            state = stateUnknown;
                        }
                        else
                        {
                            length++;
                        }

                        break;
                    case stateNumber:
                        if (char.IsDigit(c) || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F' || c == 'x')
                        {
                            length++;
                        }
                        else
                        {
                            scintilla.SetStyling(length, StyleNumber);
                            length = 0;
                            state = stateUnknown;
                            goto REPROCESS;
                        }

                        break;
                }

                startPos++;
            }

            return Task.FromResult(true);
        }
    }
}