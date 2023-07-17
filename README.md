# PostIt
A .NET Entity Framework using Web App demonstration.

A very simple twitter clone where users can post short message with a title and a body, see everyone's posts organised by relevance (a simple recency + likes formula) and leave comment posts, i.e. posts that are anchored to some other post Id, which are displayed in a post's details web page.

## Current Goal
Complete the initial basic project (purely focusing on back-end functionality), in order to then enter a "second sprint" as a demonstartion of a proper Agile-Scrum processes with a Kanban board (Found Here: PROVIDE LINK) and the goal of making the project testable with a service layer, so will be employing a Test-First Dev approach: User Stories -> Tests -> Make Tests Pass.

## Feature List
Simple Cookie-based Authentication system, usernames (Author "Handles") and passwords (hashed) stored on the database directly.

Ability to Register, Sign In and Sign Out.

Ability to view all posts.

Ability to make posts anonymously if not logged in

Can Edit and Delete your posts, but not others'

## TODO

Remove the /Edit path and make editing in-line* (push feature)

Pop-up warning to delete?* (push feature)

Fixup front end (2nd sprint)

Validation - move input validation from jquery to custom onchange() with better feedback, and also do validation on the server-side (2nd sprint)

Add a service layer to enable testing of backend functionality (2nd sprint)

Add comments to posts (initial sprint):
 - where each comment is just a post. 
 - Filter all posts that are not comments from main PostIt view. 
 - Add a "X Post Chains" message to note how many other posts area attached to this post.
 - Add a ~/id path too look at individual post chains (the root post and all other posts that have that post as its mother post)

Add like system to posts, where each user can like a post only once and order by liked-ness and recency (initial sprint)

Add an Author profile page (3rd sprint)
- including a way to see all your authored posts, most likely just as a filter on the main view