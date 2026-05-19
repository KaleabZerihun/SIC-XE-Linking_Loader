# SIC/XE Linking Loader

SIC/XE Linking Loader is a C# console application that simulates the linker/loader process used in system software. The program reads one or more object program files, builds an external symbol table, processes text and modification records, relocates addresses, and generates a final memory map.

The project demonstrates how separate object programs can be linked and loaded into memory by resolving control sections, external symbols, and relocation values.

## Project Purpose

I built this project to understand how a linker/loader works as part of system software.

The main goal was to create a separate linker/loader program that can read object code records, calculate control section addresses, build an external symbol table, apply modification records, and display the final loaded memory layout.

## Features

- Reads one or more object program files
- Processes header records
- Processes define records
- Processes text records
- Processes modification records
- Builds an External Symbol Table, also known as ESTAB
- Tracks control sections
- Calculates control section addresses
- Resolves external symbols
- Applies address relocation
- Generates a formatted memory map
- Writes final memory output to `MEMORY.DAT`
- Console-based output
- Object-oriented C# design

## How It Works

The program accepts object program file names from the command line or asks the user to enter them.

It then performs the main linker/loader steps:

1. Reads each object program file
2. Builds the ESTAB table using control section and symbol information
3. Stores text records for memory loading
4. Stores modification records for relocation
5. Resolves symbol addresses using ESTAB
6. Applies positive or negative address modifications
7. Prints the final memory layout
8. Saves the memory map to `MEMORY.DAT`

## Technologies Used

- C#
- .NET
- Console Application
- File I/O
- Object-Oriented Programming
- System Software Concepts
- Linker/Loader Design
- External Symbol Table
- SIC/XE Assembly Concepts
