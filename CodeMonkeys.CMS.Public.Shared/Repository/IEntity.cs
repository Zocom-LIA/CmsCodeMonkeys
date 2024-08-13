//
// Class: RepositoryBase
//
// Description:
// The RepositoryBase class is a base class for repositories that interact with a database context. It provides a set of methods for querying, adding, updating, and deleting entities in the database, as well as handling exceptions that occur during data access operations. The class is designed to be generic and flexible, allowing it to work with any entity type.
//
// Purpose:
// The purpose of the RepositoryBase class is to provide a common set of methods for working with a database context in a consistent and reusable way. By defining a standard set of operations for managing entities, developers can write code that is more modular, flexible, and maintainable, leading to higher-quality software. The class also helps to decouple the data access logic from the rest of the application, making it easier to test and refactor the code in the future.
//
// Usage:
// The RepositoryBase class is typically used as a base class for concrete repository implementations that interact with a specific database context. Developers can create concrete repository classes that inherit from the RepositoryBase class, providing a consistent and reusable way to interact with the database. By using the repository pattern, developers can write code that is more modular, flexible, and maintainable, leading to a more robust and scalable application.
//
// MIT License
//
// Copyright (c) 2024 Ulf Bourelius
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IEntity
    {
        object GetIdentifier();
    }
}