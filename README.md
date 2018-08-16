# GitBlog

GitDriven Blog Engine. Blog Engine is all about your content. Using just your content, GitBlog gives you the ability to focus on your content using an easy to use markdown language.
  
## Getting Started  

I don't commit my ```appsettings.json``` or my ```appsettings.Development.json```. You will need to create these in the root of the project. Look at the ```appsettings.example.json``` to know exactly what you need.  
  
## What it does

GitBlogEngine looks at a Git repo with some markdown files in them, converts the html into a usable format and delivers that data via rest api. you can then render that data on your front end DOM objects. Is it pretty, nope, but it works...sort of. You can just use your GitHub credentials for now until I implement token based auth.

## Exploring

Swagger is installed so you can explore the api and what params you need.
  
## Running the code  

Run ```dotnet restore``` and then ```dotnet run``` and the app should work when you navigate to localhost:5000/. I think...depends on your .vscode
