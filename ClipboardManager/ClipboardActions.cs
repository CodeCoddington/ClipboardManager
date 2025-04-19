using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardManager
{
    public partial class form_clipboardManager : Form
    {
        public enum TextModifier
        {
            Nothing,
            Quotes,
            Parentheses,
            CurlyBrackets,
            SquareBrackets,
            At,
            Dollar,
            CRLF
        }

        private TextModifier GetModifier(string comboText)
        {
            switch (comboText)
            {
                case "text":
                    return TextModifier.Nothing;
                case "\"text\"":
                    return TextModifier.Quotes;
                case "(text)":
                    return TextModifier.Parentheses;
                case "{text}":
                    return TextModifier.CurlyBrackets;
                case "[text]":
                    return TextModifier.SquareBrackets;
                case "@text":
                    return TextModifier.At;
                case "$text":
                    return TextModifier.Dollar;
                case @"[R]text[R]":
                    return TextModifier.CRLF;
                default:
                    return TextModifier.Nothing; // Default fallback
            }
        }

        private async void Btn_clearAll_Click(object sender, EventArgs e)
        {
            // User verification
            DialogResult dr = MessageBox.Show("This will clear all clipboard history. This cannot be undone. Continue?", "Clear History?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dr == DialogResult.Yes)
            {
                await ClearAllDataAsync();
            }
        }

        private async Task ClearAllDataAsync()
        {
            // Clear Clipboard
            Clipboard.Clear();

            // Clear SQL database
            string result = await ClearAllClipLogDataAsync();
            if (result == SQL_ERR_CLEAR)
            {
                MessageBox.Show("There was an error clearing the database. Closing application.");
                this.Close();
            }

            // Refresh (Clear) Panel
            RefreshForm();

            // Reset global vars
            currTabIndex = 0;
            clipHasValidText = false;
            eventsEnabled = true;

            Btn_changeFormSize_Click(this, EventArgs.Empty);
        }


        private void StringModifier_Clicked(object sender, EventArgs e)
        {
            if (!clipHasValidText) return;

            Button modButton = sender as Button;

            string text = string.Empty;
            string modifiedText = string.Empty;

            TextModifier modifier = GetModifier(modButton.Text);

            text = Clipboard.GetText();

            if (!string.IsNullOrEmpty(text))
            {
                modifiedText = ModifiedText(text, modifier);
                Clipboard.SetText(modifiedText);
            }
        }

        private string ModifiedText(string text, TextModifier modifier)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string modifiedText;

            switch (modifier)
            {
                case TextModifier.Nothing:
                    modifiedText = text;
                    break;

                case TextModifier.Quotes:
                    modifiedText = text.StartsWith("\"") && text.EndsWith("\"")
                        ? text.Trim('"')
                        : $"\"{text}\"";
                    break;

                case TextModifier.Parentheses:
                    modifiedText = text.StartsWith("(") && text.EndsWith(")")
                        ? text.Trim('(', ')')
                        : $"({text})";
                    break;

                case TextModifier.CurlyBrackets:
                    modifiedText = text.StartsWith("{") && text.EndsWith("}")
                        ? text.Trim('{', '}')
                        : $"{{{text}}}";
                    break;

                case TextModifier.SquareBrackets:
                    modifiedText = text.StartsWith("[") && text.EndsWith("]")
                        ? text.Trim('[', ']')
                        : $"[{text}]";
                    break;

                case TextModifier.At:
                    modifiedText = text.StartsWith("@")
                        ? text.TrimStart('@')
                        : $"@{text}";
                    break;

                case TextModifier.Dollar:
                    modifiedText = text.StartsWith("$")
                        ? text.TrimStart('$')
                        : $"${text}";
                    break;

                case TextModifier.CRLF:
                    modifiedText = text.StartsWith("\r\n") && text.EndsWith("\r\n")
                        ? text.Trim('\r', '\n')
                        : $"{Environment.NewLine}{text}{Environment.NewLine}";
                    break;

                default:
                    modifiedText = text;
                    break;
            }

            return modifiedText;
        }
    }
}
