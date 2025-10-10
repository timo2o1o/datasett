# DataSett - Schema Extraction and Transformation Toolkit

<div align="center">
  <img src="badger-workbench-icon.png" alt="DataSett Workbench" width="200" height="200">
</div>

## Overview

**DataSett** (Schema Extraction and Transformation Toolkit) is a comprehensive project that assembles a collection of small, focused tools designed to collect, edit, and utilize metadata for creating robust ETL (Extract, Transform, Load) processes.

## What is DataSett?

DataSett provides a unified workbench for data professionals who need to:

- **Extract** schema information from various data sources
- **Transform** and standardize metadata across different systems
- **Utilize** collected metadata to build efficient ETL pipelines
- **Manage** data transformations through an intuitive interface

## Key Features

- 🔍 **Schema Discovery**: Automatically extract and analyze database schemas, file structures, and API definitions
- 🔧 **Metadata Management**: Centralized repository for all your data source metadata
- 🚀 **ETL Pipeline Generation**: Transform metadata into actionable ETL processes
- 🎨 **Visual Workbench**: User-friendly interface built with Blazor for seamless workflow management
- 🔄 **Cross-Platform Compatibility**: Built on .NET 8 for Windows, macOS, and Linux support

## Project Structure

The DataSett solution is organized as follows:

- **DataSettMetamodel**: A class library that defines the metadata model used within the toolkit. [Originally developed during my tenure at Dörffler & Partner GmbH](https://github.com/doerffler/MetadataModel), this version has been adapted and refined for the DataSett project.
- **DataSettViewModel**: A standalone class library implementing the MVVM pattern for the metadata model. This library provides ViewModels that wrap the model entities with property change notification and can be used independently in any .NET application.
- **DataSettWorkbench**: The main Blazor Server application that serves as the user interface. Originally conceived as a WPF application named [MetaDataAdmin](https://github.com/doerffler/MetadataAdminTool) during my time at Dörffler & Partner GmbH, it has been reimagined and modernized for the web using Blazor WebAssembly with the MVVM pattern.
- **DataSettWorkbench.Client**: Client-side Blazor WebAssembly components implementing interactive UI features for metadata management

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- A modern web browser

### Running the Application

1. Clone the repository
2. Navigate to the `DataSettSln` directory
3. Run the following commands:

```bash
dotnet restore
dotnet build
dotnet run --project DataSettWorkbench/DataSettWorkbench
```

4. Open your browser and navigate to the displayed local URL (typically `https://localhost:5001`)

## Contributing

We welcome contributions to DataSett! Whether you're fixing bugs, adding new features, or improving documentation, your help is appreciated.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

---

*The badger workbench icon represents the industrious and methodical approach DataSett takes to data management - building robust solutions one tool at a time.*