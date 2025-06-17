# AutoEntityGenerator

**AutoEntityGenerator**  is a Visual Studio extension that simplifies the process of generating Data Transfer Objects (DTOs) and mapping extensions based on existing domain entity classes. 
This extension helps developers quickly create and maintain supporting classes, enhancing productivity and reducing manual coding errors.

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

## 
For more information, visit the [ReadMe](https://github.com/Peled-Zohar/AutoEntityGenerator/blob/main/README.md) file on the **AutoEntityGenerator** GitHub repository
## 
**AutoEntityGenerator** is completely open-source and licensed under the [MIT License](https://github.com/Peled-Zohar/AutoEntityGenerator/blob/main/AutoEntityGenerator.Manifest/LICENSE.txt).

##
Please consider rating **AutoEntityGenerator** or leaving a review to help improve it!
Your feedback is valuable, whether it’s positive or critical, and will help shape future updates.
##
Changes from previous version:

1 **Updated behavior of the entity configuration form.**  
In this version, when you choose a value in the mapping direction drop-down, it affects the generated DTO's name unless you manually change the value of the textbox.  
Default values are: `<entityName>Request` for mapping from DTO to Model,   
and `<entityName>Response` for mapping from Model to DTO.

2 **New configuration settings window.**  
Users can now control various configuration settings. Current configurable values are:

- Minimum log level (default: "Information")
- Destination folder (default: "Generated")
- Request suffix (default: "Request")
- Response suffix (default: "Response")

## How to use

### Open the light bulb menu on your domain model

![1._DomainModel.jpg](1._DomainModel.jpg)

### Click the "🔧 Generate DTO and mapping 🛠️" menu item

![2._LightBulb.jpg](2._LightBulb.jpg)

### Configure the generated entity and mapping direction

![3._EntityConfigurationMenu__1.jpg](3._EntityConfigurationMenu__1.jpg)

### To change settings, click on the settings button (small blue gear in the top-right corner of entity configuration window)

![4_Settings_Window.jpg](4_Settings_Window.jpg)

## Results

### Generated entity

![5._GeneratedEntity.jpg](5._GeneratedEntity.jpg)

### Generated mapping extension

![6._GeneratedMappingExtension.jpg](6._GeneratedMappingExtension.jpg)