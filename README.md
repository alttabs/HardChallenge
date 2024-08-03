# Solution

## Overview

This project includes various components such as data access, file system abstraction, and automated tests to ensure the correctness and performance of the system.

## Architecture

### Project Structure

- **SmartVault.Program**: The main application responsible for handling business logic and interactions between various components.
- **SmartVault.DataGeneration**: Contains the logic for generating test data, including the `AsyncDataSeeder` class that uses asynchronous operations for efficient data generation.
- **SmartVault.Library**: Contains common library code and business object definitions.
- **SmartVault.Tests**: Contains unit tests to verify the functionality and performance of the application.

### Key Components

- **IDataAccess**: Interface defining methods for accessing data from the database.
- **IFileSystem**: Interface from `System.IO.Abstractions` used for file system operations, allowing for easier testing and mocking.
- **IDataSeeder**: Interface for data seeding operations, implemented by `AsyncDataSeeder`.
- **ProgramService**: Provides methods for performing operations such as calculating total file sizes and writing specific content to files.

## Async Data Generation

### Performance Improvement with Async

The `AsyncDataSeeder` class improves the performance of data generation by using asynchronous methods. Here's how it helps:

1. **Concurrent Operations**: By using `ConcurrentBag<T>` for collecting users, accounts, and documents, the class allows multiple threads to add items concurrently without blocking each other.
2. **Asynchronous Database Operations**: The use of asynchronous methods (e.g., `InsertUsers`, `InsertAccounts`, `InsertDocuments`) ensures that database operations do not block the main thread, allowing other operations to proceed concurrently.
3. **Reduced Latency**: Async methods can handle more operations per unit of time by efficiently utilizing I/O-bound resources, such as disk and network.


## Tests

### Test Overview

The tests in the `SmartVault.Tests` project verify the correctness and performance of the system. Here are some key tests:

1. **SeedDataAsync_ShouldInsertData**: Verifies that the `AsyncDataSeeder` correctly inserts users, accounts, and documents into the database.
2. **GetAllFileSizesAsync_ShouldCalculateCorrectTotalSize**: Verifies that the `ProgramService` correctly calculates the total size of all files, ensuring that the file sizes match the expected values.

### Running Tests

To run the tests, navigate to the `SmartVault.Tests` project directory and execute the following command:

```sh
dotnet test
```

This command will run all the tests and display the results, indicating which tests passed and which failed.

## Conclusion

This project demonstrates efficient data generation using asynchronous operations and thorough testing to ensure the correctness and performance of the system. The use of interfaces and mocking in tests allows for flexible and maintainable code, making it easier to adapt and extend the system in the future.

# Overview

The point of this brief exercise is to help us better understand your ability to work through problems, design solutions, and work in an existing codebase. It's important that the solution you provide meets all the requirements, demonstrates clean code, and is scalable.

# Code

There are 3 projects in this solution:

## SmartVault.CodeGeneration

This project is used to generate code that is used in the SmartVault.Program library.

## SmartVault.DataGeneration

This project is used to create a test sqlite database.

## SmartVault.Program

This project will be used to fulfill some of the requirements that require output.

# Requirements

1. Speed up the execution of the SmartVault.DataGeneration tool. Developers have complained that this takes a long time to create a test database.

2. All business objects should have a created on date.

3. Implement a way to output the contents of every third file of an account to a single file, if the file contains the text "Smith Property".

4. Implement a way to get the total file size of all files, get the file size from the actual file as the database may be out of sync with the actual size.

5. Add a new business object to support OAuth integrations (No need to implement an actual OAuth integration, just the boilerplate necessary in the given application)

6. Commit your code to a github repository and share the link back with us

# Guidelines

- There should be at least one test project

- This project uses [Source Generators](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) and should be run in Visual Studio 2022

- You may create any additional projects to support your application, including test projects.

- Use good judgement and keep things as simple as necessary, but do make sure the submission does not feel unfinished or thrown together

- This should take 2-4 hours to complete
