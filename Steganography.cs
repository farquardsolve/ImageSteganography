using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageSteganography
{
    public class Steganography
    {
        public static Bitmap EmbedMessage(Bitmap image, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            int messageLength = messageBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(messageLength);
            byte[] dataToEmbed = new byte[lengthBytes.Length + messageBytes.Length];

            Buffer.BlockCopy(lengthBytes, 0, dataToEmbed, 0, lengthBytes.Length);
            Buffer.BlockCopy(messageBytes, 0, dataToEmbed, lengthBytes.Length, messageBytes.Length);

            if (dataToEmbed.Length * 8 > image.Width * image.Height * 3)
                throw new Exception("Message too large to hide in the image.");

            Bitmap embeddedImage = new Bitmap(image);
            int dataIndex = 0, bitIndex = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int r = pixel.R, g = pixel.G, b = pixel.B;

                    if (dataIndex < dataToEmbed.Length)
                    {
                        r = (r & 0xFE) | ((dataToEmbed[dataIndex] >> (7 - bitIndex)) & 1);
                        bitIndex++;
                        if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    }
                    if (dataIndex < dataToEmbed.Length)
                    {
                        g = (g & 0xFE) | ((dataToEmbed[dataIndex] >> (7 - bitIndex)) & 1);
                        bitIndex++;
                        if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    }
                    if (dataIndex < dataToEmbed.Length)
                    {
                        b = (b & 0xFE) | ((dataToEmbed[dataIndex] >> (7 - bitIndex)) & 1);
                        bitIndex++;
                        if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    }

                    embeddedImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return embeddedImage;
        }

        public static string ExtractMessage(Bitmap image)
        {
            byte[] extractedBytes = new byte[4];
            int dataIndex = 0, bitIndex = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);

                    extractedBytes[dataIndex] |= (byte)((pixel.R & 1) << (7 - bitIndex));
                    bitIndex++;
                    if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    if (dataIndex == 4) break;

                    extractedBytes[dataIndex] |= (byte)((pixel.G & 1) << (7 - bitIndex));
                    bitIndex++;
                    if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    if (dataIndex == 4) break;

                    extractedBytes[dataIndex] |= (byte)((pixel.B & 1) << (7 - bitIndex));
                    bitIndex++;
                    if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    if (dataIndex == 4) break;
                }
                if (dataIndex == 4) break;
            }

            int messageLength = BitConverter.ToInt32(extractedBytes, 0);
            byte[] messageBytes = new byte[messageLength];
            dataIndex = 0; bitIndex = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);

                    if (dataIndex < messageBytes.Length)
                    {
                        messageBytes[dataIndex] |= (byte)((pixel.R & 1) << (7 - bitIndex));
                        bitIndex++;
                        if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    }
                    if (dataIndex < messageBytes.Length)
                    {
                        messageBytes[dataIndex] |= (byte)((pixel.G & 1) << (7 - bitIndex));
                        bitIndex++;
                        if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    }
                    if (dataIndex < messageBytes.Length)
                    {
                        messageBytes[dataIndex] |= (byte)((pixel.B & 1) << (7 - bitIndex));
                        bitIndex++;
                        if (bitIndex == 8) { bitIndex = 0; dataIndex++; }
                    }
                }
            }

            return Encoding.UTF8.GetString(messageBytes);
        }
    }
}
