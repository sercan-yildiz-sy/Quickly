<h1 align="center">Quickly a Kitchen Inventory App</h1>


Quickly is an application that offers quality time for housekeepers by assisting with kitchen inventory management. 
By identifying that the lack of organisation in the house inventory leads to higher food and money waste, as well as fatigue and confusion, the main goal with this first iteration is to organise and update the pantry, fridge, and freezer.

## Demo

[![Watch the video](https://img.youtube.com/vi/dak9Aag-XDw/hqdefault.jpg)](https://youtu.be/dak9Aag-XDw)


For this project, I used .NET 9.0 and SQLite for backend data storage, building the application as a cross-platform .NET MAUI app.

## Features

- Organize and update pantry, fridge, and freezer inventories
- Add, edit, and delete inventory items
- Categorize items by type and location
- Store images and quantities for each item
- Cross-platform support (Android, iOS, Windows, MacCatalyst)

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 (with .NET MAUI workload installed)

### Building and Running

1. Clone the repository: 
	git clone https://github.com/sercan-yildiz-sy/quickly.git 
	cd quickly
2. Open the solution in Visual Studio 2022.
3. Restore NuGet packages.
4. Select your target platform (Android, iOS, Windows, or MacCatalyst).
5. Build and run the application.

## Usage

- Use the app to add new items to your kitchen inventory.
- Update quantities and locations as you use or move items.
- Remove items when they are no longer in your inventory.

## Data Model

The inventory items in Quickly are represented with the following properties:

| Property Name   | Type     |
|----------------|----------|
| Id             | int      |
| ItemId         | int      |
| Name           | string   |
| Image          | string   |
| Quantity       | float    |
| Quantity Type  | string   |
| Category       | string   |
| Location       | string   |


## Technologies Used

- .NET 9.0
- .NET MAUI
- sqlite-net-pcl
- SQLitePCLRaw.bundle_green
- CommunityToolkit.Mvvm
- Syncfusion.Maui.Toolkit
