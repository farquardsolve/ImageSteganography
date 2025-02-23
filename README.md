# ImageSteganography

## Overview
**ImageSteganography** is a C# library that enables embedding and extracting hidden messages within images using **Least Significant Bit (LSB) Steganography**. It is designed for secure communication and digital watermarking.

## Features
- Embed text messages inside **PNG** and **BMP** images.
- Extract hidden messages from encoded images.
- Lightweight and easy to use.

## Installation
### Using NuGet
You can install the package from NuGet:
```sh
 dotnet add package ImageSteganography
```

### Manual Installation
1. Download the latest release from [GitHub Releases](https://github.com/farquardsolve/ImageSteganography).
2. Add a reference to the **ImageSteganography.dll** in your project.

## Usage
### Embedding a Message
```csharp
using System.Drawing;
using ImageSteganography;

class Program
{
    static void Main()
    {
        Bitmap image = new Bitmap("input.png");
        string secretMessage = "Hello, this is a hidden message!";
        
        Bitmap encodedImage = SteganographyHelper.EmbedMessage(image, secretMessage);
        encodedImage.Save("encoded.png");
    }
}
```

### Extracting a Message
```csharp
using System.Drawing;
using ImageSteganography;

class Program
{
    static void Main()
    {
        Bitmap loadedImage = new Bitmap("encoded.png");
        string extractedMessage = SteganographyHelper.ExtractMessage(loadedImage);
        Console.WriteLine($"Extracted Message: {extractedMessage}");
    }
}
```

## Contributing
1. Fork the repository.
2. Create a new branch (`feature-branch`).
3. Commit your changes.
4. Push the branch and create a pull request.

## License
This project is licensed under the MIT License.

## Author
**Segun O. Fakuade**

---
© 2025 **Segun O. Fakuade**. All Rights Reserved.
