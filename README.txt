************************************
**			Prerequisites		  **
************************************
1. Install .NET Core SDK (.NET 8)
2. Install node.js (v22.14.0)
3. Install npm (11.2.0)
4. Install angular cli (19.2.5)
5. Install SQL Server (2022)

************************************
**			Setup Project		  **
************************************
1. Clone the project from the repository. Use master branch. https://github.com/tatunka/TroyLibrary
2. Open the project in Visual Studio 2022.
3. Check appsettings.Development.json in the TroyLibrary.API project and fill in apropriate values. Specifically, ensure 
	the ConnectionStrings, Cors, and Jwt settings are correct. These may not have to be changed depending on your environment setup.
	The other settings can be changed as needed. The 'MinutesUntilOverdue' setting will change the due dates of any books check
	out in the future. This is useful for testing.
4. Check the environment variables in the Angular project. Nothing should need to be changed here, but in in case the API url changes,
	this is where it can be updated.

************************************
**		Running the Project		  **
************************************
1. Launch the TroyLibrary.API project from Visual Studio. This will launch the API and additonally create and seed the database 
	with test data. You should see a new browser window launch with the Swagger API page.
2. Navigate to the Angular project under TroyLibrary.
3. Run `npm install` on the Angular project located in the TroyLibrary project to install all of the npm packages.
4. Run `ng serve` to start the Angular project. 
5. In your browser, navigate to http://localhost:60659 to view the application.

************************************
**			Notes				  **
************************************
1. This project uses Identity Framework and JWT tokens for authentication. The API is secured and requires a valid token to access.
	in order to do anything in the library, the user must be authenticated.
2. An overdue flag will appear on the book list screen and book detail screen for books that are overdue.
3. Only customers can check out books. Only librarians can return books, edit books, create books, delete books, and see overdue books.
4. Searching '*' will return all books.
5. Sorting and filtering is only performed on the current page of books.
6. Books can be filtered by multiple categories and multiple values within categories. For instance, you can filter by multiple authors
	and multiple titles.
6. Filtering is additive within a category, but not between categories. For example, if you filter Title by 'Chocolate Rain' and
	'Double Sunday' and then filter Author by 'JRR Tolkien', you will only see books written by JRR Tolkien titled 'Chocolate Rain' or
	'Double Sunday'. You will not see books written by other authors with those titles. In other words, filtering within a category is
	an OR, but filtering between categories is an AND.
7. Sorting is not cascading.
8. Librarians can edit and delete books from the book detail screen. They can also view due dates here.
9. Librarians can add books by clicking the 'Add Book' button on the book list screen. This button only appears for librarians.

************************************
**		 Useful Commands		  **
************************************
New Migration:
dotnet ef migrations add <migration name> --project TroyLibrary.Data --startup-project TroyLibrary.API --context TroyLibraryContext

Apply Migration:
dotnet ef database update <migration name> --project TroyLibrary.Data --startup-project TroyLibrary.API --context TroyLibraryContext