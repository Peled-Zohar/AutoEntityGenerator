<div align="center">
  <img  src="/AutoEntityGenerator.Manifest/AutoEntityGenerator.png" />

# AutoEntityGenerator


[![Downloads](https://img.shields.io/visual-studio-marketplace/d/zoharpeled.AutoEntityGenerator)](https://marketplace.visualstudio.com/items?itemName=ZoharPeled.AutoEntityGenerator)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)  
[![Build Status](https://github.com/Peled-Zohar/AutoEntityGenerator/actions/workflows/BuildAndTest.yml/badge.svg)](https://github.com/Peled-Zohar/AutoEntityGenerator/actions/workflows/BuildAndTest.yml/badge.svg) 
[![codecov](https://codecov.io/gh/Peled-Zohar/AutoEntityGenerator/graph/badge.svg?token=H3FNKNP6EL)](https://codecov.io/gh/Peled-Zohar/AutoEntityGenerator)  

</div>

**AutoEntityGenerator** is a Visual Studio extension that simplifies the process of generating Data Transfer Objects (DTOs) and mapping extensions based on existing domain entity classes.
This extension helps developers quickly create and maintain supporting classes, enhancing productivity and reducing manual coding errors.
**AutoEntityGenerator** logs to event log, meaning you can view the logs using windows Event Viewer, under "Windows logs" -> "Application" with the source "AutoEntityGenerator".

# Quick links

- [Features](#features)
- [User feedback](#we-love-users-feedback)
- [Licence](#license)
- [Installation](#installation)
- [Target platforms](#target-platforms)
- [Usage](#usage)
- [Configuration](#configuration)
- [Want to Contribute?](#want-to-contribute)
- [Thanks!](#thank-you)

## Features

- Automatically generates DTOs from domain entity classes or records.
- Creates mapping extension methods to convert between domain entities and DTOs.
- Supports generating code in the same folder or a new folder.
- User-friendly UI for configuring generation options.
- Seamless integration with Visual Studio.
- Supports all kinds of DTOs.
- Generates DTOs as records when the C# version allows it, or as classes for older versions.
- All generated classes are partial, allowing users to add custom code while still maintaining the ability to re-generate if needed.
- Supports generic types and generic constraints.


## We love users Feedback!

If you have any feedback of any kind, we would love to know about it!  
Please [open a new discussion or join an existing one in the feedback category](https://github.com/Peled-Zohar/AutoEntityGenerator/discussions/categories/user-feedback).

If encounter any bugs or have feature requests, 
please join the [Suggested Features discussion]([https://github.com/Peled-Zohar/AutoEntityGenerator/discussions](https://github.com/Peled-Zohar/AutoEntityGenerator/discussions/16)) on the GitHub repository.

## Note

Currently, only types with parameterless constructors are supported for mapping generation.  
If the model doesn't have a parameterless constructor, the generated mapping extension can still be generated but it will not compile without manual changes.  
In future versions, I'm considering to support entities without parameterless constructors by allowing users to include the properties that correspond to the constructor parameters.  

## License

This project is licensed under the MIT License - see the [LICENSE](/AutoEntityGenerator.Manifest/LICENSE.txt) file for details.

## Installation

#### &nbsp;&nbsp; From Visual Studio Marketplace

&nbsp;&nbsp; You can install the **AutoEntityGenerator** extension directly from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=ZoharPeled.AutoEntityGenerator).  
&nbsp;&nbsp; Simply navigate to the extension's page and click on "Install" to add it to your Visual Studio environment.

#### &nbsp;&nbsp; Manual Installation

&nbsp;&nbsp; Alternatively, you can install the extension by downloading the `.vsix` file from the latest [release](https://github.com/Peled-Zohar/AutoEntityGenerator/releases).  
&nbsp;&nbsp; Once downloaded, double-click the `.vsix` file and follow the instructions in the VSIX installer to complete the installation.

## Target platforms

Supported targets are visual studio 2022 - Community, Professinal and Enterprise editions.  

## Usage

1. Open your project in Visual Studio.
2. Open the file containing your domain entity.
3. Press <kbd>ctrl</kbd><kbd>.</kbd> or <kbd>alt</kbd><kbd>enter</kbd> to open the light bulb / screwdriver menu on the class (or record) declaration of the domain entity you want to generate DTOs for.
5. Select <kbd>🔧 Generate DTO and mapping 🛠️</kbd> from the context menu.
6. Configure the generation options in the UI dialog that appears.
7. Click <kbd>OK</kbd> to generate the DTOs and mapping extensions.

## Configuration

**AutoEntityGenerator** allows you to configure various aspects of the code generation process, including:

- The target folder (and namespace) for the generated classes.
- The properties required in the generated DTO.
- The generated DTO name
- The generated file name
- The generated mapping direction (from DTO to Model or from Model to DTO)

## Default condiguration

**AutoEntityGenerator** allows you to configure some default values for the extension:

- The minimum log level
- The default destination folder
- The default suffix for request and response DTOs

## Example

Suppose you have a domain entity class `Product`:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```
Using AutoEntityGenerator, you can generate the following Request DTO and mapping method with just a few mouse clicks:

```csharp
public partial class CreateProductRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public static partial class CreateProductRequestDtoMappingExtensions
{
    public static Product ToProduct(this CreateProductRequest request)
    {
        return new Product
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price
        };
    }
}
```


## Want to Contribute?

We'd love your help!

Whether it's:
- Trying the extension and giving feedback
- Suggesting a feature
- Fixing a bug
- Improving tests or docs

Start here 👉 [How to Contribute](https://github.com/Peled-Zohar/AutoEntityGenerator/blob/main/.github/CONTRIBUTING.md)  
or check out [open issues](https://github.com/Peled-Zohar/AutoEntityGenerator/issues) and [good first issues](https://github.com/Peled-Zohar/AutoEntityGenerator/issues?q=is%3Aopen+is%3Aissue+label%3A%22good+first+issue%22)

We appreciate every bit of help 💙

## Thank you!

Thank you for using AutoEntityGenerator! We hope it enhances your development experience by automating the creation of DTOs and mapping methods.
