# Service Description  

<h2 id="table-of-contents">Table Of Contents</h2>  
1. <a href="#table-of-contents" target="_self">Table Of Contents</a> <br>
2. <a href="#account" target="_self">Account</a> <br>
3. <a href="#admin" target="_self">Admin</a> <br>
4. <a href="#charity" target="_self">Charity</a> <br>
5. <a href="#hirer" target="_self">Hirer</a> <br>
6. <a href="#player" target="_self">Player</a> <br>
7. <a href="#order" target="_self">Order</a> <br>
8. <a href="#game-type" target="_self">Game Type</a> <br>
9. <a href="#types-of-game" target="_self">Types Of Game</a> <br>
10. <a href="#game" target="_self">Game</a> <br>
11. <a href="#rank" target="_self">Rank</a> <br>
12. <a href="#games-of-player" target="_self">Games Of Player</a> <br>
13. <a href="#music" target="_self">Music</a> <br>
14. <a href="#musics-of-player" target="_self">Music Of Player</a> <br>
15. <a href="#image" target="_self">Image</a> <br>
16. <a href="#rating" target="_self">Rating</a> <br>
17. <a href="#report" target="_self">Report</a> <br>
18. <a href="#notification" target="_self">Notification</a> <br>
19. <a href="#chat" target="_self">Chat</a> <br>
20. <a href="#api" target="_self">PlayTogether API</a> <br>


<h2 id="account">Account <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``POST /api/play-together/v1/accounts/login ``  
>   ***Description***: Login all User Account
>
> - ``GET /api/play-together/v1/accounts/check-exist-email ``  
>   ***Description***: Check Exist Email
>
> - ``POST /api/play-together/v1/accounts/login-google-player ``  
>   ***Description***: Sign in with Google for PDlayer
>
> - ``POST /api/play-together/v1/accounts/login-google-hirer ``  
>   ***Description***: Sign in with Google for Hirer
>
> - ``POST /api/play-together/v1/accounts/register-admin ``  
>   ***Description***: Register Admin  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/accounts/register-charity ``  
>   ***Description***: Register Charity  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/accounts/register-player ``  
>   ***Description***: Register a Normal Player Account  
>
> - ``POST /api/play-together/v1/accounts/register-hirer ``  
>   ***Description***: Register a Normal Hirer Account  
>
> - ``PUT /api/play-together/v1/accounts/logout ``  
>   ***Description***: Logout Hirer or Player  
>   ***Role Access***: Hirer, Player    
>
> - ``POST /api/play-together/v1/accounts/register-multi-player ``  
>   ***Description***: Register multi Normal Player Account **for TEST**  
>
> - ``POST /api/play-together/v1/accounts/register-multi-hirer ``  
>   ***Description***: Register multi Normal Hirer Account **for TEST**  
>

<h2 id="admin">Admin <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``GET /api/play-together/v1/admins ``  
>   ***Description***: Get all Admins   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name  
>
> - ``PUT /api/play-together/v1/admins/hirer-status/{hirerId} ``  
>   ***Description***: Active or Disable (1 day) a hirer account   
>   ***Role Access***: Admin    
>
> - ``GET /api/play-together/v1/admins/{userId}/orders ``  
>   ***Description***: Get all Orders of a specific user (Hirer and Player)   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Filter by Status, from day to day  
>
> - ``GET /api/play-together/v1/admins/users/orders/{orderId} ``  
>   ***Description***: Get a order in detail   
>   ***Role Access***: Admin  
>   ***Use for***: View more detail information of the order   
>
> - ``GET /api/play-together/v1/admins/reports ``  
>   ***Description***: Get all reports of all players   
>   ***Role Access***: Admin  
>   ***Use for***: Admin manage reports 
>
> - ``GET /api/play-together/v1/admins/reports/{reportId} ``  
>   ***Description***: Get a report in detail by report Id   
>   ***Role Access***: Admin  
>   ***Use for***: Admin view more detail to know the reason why player make this report  
>
> - ``PUT /api/play-together/v1/admins/reports/{reportId} ``  
>   ***Description***: Approve or not a report   
>   ***Role Access***: Admin  
>   ***Use for***: Admin make decision approve or not a report  
>

<h2 id="charity">Charity  <a href="#table-of-contents" target="_self">🔙</a></h2> 

> - ``GET /api/play-together/v1/charities ``  
>   ***Description***: Get all Charities   
>   ***Role Access***: Admin, Player  
>   ***Extension***: Paging, Search by Name  
>
> - ``GET /api/play-together/v1/charities/{id} ``  
>   ***Description***: Get a Charity by Id   
>   ***Role Access***: Admin, Player   
>


<h2 id="hirer">Hirer  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/hirers ``  
>   ***Description***: Get all Hirers for Admin  
>   ***Use for***: Get all hirer account in Admin web   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name, filter by status, filter active account 
>
> - ``GET /api/play-together/v1/hirers/profile ``  
>   ***Description***: Get Hirer Profile  
>   ***Use for***: Hirer get their own Profile   
>   ***Role Access***: Hirer  
> 
> - ``GET /api/play-together/v1/hirers/{id} ``  
>   ***Description***: Get a Hirer by Id   
>   ***Use for***: View a specific hirer information  
>   ***Role Access***: Hirer, Player, Admin  
> 
> - ``PUT /api/play-together/v1/hirers/{id} ``  
>   ***Description***: Update Hirer Info    
>   ***Use for***: Update hirer own information    
>   ***Role Access***: Hirer 
>
> - ``GET /api/play-together/v1/hirers/orders ``  
>   ***Description***: Get a Hirer all Orders   
>   ***Use for***: Hirer get their orders    
>   ***Role Access***: Hirer  
>   ***Extension***: Paging, Filter by Status , Order by Create Date  
>
> - ``POST /api/play-together/v1/hirers/orders/{playerId} ``  
>   ***Description***: Create an Order   
>   ***Use for***: Hirer create an Order to Player    
>   ***Role Access***: Hirer    
>
> - ``PUT /api/play-together/v1/hirers/cancel/orders/{orderId} ``  
>   ***Description***: Cancel an order   
>   ***Use for***: Hirer cancel order    
>   ***Role Access***: Hirer    
>


<h2 id="player">Player  <a href="#table-of-contents" target="_self">🔙</a></h2>    

> - ``GET /api/play-together/v1.1/players ``  
>   ***Description***: Get all Players for Hirer  
>   ***Use for***: Search players in Hirer app   
>   ***Role Access***: Hirer  
>   ***Extension***: Search by name of Player or Game or Game Type, Paging, Filter by Gender, Game, Music, Name, Order by First Name, Rating, Pricing, Recent hired  
>
> - ``GET /api/play-together/v1/players/profile ``  
>   ***Description***: Get Player Profile  
>   ***Use for***: Player get their own Profile   
>   ***Role Access***: Player  
> 
> - ``GET /api/play-together/v1/players/{id} ``  
>   ***Description***: Get a Player by Id for Player   
>   ***Use for***: Player get their own information  
>   ***Role Access***: Player 
>
> - ``GET /api/play-together/v1.1/players/{id} ``  
>   ***Description***: Get a Player by Id for Hirer   
>   ***Use for***: Hire get player by Id to view their info  
>   ***Role Access***: Hirer  
>
> - ``PUT /api/play-together/v1/players/{id} ``  
>   ***Description***: Update Player Info    
>   ***Use for***: Update player own information    
>   ***Role Access***: Player  
>
> - ``GET /api/play-together/v1/players/service-info/{id} ``  
>   ***Description***: Get a Player service info (like is ready for hire, price per hour,...) by Id for Player   
>   ***Use for***: Player get their own service information    
>   ***Role Access***: Player  
>
> - ``PUT /api/play-together/v1/players/service-info/{id} ``  
>   ***Description***: Update Player service info (like is ready for hire, price per hour,...)    
>   ***Use for***: Update player own service information    
>   ***Role Access***: Player  
>
> - ``GET /api/play-together/v1/players/{playerId}/games ``  
>   ***Description***: Get all Games of Player   
>   ***Use for***: View all game of specific player  
>   ***Role Access***: Player, Hirer  
>
> - ``POST /api/play-together/v1/players/{playerId}/games ``  
>   ***Description***: Create a New Game for Player   
>   ***Use for***: Player add new a game which him/her can play  
>   ***Role Access***: Player  
>
> - ``GET /api/play-together/v1/players/{playerId}/musics ``  
>   ***Description***: Get all Musics of Player   
>   ***Use for***: View all musics of specific player  
>   ***Role Access***: Player, Hirer  
>
> - ``POST /api/play-together/v1/players/{playerId}/musics ``  
>   ***Description***: Create a New Music for Player   
>   ***Use for***: Player add new a music which him/her can play  
>   ***Role Access***: Player  
>
> - ``GET /api/play-together/v1/players/{id}/other-skills ``  
>   ***Description***: Get a Player other Skills   
>   ***Use for***: Player get their other skills    
>   ***Role Access***: Player  
>
> - ``PUT /api/play-together/v1/players/{id}/other-skills ``  
>   ***Description***: Update Player other Skill    
>   ***Use for***: Update player other skill    
>   ***Role Access***: Player 
>
> - ``GET /api/play-together/v1/players/orders ``  
>   ***Description***: Get a Player all Orders   
>   ***Use for***: Player get their orders    
>   ***Role Access***: Player  
>   ***Extension***: Paging, Filter by status, Order by Create Date    
>
> - ``PUT /api/play-together/v1/players/orders/{orderId}/process ``  
>   ***Description***: Process the incoming order (Accept or reject)   
>   ***Use for***: Accept or reject a order request    
>   ***Role Access***: Player  
>
> - ``PUT ​/api​/play-together​/v1​/players​/accept-policy ``  
>   ***Description***: Player Accept the Policy   
>   ***Use for***: Player confirm that his/her accept or not the policy    
>   ***Role Access***: Player  
>


<h2 id="order">Order  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/orders/{id} ``  
>   ***Description***: Get order by Id  
>   ***Use for***: Get order by Id  
>   ***Role Access***: Hirer, Player  
>
> - ``PUT /api/play-together/v1/orders/finish/{id} ``  
>   ***Description***: Finish the order  
>   ***Use for***: Finish the order when end of time, auto call this api from Player App   
>   ***Role Access***: Player   
>
> - ``PUT /api/play-together/v1/orders/finish-soon/{id} ``  
>   ***Description***: Finish soon order  
>   ***Use for***: Hirer or Player make finish soon order request   
>   ***Role Access***: Hirer, Player   
>


<h2 id="game-type">Game Type  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``GET /api/play-together/v1/game-types ``  
>   ***Description***: Get all Game Types  
>   ***Use for***: Admin get to view all game types  
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name  
>
> - ``GET /api/play-together/v1/game-types/{id} ``  
>   ***Description***: Get a Game Type By Id  
>   ***Use for***: Admin view a game type in detail  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/game-types ``  
>   ***Description***: Add New a Game Type  
>   ***Use for***: Admin add new game type   
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/game-types/{id} ``  
>   ***Description***: Update a Game Type  
>   ***Use for***: Admin update game type    
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/game-types/{id} ``  
>   ***Description***: Delete a Game Type       
>   ***Use for***: Admin delete a game type  
>   ***Role Access***: Admin  
>


<h2 id="types-of-game">Types Of Game  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``GET /api/play-together/v1/types-of-game/{id} ``  
>   ***Description***: Get Type of Game  
>   ***Use for***: Support to view types of game  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/types-of-game ``  
>   ***Description***: Add type of game  
>   ***Use for***: Support to add type of game   
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/types-of-game/{id} ``  
>   ***Description***: Delete type of game    
>   ***Use for***: Delete type of game  
>   ***Role Access***: Admin  
>


<h2 id="game">Game  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/games ``  
>   ***Description***: Get all Games   
>   ***Use for***: Get to view all games  
>   ***Role Access***: Admin, Player, Hirer  
>   ***Extension***: Paging, Search by Name  
>
> - ``GET /api/play-together/v1/games/{gameId}/ranks ``  
>   ***Description***: Get all Ranks in Game   
>   ***Use for***: Get to view all ranks in game  
>   ***Role Access***: Admin, Player  
>
> - ``POST /api/play-together/v1/games/{gameId}/ranks ``  
>   ***Description***: Create a rank in game   
>   ***Use for***: Add rank to game  
>   ***Role Access***: Admin  
>
> - ``GET /api/play-together/v1/games/{id} ``  
>   ***Description***: Get a Game By Id  
>   ***Use for***: View a game in detail  
>   ***Role Access***: Admin, Player, Hirer  
>
> - ``POST /api/play-together/v1/games ``  
>   ***Description***: Add New a Game  
>   ***Use for***: Admin add new game   
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/games/{id} ``  
>   ***Description***: Update a Game  
>   ***Use for***: Admin update game    
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/games/{id} ``  
>   ***Description***: Delete a Game       
>   ***Use for***: Admin delete a game  
>   ***Role Access***: Admin  
>


<h2 id="rank">Rank  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/ranks/{id} ``  
>   ***Description***: Get Rank by Id  
>   ***Use for***: Get rank by Id  
>   ***Role Access***: Admin, Player  
>
> - ``PUT /api/play-together/v1/ranks/{id} ``  
>   ***Description***: Update rank  
>   ***Use for***: Update rank info   
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/ranks/{id} ``  
>   ***Description***: Delete rank    
>   ***Use for***: Delete rank  
>   ***Role Access***: Admin  
>


<h2 id="games-of-player">Games Of Player  <a href="#table-of-contents" target="_self">🔙</a></h2> 

> - ``GET /api/play-together/v1/games-of-player/{id} ``  
>   ***Description***: Get Game of Player by Id  
>   ***Use for***: Get Game of Player  
>   ***Role Access***: Player  
>
> - ``PUT /api/play-together/v1/games-of-player/{id} ``  
>   ***Description***: Update Game of Player  
>   ***Use for***: Update Game of Player   
>   ***Role Access***: Player  
> 
> - ``DELETE /api/play-together/v1/games-of-player/{id} ``  
>   ***Description***: Delete Game of Player    
>   ***Use for***: Delete Game of Player  
>   ***Role Access***: Player  
>


<h2 id="music">Music  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/musics ``  
>   ***Description***: Get all Musics   
>   ***Use for***: Get all musics  
>   ***Role Access***: Admin, Player  
>   ***Extension***: Paging, Search by Name  
>
> - ``GET /api/play-together/v1/musics/{id} ``  
>   ***Description***: Get Music By Id  
>   ***Use for***: View music in detail  
>   ***Role Access***: Admin, Player   
>
> - ``POST /api/play-together/v1/musics ``  
>   ***Description***: Add New Music  
>   ***Use for***: Add music   
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/musics/{id} ``  
>   ***Description***: Update Music  
>   ***Use for***: Update music    
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/musics/{id} ``  
>   ***Description***: Delete Music       
>   ***Use for***: Delete music  
>   ***Role Access***: Admin  
>


<h2 id="musics-of-player">Musics Of Player  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/musics-of-player/{id} ``  
>   ***Description***: Get Music of Player by Id  
>   ***Use for***: Get Music of Player  
>   ***Role Access***: Player  
> 
> - ``DELETE /api/play-together/v1/musics-of-player/{id} ``  
>   ***Description***: Delete Music of Player    
>   ***Use for***: Delete Music of Player  
>   ***Role Access***: Player  
>


<h2 id="image">Image  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/images/{id} ``  
>   ***Description***: Get Image by Id for Player and Hirer  
>   ***Use for***: View image of Player   
>   ***Role Access***: Player, Hirer  
>
> - ``POST /api/play-together/v1/images ``  
>   ***Description***: Add New an Image of Player   
>   ***Use for***: Player add a new image to their image collection   
>   ***Role Access***: Player  
> 
> - ``DELETE /api/play-together/v1/images/{id} ``  
>   ***Description***: Delete an Image of Player    
>   ***Use for***: Player delete an image from their image collection  
>   ***Role Access***: Player  
>


<h2 id="rating">Rating  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/rating/{playerId} ``  
>   ***Description***: Get all Ratings of a Player   
>   ***Use for***: View all ratings of one specific Player   
>   ***Role Access***: Player, Hirer  
>   ***Extension***: Paging, Order by Created Date, Filter by number of star vote  
>
> - ``GET /api/play-together/v1/rating/violates ``  
>   ***Description***: Get all Violate Ratings of a Player   
>   ***Use for***: Admin get all violate ratings   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Order by Created Date, Filter by active/ disable violate ratings  
>
> - ``POST /api/play-together/v1/rating/{orderId} ``  
>   ***Description***: Create a Rating for Player base in current Order  
>   ***Use for***: Make rate, feedback about player after order his/her  
>   ***Role Access***: Hirer    
>
> - ``PUT /api/play-together/v1/rating/violate/{rateId} ``  
>   ***Description***: Report violate Feedback   
>   ***Use for***: Player report the violate feedback   
>   ***Role Access***: Player    
>
> - ``PUT /api/play-together/v1/rating/disable/{rateId} ``  
>   ***Description***: Disable violate Feedback  
>   ***Use for***: Admin disable the violate feedback  
>   ***Role Access***: Admin 


<h2 id="report">Report  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``POST /api/play-together/v1/reports/{orderId} ``  
>   ***Description***: Create a report base on an Order   
>   ***Use for***: Player report Hirer violate in a specific order   
>   ***Role Access***: Player   
>
> - ``GET /api/play-together/v1/reports/{hirerId} ``  
>   ***Description***: Get all report of a specific Hirer   
>   ***Use for***: View reports of a hirer, help player make decision for accept the order or not,...     
>   ***Role Access***: Player, Hirer, Admin   
>


<h2 id="notification">Notification  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/notification ``  
>   ***Description***: Get all Notifications   
>   ***Use for***: View all notifications  
>   ***Role Access***: Admin, Player, Hirer, Charity  
>   ***Extension***: Paging, Filter not read or read notification, Order by Created Date  
>
> - ``GET /api/play-together/v1/notification/{id} ``  
>   ***Description***: Get Notification By Id  
>   ***Use for***: View Notification in detail  
>   ***Role Access***: Admin, Player, Hirer, Charity   
> 
> - ``DELETE /api/play-together/v1/notification/{id} ``  
>   ***Description***: Delete Notification       
>   ***Use for***: Delete notification  
>   ***Role Access***: Admin, Player, Hirer, Charity  
>


<h2 id="chat">Chat  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/chats/{receiveId} ``  
>   ***Description***: Get all chats of current user and specific receiver or partner   
>   ***Use for***: View all chats  
>   ***Role Access***: Admin, Player, Hirer, Charity  
>   ***Extension***: Paging, Order by Created Date  
>
> - ``POST /api/play-together/v1/chats/{receiveId} ``  
>   ***Description***: Create a chat message to a specific receiver  
>   ***Use for***: Chat with specific user or partner  
>   ***Role Access***: Admin, Player, Hirer, Charity   
> 
> - ``DELETE /api/play-together/v1/chats/{chatId} ``  
>   ***Description***: Remove chat       
>   ***Use for***: Remover wrong chat  
>   ***Role Access***: Admin, Player, Hirer, Charity  
>

<h2 id="api">PlayTogether API  <a href="#table-of-contents" target="_self">🔙</a></h2>   