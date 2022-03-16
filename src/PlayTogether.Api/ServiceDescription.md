<h1 id="services-description">Services Description</h2> 

<h2 id="table-of-contents">Table Of Contents</h2>  
- <a href="#services-description" target="_self">Title</a> <br>
- <a href="#table-of-contents" target="_self">Table Of Contents</a> <br>
- <a href="#account" target="_self">Account</a> <br>
- <a href="#user" target="_self">User</a> <br>
- <a href="#hobby" target="_self">Hobby</a> <br>
- <a href="#game-type" target="_self">Game Type</a> <br>
- <a href="#types-of-game" target="_self">Types Of Game</a> <br>
- <a href="#game" target="_self">Game</a> <br>
- <a href="#rank" target="_self">Rank</a> <br>
- <a href="#api" target="_self">PlayTogether API</a> <br>


<h2 id="account">Account <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``POST /api/play-together/v1/accounts/login-user ``  
>   ***Description***: Login User Account  
>   ***Use for***: Login User and update status of User to Online  
>
> - ``POST /api/play-together/v1/accounts/login-admin ``  
>   ***Description***: Login Admin Account   
>
> - ``POST /api/play-together/v1/accounts/login-charity ``  
>   ***Description***: Login Charity Account  
>
> - ``POST /api/play-together/v1/accounts/login-google ``  
>   ***Description***: Sign in with Google
>
> - ``PUT /api/play-together/v1/accounts/logout ``  
>   ***Description***: Logout Hirer or Player  
>   ***Role Access***: User    
>   ***Use for***: Update status of User to Offline
>
> - ``POST /api/play-together/v1/accounts/register-admin ``  
>   ***Description***: Register Admin  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/accounts/register-charity ``  
>   ***Description***: Register Charity  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/accounts/register-user ``  
>   ***Description***: Register a user Account  
>
> - ``PUT /api/play-together/v1/accounts/change-password ``  
>   ***Description***: Change password of account  
>   ***Role Access***: User, Charity, Admin  
>
> - ``PUT /api/play-together/v1/accounts/reset-password-admin ``  
>   ***Description***: Reset password for admin  
>   ***Role Access***: Admin  
>   ***Use for***: Reset password of an Admin account
>
> - ``PUT /api/play-together/v1/accounts/reset-password-token ``  
>   ***Description***: Get token for reset password  
>   ***Role Access***: User, Charity  
>   ***Use for***: Get token for reset password, go with verify email
>
> - ``PUT /api/play-together/v1/accounts/reset-password ``  
>   ***Description***: Reset password  
>   ***Role Access***: User, Charity  
>
> - ``POST /api/play-together/v1/accounts/register-multi-player ``  
>   ***Description***: Register multi Player Account **for TEST**  
>
> - ``POST /api/play-together/v1/accounts/register-multi-user ``  
>   ***Description***: Register multi Normal User Account **for TEST**  
>
> - ``GET /api/play-together/v1/accounts/check-exist-email ``  
>   ***Description***: Check Exist Email
>


<h2 id="user">User <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``GET /api/play-together/v1/users/personal ``  
>   ***Description***: Get user personal profile  
>   ***Use for***: View personal profile, check status  
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/users/{userId}/hobbies ``  
>   ***Description***: Get all hobbies of a specific user  
>   ***Use for***: View hobbies of a specific user  
>   ***Role Access***: User  
>



<h2 id="hobby">Hobby <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``POST /api/play-together/v1/hobbies ``  
>   ***Description***: Create hobbies   
>   ***Role Access***: User   
>
> - ``DELETE /api/play-together/v1/hobbies/{hobbyId} ``  
>   ***Description***: Delete a hobby  
>   ***Role Access***: User  
>




<h2 id="game-type">Game Type  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``GET /api/play-together/v1/game-types ``  
>   ***Description***: Get all Game Types  
>   ***Use for***: Get all game types  
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by info input  
>
> - ``GET /api/play-together/v1/game-types/{gameTypeId} ``  
>   ***Description***: Get a Game Type By Id  
>   ***Use for***: View a game type in detail  
>   ***Role Access***: Admin, User  
>
> - ``POST /api/play-together/v1/game-types ``  
>   ***Description***: Add New a Game Type  
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/game-types/{gameTypeId} ``  
>   ***Description***: Update a Game Type  
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/game-types/{gameTypeId} ``  
>   ***Description***: Delete a Game Type  
>   ***Role Access***: Admin  
>


<h2 id="types-of-game">Types Of Game  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``GET /api/play-together/v1/types-of-game/{typeOfGameId} ``  
>   ***Description***: Get Type of Game by Id   
>   ***Role Access***: Admin, User  
>
> - ``POST /api/play-together/v1/types-of-game ``  
>   ***Description***: Add type of game  
>   ***Use for***: Support to add type of game   
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/types-of-game/{typeOfGameId} ``  
>   ***Description***: Delete type of game    
>   ***Use for***: Delete type of game  
>   ***Role Access***: Admin  
>


<h2 id="game">Game  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/games ``  
>   ***Description***: Get all Games   
>   ***Use for***: View all games  
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by Name  
>
> - ``GET /api/play-together/v1/games/{gameId}/ranks ``  
>   ***Description***: Get all Ranks in Game   
>   ***Use for***: View all ranks in a game  
>   ***Role Access***: Admin, User  
>
> - ``POST /api/play-together/v1/games/{gameId}/ranks ``  
>   ***Description***: Create a rank in game   
>   ***Use for***: Add rank to game  
>   ***Role Access***: Admin  
>
> - ``GET /api/play-together/v1/games/{gameId} ``  
>   ***Description***: Get a Game By Id  
>   ***Use for***: View a game in detail  
>   ***Role Access***: Admin, User  
>
> - ``POST /api/play-together/v1/games ``  
>   ***Description***: Add New a Game  
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/games/{gameId} ``  
>   ***Description***: Update a Game  
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/games/{gameId} ``  
>   ***Description***: Delete a Game    
>   ***Role Access***: Admin  
>


<h2 id="rank">Rank  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/ranks/{rankId} ``  
>   ***Description***: Get Rank by Id  
>   ***Use for***: Get rank by Id  
>   ***Role Access***: Admin, User  
>
> - ``PUT /api/play-together/v1/ranks/{rankId} ``  
>   ***Description***: Update rank  
>   ***Use for***: Update rank info   
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/ranks/{rankId} ``  
>   ***Description***: Delete rank    
>   ***Use for***: Delete rank  
>   ***Role Access***: Admin  
>


<h2 id="api">PlayTogether API  <a href="#table-of-contents" target="_self">🔙</a></h2>   

