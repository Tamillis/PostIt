# PostIt
A .NET, Entity Framework, SpecFlow, MVC Web App demonstration.

<img src="./PostItDemo/wwwroot/Assets//PostItsLogo.png" style="float:right;top:0px;" width="200px"/>

A very simple twitter clone where users can register & log in, post short messages with a title and a body, see everyone's posts organised by relevance (a simple recency + likes formula) and leave comment posts, i.e. posts that are anchored to some other post Id, which are displayed in a post's details web page.

## Feature List
Simple Cookie-based Authentication system, usernames (Author "Handles") and passwords (hashed) stored on the SQL database directly.

- Cookie-based Registration, with and Sign In and Sign Out.
- Ability to make posts anonymously if not logged in
- A main view where all posts can be seen
- A per-post view where comment posts can be left, other comment posts seen
- Can Edit and Delete your own posts, but not others'

## Current State of the Project
Sprint 1 is complete as of this push. Ready to start sprint 2, which doubles as a demonstration of the entire sprint process. There will be links to a kanban board, User Stories, Figma design board, a metro retrospective and a redone README suitable for the project. The README will contain an example of a User Story, its breakdown into SpecFlow Unit Tests, the result of those tests (passing) and then a short section on the code that makes it pass, all together functioning as a demonstration of TDD.

## QA Demonstration
A quick demonstration of Unit testing can be found in the code [here](./PostItsTests/UtilsTests.cs)

## User Journey Demonstration

- User Story (Kanban board screenshot)
- Tests (Unit testing project code screenshot)
- Implementation (Code implementation screenshot)
- Product (website screenshot/s)

## KANBAN Board

https://github.com/users/Tamillis/projects/2

## Figma Design
# [Home Page Design](https://www.figma.com/file/1JO25scDlwgyS5pMxvU0Cb/PostIts-Homepage?type=design&node-id=0%3A1&mode=design&t=wMqsCHv867fsVw1f-1)

I am not a designer, but this will do as a demonstration of "this is what I want you to make", and then the real site being how close I can get to that.

## Metroretro Retrospective
LINK

## Installation

Under releases, download the provided .zip, unzip and, using Visual Studio (with ASP.NET and web development & SQL Server installed), open the .sln, click run.

For the latest development iteration: clone the repo and load the .sln with Visual Studio (with ASP.NET and web development & SQL Server installed), and click run.