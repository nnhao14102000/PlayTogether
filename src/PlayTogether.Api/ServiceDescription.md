<h1 id="services-description">Services Description</h2> 

<h2 id="table-of-contents">Table Of Contents</h2>  
- <a href="#services-description" target="_self">Title</a> <br>
- <a href="#table-of-contents" target="_self">Table Of Contents</a> <br>
- <a href="#account" target="_self">Account</a> <br>
- <a href="#admin" target="_self">Admin</a> <br>
- <a href="#user" target="_self">User</a> <br>
- <a href="#charity" target="_self">Charity</a> <br>
- <a href="#order" target="_self">Order</a> <br>
- <a href="#rating" target="_self">Rating</a> <br>
- <a href="#report" target="_self">Report</a> <br>
- <a href="#hobby" target="_self">Hobby</a> <br>
- <a href="#search-history" target="_self">Search History</a> <br>
- <a href="#game-of-user" target="_self">Games of User</a> <br>
- <a href="#game-type" target="_self">Game Type</a> <br>
- <a href="#types-of-game" target="_self">Types Of Game</a> <br>
- <a href="#game" target="_self">Game</a> <br>
- <a href="#rank" target="_self">Rank</a> <br>
- <a href="#chat" target="_self">Chat</a> <br>
- <a href="#notification" target="_self">Notification</a> <br>
- <a href="#image" target="_self">Image</a> <br>
- <a href="#donate" target="_self">Donate</a> <br>
- <a href="#feedback" target="_self">System Feedback</a> <br>
- <a href="#dating" target="_self">Dating</a> <br>
- <a href="#recommend" target="_self">Recommend</a> <br>
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
>   ***Description***: Logout User  
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
>   ***Use for***: Get token for reset password, go with verify email
>
> - ``PUT /api/play-together/v1/accounts/reset-password ``  
>   ***Description***: Reset password  
>
> - ``POST /api/play-together/v1/accounts/register-multi-User ``  
>   ***Description***: Register multi User Account **for TEST**  
>
> - ``POST /api/play-together/v1/accounts/register-multi-user ``  
>   ***Description***: Register multi Normal User Account **for TEST**  
>
> - ``GET /api/play-together/v1/accounts/check-exist-email ``  
>   ***Description***: Check Exist Email
>



<h2 id="admin">Admin <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``GET /api/play-together/v1/admins/dash-board ``  
>   ***Description***: Get number of Report, DisableUser, SystemFeedback Suggest, New User    
>   ***Role Access***: Admin  
>   ***Use for***: Fill info in Dash Board of Admin 
>
> - ``GET /api/play-together/v1/admins/reports ``  
>   ***Description***: Get all reports of all users   
>   ***Role Access***: Admin  
>   ***Use for***: Admin manage reports 
>
> - ``GET /api/play-together/v1/admins/reports/{reportId} ``  
>   ***Description***: Get a report in detail by report Id   
>   ***Role Access***: Admin  
>   ***Use for***: Admin view more detail to know the reason why User make this report  
>
> - ``PUT /api/play-together/v1/admins/reports/{reportId} ``  
>   ***Description***: Approve or not a report   
>   ***Role Access***: Admin  
>   ***Use for***: Admin make decision approve or not a report  
>
> - ``GET /api/play-together/v1/admins/users ``  
>   ***Description***: Get all users    
>   ***Role Access***: Admin  
>   ***Extension***: Paging, filtering by name, status  
>
> - ``GET /api/play-together/v1/admins/transactions/{userId} ``  
>   ***Description***: Get a specific user transaction histories    
>   ***Role Access***: Admin        
>   ***Extension***: Paging, filtering by type transaction, filter in range of Date, order new transaction   
>
> - ``GET /api/play-together/v1/admins/{userId}/orders ``  
>   ***Description***: Get all Orders of a specific user (Hirer and Player)   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Filter by Status, from day to day, Sort new order  
>
> - ``PUT /api/play-together/v1/admins/charities/{charityId} ``  
>   ***Description***: Disable or Active charity account   
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/admins/users/activate/{userId} ``  
>   ***Description***: Disable or Active user account   
>   ***Role Access***: Admin  
>   ***Note***: if active , just input only 1 field IsActive, else is disable, fill all info input  
> 
> - ``PUT /api/play-together/v1/admins/feedbacks/{feedbackId} ``  
>   ***Description***: Process the feedback   
>   ***Role Access***: Admin  
>
> - ``PUT /api/play-together/v1/admins/feedbacks/{feedbackId} ``  
>   ***Description***: Process the feedback   
>   ***Role Access***: Admin  
>
> - ``GET /api/play-together/v1/admins/train-model ``  
>   ***Description***: Train the model    
>   ***Role Access***: Admin  
>



<h2 id="user">User <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``GET /api/play-together/v1/users/ ``  
>   ***Description***: Get all users / Search users  
>   ***Role Access***: User  
>   ***Extension***: Paging, searching, filtering by name, gameId, gender, status, recent ordered, sort by Name, Price, Rate, New account     
>
> - ``GET /api/play-together/v1/users/personal ``  
>   ***Description***: Get user personal profile  
>   ***Use for***: View personal profile, check status  
>   ***Role Access***: User   
>
> - ``PUT /api/play-together/v1/users/personal ``  
>   ***Description***: Update user personal profile    
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/users/{userId}/hobbies ``  
>   ***Description***: Get all hobbies of a specific user  
>   ***Use for***: View hobbies of a specific user  
>   ***Role Access***: User  
>
> - ``PUT /api/play-together/v1/users/service/{userId} ``  
>   ***Description***: Get a specific user service info    
>   ***Role Access***: User   
>
> - ``PUT /api/play-together/v1/users/service ``  
>   ***Description***: Update user service info    
>   ***Role Access***: User   
>
> - ``PUT /api/play-together/v1/users/player ``  
>   ***Description***: Update user IsPlayer State to Enable or Disable   
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/users/{userId} ``  
>   ***Description***: Get a specific user general info    
>   ***Role Access***: User, Admin   
>
> - ``GET /api/play-together/v1/users/{userId}/games ``  
>   ***Description***: Get a specific user skill (games of user)    
>   ***Role Access***: User   
>
> - ``POST /api/play-together/v1/users/orders/{toUserId} ``  
>   ***Description***: Create a order request to a specific User    
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/users/orders ``  
>   ***Description***: Get all user orders which are created by this User    
>   ***Role Access***: User   
>   ***Extension***: Paging, filtering by status, order by Created Date (is order new)  
>
> - ``PUT /api/play-together/v1/users/orders/cancel/{orderId} ``  
>   ***Description***: Make to cancel an order request from created User  
>   ***User for***: Cancel from created User or auto invoke to cancel if wait for process long more than 1 minute     
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/users/orders/requests ``  
>   ***Description***: Get all user order requests which are receiver is this User  
>   ***Role Access***: User     
>
> - ``PUT /api/play-together/v1/users/orders/{orderId}/process ``  
>   ***Description***: Process an order: accept or not      
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/users/transactions ``  
>   ***Description***: Get all user transaction histories    
>   ***Role Access***: User     
>   ***Extension***: Paging, filtering by type transaction, filter in range of Date, order new transaction   
>
> - ``GET /api/play-together/v1/users/un-active-balance ``  
>   ***Description***: Get all user not active balance    
>   ***Role Access***: User     
>   ***Extension***: Paging, filter in range of Date, order new, filter by status release    
>
> - ``PUT /api/play-together/v1/users/un-active-balance ``  
>   ***Description***: Check to active the un active money    
>   ***Role Access***: User         
>
> - ``POST /api/play-together/v1/users/donates/{charityId} ``  
>   ***Description***: Make donate to a specific charity     
>   ***Role Access***: User         
>
> - ``GET /api/play-together/v1/users/disable ``  
>   ***Description***: Get user disable info    
>   ***Role Access***: User        
>
> - ``PUT /api/play-together/v1/users/active ``  
>   ***Description***: Active user if pass date active  
>   ***Role Access***: User         
>

<h2 id="charity">Charity  <a href="#table-of-contents" target="_self">🔙</a></h2> 

> - ``GET /api/play-together/v1/charities ``  
>   ***Description***: Get all Charities   
>   ***Role Access***: Admin, Player  
>   ***Extension***: Paging, Filter by Name  
>
> - ``GET /api/play-together/v1/charities/{charityId} ``  
>   ***Description***: Get a Charity by Id   
>   ***Role Access***: Admin, Player   
>
> - ``PUT /api/play-together/v1/charities/{charityId} ``  
>   ***Description***: Update a Charity profile   
>   ***Role Access***: Charity, Admin     
>
> - ``GET /api/play-together/v1/charities/personal ``  
>   ***Description***: Get a Charity profile   
>   ***Role Access***: Charity   
>
> - ``GET /api/play-together/v1/charities/dash-board ``  
>   ***Description***: Get a Charity dashboard: number of donate in day, total donate receive in day, total donate, total money receive     
>   ***Role Access***: Charity   
>

<h2 id="order">Order  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/orders/{id} ``  
>   ***Description***: Get order by Order Id  
>   ***Role Access***: User  
>
> - ``GET /api/play-together/v1/orders/detail/{orderId} ``  
>   ***Description***: Get a order in detail   
>   ***Role Access***: Admin, User  
>   ***Use for***: View more detail information of the order   
>
> - ``PUT /api/play-together/v1/orders/finish/{id} ``  
>   ***Description***: Finish the order  
>   ***Use for***: Finish the order when end of time   
>   ***Role Access***: User   
>
> - ``PUT /api/play-together/v1/orders/finish-soon/{id} ``  
>   ***Description***: Finish soon order  
>   ***Use for***: Make finish soon order request   
>   ***Role Access***: User   
>

<h2 id="rating">Rating  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/rating/{userId} ``  
>   ***Description***: Get all Ratings of a User   
>   ***Use for***: View all ratings of one specific User   
>   ***Role Access***: User, Admin  
>   ***Extension***: Paging, Order by Created Date, Filter by number of star vote  
>
> - ``GET /api/play-together/v1/rating/violates ``  
>   ***Description***: Get all Violate Ratings of a User   
>   ***Use for***: Admin get all violate ratings   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Order by Created Date, Filter by active/ disable violate ratings  
>
> - ``POST /api/play-together/v1/rating/{orderId} ``  
>   ***Description***: Create a Rating for User base in current Order  
>   ***Use for***: Make rate, feedback about User after order his/her  
>   ***Role Access***: User    
>
> - ``PUT /api/play-together/v1/rating/violate/{rateId} ``  
>   ***Description***: Report violate Feedback   
>   ***Use for***: User report the violate feedback   
>   ***Role Access***: User    
>
> - ``PUT /api/play-together/v1/rating/process/{rateId} ``  
>   ***Description***: Process violate Feedback  
>   ***Use for***: Admin process the violate feedback  
>   ***Role Access***: Admin


<h2 id="report">Report  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``POST /api/play-together/v1/reports/{orderId} ``  
>   ***Description***: Create a report base on an Order   
>   ***Use for***: User report Hirer violate in a specific order   
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/reports/{userId} ``  
>   ***Description***: Get all report of a specific User   
>   ***Use for***: View reports of a user, help making decision for accept the order or not,...     
>   ***Role Access***: User, Admin   
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



<h2 id="search-history">Search History <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``GET /api/play-together/v1/search-histories ``  
>   ***Description***: Get all search Histories   
>   ***Role Access***: User   
>   ***Use for***: Help user get their search histories   
>   ***Extension***: Paging, Sort by created date, Filter by search content  
>
> - ``DELETE /api/play-together/v1/search-histories/{searchHistoryId} ``  
>   ***Description***: Delete a search history  
>   ***Role Access***: User  
>



<h2 id="game-of-user">Game of User <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``POST /api/play-together/v1/games-of-user ``  
>   ***Description***: Create a skill (Game of User)  
>   ***Role Access***: User   
>
> - ``GET /api/play-together/v1/games-of-user/{gameOfUserId} ``  
>   ***Description***: Get a specific skill (Game of User)  
>   ***Role Access***: User    
>
> - ``PUT /api/play-together/v1/games-of-user/{gameOfUserId} ``  
>   ***Description***: Update a specific skill (Game of User)  
>   ***Role Access***: User    
>
> - ``DELETE /api/play-together/v1/games-of-user/{gameOfUserId} ``  
>   ***Description***: Delete a specific skill (Game of User)  
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
>   ***Extension***: Paging, Filter by Name, Filter most favorite game, sort new game  
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



<h2 id="chat">Chat  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/chats/{receiveId} ``  
>   ***Description***: Get all chats of current user and specific receiver or partner   
>   ***Role Access***: User  
>   ***Extension***: Paging, Order by Created Date  
>
> - ``POST /api/play-together/v1/chats/{receiveId} ``  
>   ***Description***: Create a chat message to a specific receiver  
>   ***Use for***: Chat with specific user or partner  
>   ***Role Access***: User   
> 
> - ``DELETE /api/play-together/v1/chats/{chatId} ``  
>   ***Description***: Remove chat       
>   ***Use for***: Remover wrong chat  
>   ***Role Access***: User  
>


<h2 id="notification">Notification  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``POST /api/play-together/v1/notification ``  
>   ***Description***: Create a Notification   
>   ***Use for***: Send a notification reply to a specific user  
>   ***Role Access***: Admin, User  
>
> - ``GET /api/play-together/v1/notification ``  
>   ***Description***: Get all Notifications   
>   ***Use for***: View all notifications  
>   ***Role Access***: Admin, User, Charity  
>   ***Extension***: Paging, Filter not read or read notification, Order by Created Date  
>
> - ``GET /api/play-together/v1/notification/{id} ``  
>   ***Description***: Get Notification By Id  
>   ***Use for***: View Notification in detail  
>   ***Role Access***: Admin, User, Charity   
> 
> - ``DELETE /api/play-together/v1/notification/{id} ``  
>   ***Description***: Delete Notification       
>   ***Use for***: Delete notification  
>   ***Role Access***: Admin, User, Charity  
>


<h2 id="image">Image  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/images/{id} ``  
>   ***Description***: Get Image by Id for User  
>   ***Use for***: View image of User   
>   ***Role Access***: User  
>
> - ``POST /api/play-together/v1/images ``  
>   ***Description***: Add New an Image of User   
>   ***Use for***: User add a new image to their image collection   
>   ***Role Access***: User  
> 
> - ``DELETE /api/play-together/v1/images/{id} ``  
>   ***Description***: Delete an Image of User    
>   ***Use for***: User delete an image from their image collection  
>   ***Role Access***: User  
>
> - ``POST /api/play-together/v1/images/multi-images ``  
>   ***Description***: Add New multi Images of User   
>   ***Role Access***: User  
> 
> - ``DELETE /api/play-together/v1/images ``  
>   ***Description***: Delete multi Images of User    
>   ***Role Access***: User  
>


<h2 id="donate">Donate  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/play-together/v1/donates ``  
>   ***Description***: Get all donates    
>   ***Role Access***: User, Charity  
>
> - ``GET /api/play-together/v1/donates/{id} ``  
>   ***Description***: Get donate by Id  
>   ***Role Access***: User, Charity   
>

<h2 id="feedback">System Feedback  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``GET /api/play-together/v1/feedbacks ``  
>   ***Description***: Get all feedbacks    
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by type, day, order newest feedback  
>   ***Note***: If role access is admin, it will get all feedbacks, if role access is user , it will get only feedbacks of this user  
>
> - ``GET /api/play-together/v1/feedbacks/{feedbackId} ``  
>   ***Description***: Get a feedback By Id  
>   ***Role Access***: Admin, User  
>
> - ``POST /api/play-together/v1/feedbacks ``  
>   ***Description***: Create feedback  
>   ***Role Access***: User  
>
> - ``PUT /api/play-together/v1/feedbacks/{feedbackId} ``  
>   ***Description***: Update feedback  
>   ***Role Access***: User  
> 
> - ``DELETE /api/play-together/v1/feedbacks/{feedbackId} ``  
>   ***Description***: Delete feedback  
>   ***Role Access***: User    
>


<h2 id="dating">Dating  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``POST /api/play-together/v1/dating ``  
>   ***Description***: Create dating  
>   ***Role Access***: User  
> 
> - ``DELETE /api/play-together/v1/dating/{datingId} ``  
>   ***Description***: Delete dating  
>   ***Role Access***: User    
>

<h2 id="recommend">Recommend  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``POST /api/play-together/v1/recommends ``  
>   ***Description***: Check is Player recommend for User  
>   ***Role Access***: User  
>

<h2 id="api">PlayTogether API  <a href="#table-of-contents" target="_self">🔙</a></h2>   

