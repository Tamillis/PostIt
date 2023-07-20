# PostIt
A .NET Entity Framework using Web App demonstration.

A very simple twitter clone where users can post short message with a title and a body, see everyone's posts organised by relevance (a simple recency + likes formula) and leave comment posts, i.e. posts that are anchored to some other post Id, which are displayed in a post's details web page.


## Current State of the Project
Sprint 1 is complete as of this push. Ready to start sprint 2, which doubles as a demonstration of the entire sprint process. There will be links to a kanban board, User Stories, Figma design board, a metro retrospective and a redone README suitable for the project. The README will contain an example of a User Story, its breakdown into SpecFlow Unit Tests, the result of those tests (passing) and then a short section on the code that makes it pass, all together functioning as a demonstration of TDD.

## Current Goal
Complete the initial basic project (purely focusing on back-end functionality), in order to then enter a "second sprint" as a demonstration of a proper Agile-Scrum processes with a Kanban board (Found Here: PROVIDE LINK) and the goal of making the project testable with a service layer, so will be employing a Test-First Dev approach: User Stories -> Tests -> Make Tests Pass.

## Feature List
Simple Cookie-based Authentication system, usernames (Author "Handles") and passwords (hashed) stored on the SQL database directly.

Ability to Register, Sign In and Sign Out.

Ability to view all posts and single posts via ID.

Ability to make posts anonymously if not logged in

Can Edit and Delete your posts, but not others'

Can make posts when viewing a post, making that post a reply (and see other replies)

## TODO

Fixup front end (2nd sprint)

Validation - move input validation from jquery to custom onchange() with better feedback, and also do validation on the server-side (2nd sprint)

Completely remove Bootstrap and use something else, like Pico with some custom css classes. (2nd / 3rd Sprint)

Add a service layer to enable testing of backend functionality (2nd sprint)

Publish the site online via Azure. (2nd sprint)

Add an Author profile page (3rd sprint)
- including a way to see all your authored posts, most likely just as a filter on the main view

Remove the /Edit path and make editing in-line* (push feature)

Pop-up warning to delete?* (push feature)