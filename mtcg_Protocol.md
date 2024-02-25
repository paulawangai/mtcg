## Project Protocol: Trading Card Game (MTCG)

## Date: 25/02/2024

## Technical Steps:

1. Design Phase:

During the design phase, my aim was to get as much understanding as possible for what I needed for my project based on the project requirements. The first step was coming up with the classes
I felt I needed based on the information given in the requirements - I decided to create classes that would be the building blocks of my project - those were the model classes,
which I used as a basis to design my database scema to store userdata, cards, decks, battles, etc. I decided to also add other files that would deal with the endpoints, which were defined
in the yaml file - these files would later be my controller classes - this is where the api calls would be executed, lastly i needed separate files for the game logic - which later i named
my Service classes - this is where i for example handled my battle logic. And finally I needed test files where i'd write out my unit tests

2. Implementation Phase:
   - Set up the ASP.NET Core backend project with necessary dependencies.
   - Implemented user authentication and authorization using JWT tokens - created a separate Service file - JwtAuthenticationService.cs to handle this.
   - Created database models based on my Model classes.
   - Implemented CRUD operations for managing users, cards, decks, etc.
   - Implemented the battle logic and service for conducting battles between players.
   - Integrated unit tests for critical components to ensure reliability.

3. Testing Phase:
   - Conducted manual testing to ensure proper functioning of API endpoints - i used my curl script for this: each curl command called a different API Endpoint.
   - Executed unit tests to verify the correctness of critical components such as battle logic.
   - Addressed and resolved any bugs or issues identified during testing.



4. Unit Tests:

I chose the following unit tests for my mtcg project - only 15 are listed in the documentation but they are 20 in total and are contained in my Tests Folder:

1. GetUserCards_ValidUser_ReturnsUserCards:
   - This test verifies that the CardsController correctly retrieves cards for a valid user.
   - Ensures that user cards are fetched from the database and returned as expected.
   - Critical because it ensures that users can access their cards, which is obviously a fundamental feature of the game.

2. GetUserCards_InvalidUser_ReturnsUnauthorized:
   - This test verifies that the CardsController returns an unauthorized status code for an invalid user.
   - Ensures that unauthorized users cannot access user card data, which maintains some level of security in the game .

3. GetUserDeck_UserWithConfiguredDeck_ReturnsOkResult:
   - This test ensures that the DeckController returns a configured deck for a user who has already set up their deck.
   - It is important because it confirms that users can access their configured decks during battles, allowing the battle to take place.

4. ConfigureUserDeck_ValidCardIds_ReturnsOkResult:
   - This test verifies that the DeckController successfully configures a user's deck with valid card IDs.
   - Ensures that users can configure their decks with the desired cards which will be later used for battles.

5. CreatePackage_ValidPackage_ReturnsCreatedResult:
   - This test confirms that the PackagesController creates a new package with valid cards.
   - It makes sure that packages are being created and made available for users to purchase in order to be able to own cards.

6. Login_ValidCredentials_ReturnsOkResultWithToken:
   - This test verifies that the SessionsController returns a valid JWT token for successful user login.
   - Ensures that users can authenticate and receive a token for accessing protected endpoints.

7. GetTradingDeals_ReturnsAvailableDeals:
   - Verifies that the TradingController returns available trading deals.
   - Critical for facilitating card trading between users.

8. PostTradingDeal_ValidDeal_ReturnsCreatedResult:
   - Validates that the TradingController successfully creates a new trading deal.
   - Ensures that users can post new trading deals for card exchange.

9. DeleteTradingDeal_ValidDeal_ReturnsOkResult:
   - Tests the deletion of a trading deal.
   - Critical for maintaining the integrity of the trading system and removing expired or unwanted deals.

10. TradeWithDeal_ValidTrade_ReturnsOkResult:
   - Verifies the successful execution of a trade with a trading deal.
   - Ensures that users can exchange cards based on posted trading deals.

11. PurchasePackage_UserAuthenticatedAndSufficientCoins_ReturnsOkResult:
   - Validates the purchase of a package by a user with sufficient coins.
   - Critical for users to acquire new cards and items using in-game currency.

12. PurchasePackage_UserAuthenticatedAndInsufficientCoins_ReturnsBadRequestResult:
   - Verifies that users with insufficient coins cannot purchase packages.
   - Prevents users from acquiring packages if they dont have money.

13. RegisterUser_ValidRequest_ReturnsOkResult:
   - Tests the registration of a new user with valid credentials.
   - Ensures that new users can successfully register and create accounts .

14. UpdateUserProfile_ValidRequest_ReturnsOkResult:
   - Verifies the successful update of a user's profile information.
   - Allows users to update their profile details such as name and bio.

15. GetUserProfile_ExistingUser_ReturnsOkResultWithUserProfile:
   - Validates that the UserController returns the user profile for an existing user.
   - Ensures that users can view their profile information.


5. Problems encountered:

    - Initially did not create the database before implementing the logic of the game - i created a functional game without using a database initially , the plan was to add the database
      and api endpoint logic later, only to realise that that was the biggest mistake - i had to restart the project implementation from scratch , by first creating a database and working from that.

    - Creating a dynamically generated jwt secret key - this was just a pain to consistently update everytime i restarted my server

    - Migrations - during the implementation phase, i had to add properties to my model classes that i had overlooked or didnt know i needed before implementing the game logic,
      for example giving eack package a constant price, adding an ownerid to cards, adding a configureddeck to each user, i would then have to perform a migration to add this information to my database, which resulted in errors multiple times,
      especially because i had to perform this migrations multiple times for different properties for my model classes

    - Threading to allow a battle between two players - this was difficult to create a solution for especially since the battles endpoint was called twice in a row for the two plyers


6. Lessons Learned

    - Always start with the database!!!
    - In a project like this a dynamically generated Secret key for token generation is not a good idea
    - How to authorize access to Api endpoints using jwt authentication
    - Creating Unit tests that implement mocking 


7. Unique Mandatory Feature

    - Winner is awarded 5 points upon winning a battle: UpdateWinnerCoinBalance(winnerId);

Time Tracking:

The time spent on the project is tracked as follows:

- Design Phase: [20 hours]
- Implementation Phase: [400 hours] 
- Testing Phase: [20 hours]

Total Time Spent: [440 Hours]

