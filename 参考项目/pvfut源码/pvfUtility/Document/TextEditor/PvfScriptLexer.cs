using System.Collections.Generic;
using System.Threading.Tasks;
using ScintillaNET;

namespace pvfUtility.Document.TextEditor
{
    public static class PvfScriptLexer
    {
        public const int StyleDefault = 0;
        public const int StyleIdentifier = 1;
        public const int StyleNumber = 2;
        public const int StyleString = 3;
        public const int StyleSection = 4;
        public const int StyleStringLink = 5;
        private static readonly HashSet<string> _keywords;

        static PvfScriptLexer()
        {
            // Put keywords in a HashSet
            var list = new List<string> {"#PVF_File"};
            _keywords = new HashSet<string>(list);
        }

        public static Task Style(Scintilla scintilla, int startPos, int endPos)
        {
            const int stateUnknown = 0;
            const int stateIdentifier = 1;
            const int stateNumber = 2;
            const int stateString = 3;
            const int stateSection = 4;
            const int stateStringLink = 5;
            // Back up to the line start
            var line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;
            var length = 0;
            var state = stateUnknown;

            var inStringLink = false;
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
                        else if (c == '[')
                        {
                            // Start of "string"
                            scintilla.SetStyling(1, StyleSection);
                            state = stateSection;
                        }
                        else if (c == '<')
                        {
                            // Start of "string"
                            scintilla.SetStyling(1, StyleStringLink);
                            state = stateStringLink;
                        }
                        else if (char.IsDigit(c))
                        {
                            state = stateNumber;
                            goto REPROCESS;
                        }
                        else if (char.IsLetter(c))
                        {
                            state = stateIdentifier;
                            goto REPROCESS;
                        }
                        else
                        {
                            // Everything else
                            scintilla.SetStyling(1, StyleDefault);
                        }

                        break;
                    case stateSection:
                        if (c == ']')
                        {
                            length++;
                            scintilla.SetStyling(length, StyleSection);
                            length = 0;
                            state = stateUnknown;
                        }
                        else
                        {
                            length++;
                        }

                        break;
                    case stateStringLink:
                        if (c == '`') //针对skill/atgunner/tripleclutch.skl 进行调整
                        {
                            length++;
                            inStringLink = !inStringLink;
                        }
                        else if (c == '>' && !inStringLink)
                        {
                            length++;
                            scintilla.SetStyling(length, StyleStringLink);
                            length = 0;
                            state = stateUnknown;
                        }
                        else
                        {
                            length++;
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
                    case stateIdentifier:
                        if (char.IsLetterOrDigit(c))
                        {
                            length++;
                        }
                        else
                        {
                            var style = StyleIdentifier;
                            var identifier = scintilla.GetTextRange(startPos - length, length);
                            if (_keywords.Contains(identifier)) style = StyleIdentifier;
                            scintilla.SetStyling(length, style);
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