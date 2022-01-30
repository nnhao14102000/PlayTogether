# Service Description  
## Auth  
> - ``POST /api/play-together/v1/auth/login ``  
>   ***Description***: Login all User Account
>
> - ``GET /api/play-together/v1/auth/check-exist-email ``  
>   ***Description***: Check Exist Email
>
> - ``POST /api/play-together/v1/auth/login-google-player ``  
>   ***Description***: Sign in with Google for PDlayer
>
> - ``POST /api/play-together/v1/auth/login-google-hirer ``  
>   ***Description***: Sign in with Google for Hirer
>
> - ``POST /api/play-together/v1/auth/register-admin ``  
>   ***Description***: Register Admin  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/auth/register-charity ``  
>   ***Description***: Register Charity  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/auth/register-player ``  
>   ***Description***: Register a Normal Player Account  
>
> - ``POST /api/play-together/v1/auth/register-hirer ``  
>   ***Description***: Register a Normal Hirer Account  
>
>

## Admin   
> - ``GET /api/play-together/v1/admins ``  
>   ***Description***: Get all Admins   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name  
>

## Charity  
> - ``GET /api/play-together/v1/charities ``  
>   ***Description***: Get all Charities   
>   ***Role Access***: Admin, Player  
>   ***Extension***: Paging, Search by Name  
>

## GameType
> - ``GET /api/play-together/v1/gametypes ``  
>   ***Description***: Get all Game Types  
>   ***Use for***: Admin get to view all game types  
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name  
>
> - ``GET /api/play-together/v1/gametypes/{id} ``  
>   ***Description***: Get a Game Type By Id  
>   ***Use for***: Admin view a game type in detail  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/gametypes ``  
>   ***Description***: Add New a Game Type  
>   ***Use for***: Admin add new game type   
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/gametypes/{id} ``  
>   ***Description***: Update a Game Type  
>   ***Use for***: Admin update game type    
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/gametypes/{id} ``  
>   ***Description***: Delete a Game Type       
>   ***Use for***: Admin delete a game type  
>   ***Role Access***: Admin  
>

## Game
> - ``GET /api/play-together/v1/games ``  
>   ***Description***: Get all Games   
>   ***Use for***: Get to view all games  
>   ***Role Access***: Admin, Player  
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
>   ***Role Access***: Admin, Player  
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

## Player  
> - ``GET /api/play-together/v1.1/players ``  
>   ***Description***: Get all Players for Hirer  
>   ***Use for***: Search players in Hirer app   
>   ***Role Access***: Hirer  
>   ***Extension***: Paging, Filter by Gender, Search by Name 
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

## Ranks
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

## TypeOfGame
> - ``GET /api/play-together/v1/typeofgames/{id} ``  
>   ***Description***: Get Type of Game  
>   ***Use for***: Support to view types of game  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/typeofgames ``  
>   ***Description***: Add type of game  
>   ***Use for***: Support to add type of game   
>   ***Role Access***: Admin  
> 
> - ``DELETE /api/play-together/v1/typeofgames/{id} ``  
>   ***Description***: Delete type of game    
>   ***Use for***: Delete type of game  
>   ***Role Access***: Admin  
>

## Image
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


## Hirer 
> - ``GET /api/play-together/v1/hirers ``  
>   ***Description***: Get all Hirers  
>   ***Use for***: Get all hirer account in Admin web   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name 
>
> - ``GET /api/play-together/v1/hirers/profile ``  
>   ***Description***: Get Hirer Profile  
>   ***Use for***: Hirer get their own Profile   
>   ***Role Access***: Hirer  
> 
> - ``GET /api/play-together/v1/hirers/{id} ``  
>   ***Description***: Get a Hirer by Id   
>   ***Use for***: Hirer get their own information for update  
>   ***Role Access***: Hirer  
> 
> - ``PUT /api/play-together/v1/hirers/{id} ``  
>   ***Description***: Update Hirer Info    
>   ***Use for***: Update hirer own information    
>   ***Role Access***: Hirer 
>
## Music
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