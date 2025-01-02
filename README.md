# 🎥 MovieBinge
> A web application to browse, add, and manage your favorite movies.

## Table of Contents
* [General Info](#general-information)
* [Technologies](#technologies-used)
* [Features](#features)
* [Setup](#setup)
* [Examples](#examples)
* [Project Status](#project-status)
* [Room for Improvement](#room-for-improvement)

## General Information
- 🎞 **MovieBinge** is a web-based platform designed for movie enthusiasts to explore and manage their personal movie collections.
- 💻 Developed as part of [The C# Academy](https://www.thecsharpacademy.com) to learn about ASP.NET Core MVC.
- 🙍🏻‍ Allows users to browse a catalog of movies, add new movies, and manage their library.

## Technologies Used
- ASP.NET Core MVC - for building the web application and managing server-side logic.
- Azurite Local Blob Storage - for local file and media storage.
- Bootstrap - for responsive and modern UI components.
- Entity Framework Core - for data access and ORM.
- SQL Server - for robust and scalable database storage.
- HTML, CSS, Javascript and JQuery

## Features
- Users can browse a catalog of movies with details such as title, genre, release date, and ratings.
- Add new movies to the collection, complete with metadata and optional image uploads.
- Update or delete movie records as needed.

## Setup
1. **Clone the repository**
   ```bash
   git clone https://github.com/Dejmenek/Movies.git
   cd Movies
   ```
2. **Open the project in Visual Studio**  
	- Launch Visual Studio.
	- Open the Movies.Dejmenek.sln solution file from the project directory
3. **Restore dependencies**  
	- In Visual Studio, go to the Tools menu and select NuGet Package Manager > Manage NuGet Packages for Solution.
	- Alternatively, dependencies will be restored automatically when you build the solution. To manually restore NuGet packages: ```dotnet restore```
4. **Update the database**
	- Run the following command to create the database and apply migrations:
		```bash	
	    Update-Database
		```
5. **Build and run the project**  
    - In Visual Studio, press Ctrl + Shift + B to build the project.
    - Then press F5 or click Start to run the project.

## Examples

## Project Status
Project is: _in progress_ 🛠️

## Room for Improvement
- Implement user authentication for personalized movie collections.
- Integrate external movie APIs (e.g., TMDB or IMDb) for automatic data fetching.
- Improve the UI with animations and transitions for a more engaging user experience.
