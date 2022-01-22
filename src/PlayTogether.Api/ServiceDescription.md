# Service Description #
    

> #### Auth ####
>
> - ``POST: `/api/play-together/v1/auth/login` ``  
>   ***Description***: Login all user account
>
> - ``GET: `/api/play-together/v1/auth/check-exist-email` ``  
>   ***Description***: Check exist email
>
> - ``POST: `/api/play-together/v1/auth/login-google-player` ``  
>   ***Description***: Sign in with Google for player
>
> - ``POST: `/api/play-together/v1/auth/login-google-hirer` ``  
>   ***Description***: Sign in with Google for hirer
>
> - ``POST: `/api/play-together/v1/auth/register-admin` ``  
>   ***Description***: Register admin  
>   ***Role Access***: Admin  
>
> - ``POST: `/api/play-together/v1/auth/register-charity` ``  
>   ***Description***: Register charity  
>   ***Role Access***: Admin  
>
> - ``POST: `/api/play-together/v1/auth/register-player` ``  
>   ***Description***: Register a normal player account  
>
> - ``POST: `/api/play-together/v1/auth/register-hirer` ``  
>   ***Description***: Register a normal hirer account  
>
>

> #### Admin ####
>
> - ``GET: /api/play-together/v1/admins ``  
>   ***Description***: Get all admins   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, search by name  
>

> #### Charity ####
>
> - ``GET: /api/play-together/v1/charities ``  
>   ***Description***: Get all charities   
>   ***Role Access***: Admin, Player  
>   ***Extension***: Paging, search by name  
>

> #### Player ####
>
> - ``GET: /api/play-together/v1/players ``  
>   ***Description***: Get all players   
>   ***Role Access***: Admin, Hirer  
>   ***Extension***: Paging, filter by gender, search by name 
>

> #### Hirer ####
>
> - ``GET: /api/play-together/v1/hirers ``  
>   ***Description***: Get all hirers   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, search by name 
>
