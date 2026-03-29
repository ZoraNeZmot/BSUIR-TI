using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cipher1
{
    class Cipherer
    {
        public enum CipherType
        {
            Column,
            Visner
        }

        public static string PerformCiphering(string content, string key, CipherType type)
        {
            content = CipherFont.RemoveNonAlphabetic(content).ToUpper();
            key = key.ToUpper();

            switch (type)
            {
                case CipherType.Column:
                    return CipherColumn(content, key);
                case CipherType.Visner:
                    return CipherVisner(content, key);
                default:
                    return "";
            }
        }

        public static string PerformDeciphering(string content, string key, CipherType type)
        {
            content = CipherFont.RemoveNonAlphabetic(content).ToUpper();
            key = key.ToUpper();

            switch (type)
            {
                case CipherType.Column:
                    return DecipherColumn(content, key);
                case CipherType.Visner:
                    return DecipherVisner(content, key);
                default:
                    return "";
            }
        }

        /// <summary>
        /// Encrypts text using columnar transposition cipher
        /// </summary>
        /// <param name="plainText">The text to encrypt (uppercase)</param>
        /// <param name="key">The keyword to determine column order (uppercase)</param>
        /// <returns>Encrypted text (uppercase)</returns>
        protected static string CipherColumn(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(key))
                return plainText;

            int columns = key.Length;
            int rows = (int)Math.Ceiling((double)plainText.Length / columns);

            char[,] grid = new char[rows, columns];

            // Fill the grid row by row
            int index = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (index < plainText.Length)
                    {
                        grid[row, col] = plainText[index];
                        index++;
                    }
                    else
                    {
                        grid[row, col] = '\0'; // Empty cell
                    }
                }
            }

            int[] columnOrder = GetColumnOrder(key);

            // Read columns in the order determined by the key
            StringBuilder cipherText = new StringBuilder();
            for (int orderIndex = 1; orderIndex <= columns; orderIndex++)
            {
                int col = Array.IndexOf(columnOrder, orderIndex);
                for (int row = 0; row < rows; row++)
                {
                    if (grid[row, col] != '\0')
                        cipherText.Append(grid[row, col]);
                }
            }

            return cipherText.ToString();
        }

        /// <summary>
        /// Decrypts text that was encrypted using the columnar transposition cipher
        /// </summary>
        /// <param name="cipherText">The encrypted text to decrypt (uppercase)</param>
        /// <param name="key">The same keyword that was used for encryption (uppercase)</param>
        /// <returns>The original plaintext (uppercase)</returns>
        protected static string DecipherColumn(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText) || string.IsNullOrEmpty(key))
                return cipherText;

            int columns = key.Length;
            int totalChars = cipherText.Length;
            int rows = (int)Math.Ceiling((double)totalChars / columns);

            int fullColumns = totalChars % columns;
            if (fullColumns == 0) fullColumns = columns;

            int[] columnOrder = GetColumnOrder(key);

            int[] columnLengths = new int[columns];
            for (int i = 0; i < columns; i++)
            {
                int originalCol = Array.IndexOf(columnOrder, i + 1);
                columnLengths[originalCol] = (originalCol < fullColumns) ? rows : rows - 1;
            }

            // Fill the grid column by column in the order determined by the key
            char[,] grid = new char[rows, columns];
            int currentPos = 0;

            for (int orderIdx = 1; orderIdx <= columns; orderIdx++)
            {
                int col = Array.IndexOf(columnOrder, orderIdx);
                for (int row = 0; row < columnLengths[col]; row++)
                {
                    if (currentPos < cipherText.Length)
                    {
                        grid[row, col] = cipherText[currentPos];
                        currentPos++;
                    }
                }
            }

            // Read the grid row by row to reconstruct the plaintext
            StringBuilder plainText = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (grid[row, col] != '\0')
                    {
                        plainText.Append(grid[row, col]);
                    }
                }
            }

            return plainText.ToString();
        }



        private const string RussianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        protected static string CipherVisner(string content, string key)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(key))
                return content;

            StringBuilder result = new StringBuilder();
            string extendedKey = key + content;

            for (int i = 0; i < content.Length; i++)
            {
                char currentChar = content[i];
                char keyChar = extendedKey[i];

                int currentPos = RussianAlphabet.IndexOf(currentChar);
                int keyPos = RussianAlphabet.IndexOf(keyChar);

                if (currentPos == -1 || keyPos == -1)
                    throw new ArgumentException($"Invalid character detected: {currentChar} or {keyChar}");

                // Vigenère encryption: (P + K) mod 33
                int encryptedPos = (currentPos + keyPos) % 33;

                result.Append(RussianAlphabet[encryptedPos]);
            }

            return result.ToString();
        }

        protected static string DecipherVisner(string content, string key)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(key))
                return content;

            StringBuilder result = new StringBuilder();
            StringBuilder extendedKeyBuilder = new StringBuilder(key);

            for (int i = 0; i < content.Length; i++)
            {
                char currentChar = content[i];
                char keyChar = extendedKeyBuilder[i];

                int currentPos = RussianAlphabet.IndexOf(currentChar);
                int keyPos = RussianAlphabet.IndexOf(keyChar);

                if (currentPos == -1 || keyPos == -1)
                    throw new ArgumentException($"Invalid character detected: {currentChar} or {keyChar}");

                // Vigenère decryption: (C - K) mod 33
                int decryptedPos = (currentPos - keyPos + 33) % 33;
                char decryptedChar = RussianAlphabet[decryptedPos];

                result.Append(decryptedChar);
                extendedKeyBuilder.Append(decryptedChar);
            }

            return result.ToString();
        }

        /// <summary>
        /// Determines column order based on keyword
        /// </summary>
        /// <param name="keyword">Uppercase keyword</param>
        /// <returns>Array where index is original column position and value is order (1-based)</returns>
        private static int[] GetColumnOrder(string keyword)
        {
            int length = keyword.Length;
            int[] order = new int[length];

            // Create list with original positions
            var charPositions = new List<(char character, int originalPosition)>();

            for (int i = 0; i < length; i++)
            {
                charPositions.Add((keyword[i], i));
            }

            // Sort by character, then by original position
            var sorted = charPositions
                .OrderBy(cp => cp.character)
                .ThenBy(cp => cp.originalPosition)
                .ToList();

            // Assign order numbers (1-based)
            for (int i = 0; i < length; i++)
            {
                order[sorted[i].originalPosition] = i + 1;
            }

            return order;
        }
    }
}